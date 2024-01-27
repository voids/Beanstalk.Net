using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Beanstalk.Net {

    public static class BeanstalkStreamExtension {
        public static async Task<byte[]> ReadBeanstalkHeader(this Stream stream, CancellationToken token = default) {
            var delimiter = BeanstalkConnection.Delimiter;
            var data = new List<byte>();
            var t = 0;
            var i = 0;
            while (!token.IsCancellationRequested) {
                // todo: "BAD_FORMAT\r\n" explains the max line length is 224 including \r\n, so 256 is good enough for msg header.
                if (t > 256) throw new BeanstalkException("BAD_FORMAT");
                var buf = new byte[1];
                if (await stream.ReadAsync(buf, 0, buf.Length, token) < buf.Length) throw new Exception("BAD_FORMAT");
                data.Add(buf[0]);
                t++;
                if (buf[0] == delimiter[i]) {
                    i++;
                    if (i == delimiter.Length) break;
                } else i = 0;
            }
            return data.Take(t - delimiter.Length).ToArray();
        }

        public static async Task<byte[]> ReadBeanstalkBody(this Stream stream, uint len, CancellationToken token = default) {
            var buf = new byte[len];
            _ = await stream.ReadAsync(buf, 0, buf.Length, token);
            var recycle = new byte[BeanstalkConnection.Delimiter.Length];
            _ = await stream.ReadAsync(recycle, 0, recycle.Length, token);
            return buf;
        }
    }
}