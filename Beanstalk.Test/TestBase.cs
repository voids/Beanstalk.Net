using System.IO.Pipes;
using System.Text;
using Beanstalk.Net;

namespace Beanstalk.Test;

public abstract class TestBase {

    protected const string Pipe = nameof(Pipe);

    protected static readonly Encoding Enc = BeanstalkConnection.Enc;

    protected static readonly byte[] Delim = BeanstalkConnection.Delimiter;

    protected const uint Pri = BeanstalkConnection.DefaultPriority;

    protected const uint Delay = BeanstalkConnection.DefaultDelay;

    protected const uint Ttr = BeanstalkConnection.DefaultTtr;

    protected static byte[] _add_delim(string str) {
        return _add_delim(Enc.GetBytes(str));
    }

    protected static byte[] _add_delim(byte[] bytes) {
        var ret = bytes.ToList();
        ret.AddRange(Delim);
        return ret.ToArray();
    }

    protected static async Task _mock(Func<Stream, Task> sf, Func<Stream, Task> cf) {
        await Task.WhenAll([
            Task.Run(async () => {
                await using var s = new NamedPipeServerStream(Pipe, PipeDirection.InOut);
                await s.WaitForConnectionAsync();
                await sf.Invoke(s);
            }),
            Task.Run(async () => {
                await using var c = new NamedPipeClientStream(".", Pipe, PipeDirection.InOut);
                await c.ConnectAsync();
                await cf.Invoke(c);
            })
        ]);
    }
}