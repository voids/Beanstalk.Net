using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Beanstalk.Net {

    public delegate Task Handler(string[] args, Stream stream);

    public class BeanstalkConnection : IDisposable {

        public static readonly IDeserializer YamlDz = new DeserializerBuilder().Build();

        public static readonly Encoding Enc = Encoding.ASCII;

        public static readonly byte[] Delimiter = { 0x0D, 0x0A };

        public const uint DefaultPriority = 1024;

        public const uint DefaultDelay = 0;

        public const uint DefaultTtr = 60;

        private readonly TcpClient _client;

        private readonly Stream _stream;

        public BeanstalkConnection(string host, ushort port) {
            _client = new TcpClient(host, port);
            _stream = _client.GetStream();
        }

        public BeanstalkConnection(Stream stream) {
            _stream = Stream.Synchronized(stream);
        }

        public void Dispose() {
            _stream?.Dispose();
            _client?.Dispose();
        }

        /// <summary>
        /// Issue a command to beanstalkd
        /// </summary>
        /// <param name="command"></param>
        /// <param name="token"></param>
        public async Task Issue(IBeanstalkCommand command, CancellationToken token = default) {
            try {
                foreach (var buffer in command.Provide()) {
                    await _stream.WriteAsync(buffer, 0, buffer.Length, token);
                    await _stream.WriteAsync(Delimiter, 0, Delimiter.Length, token);
                }
                var respond = await _stream.ReadBeanstalkHeader(token);
                var respondStr = Enc.GetString(respond);
                var args = respondStr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (args.Length == 0) throw new Exception("Server error.");
                if (!command.Handlers.TryGetValue(args[0], out var handler)) throw new BeanstalkException(args[0]);
                if (handler != null) await handler.Invoke(args, _stream);
            } catch (OperationCanceledException) { }
        }
    }
}