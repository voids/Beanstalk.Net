using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Beanstalk.Net.Models;

namespace Beanstalk.Net.Commands {
    /// <summary>
    /// The stats-tube command gives statistical information about the specified tube if it exists.
    /// </summary>
    public class StatsTube : BaseCommand {
        private readonly string _tube;

        public StatsTube(string tube) {
            _tube = tube;
        }

        public StatsTube OnOk(Func<TubeStats, Task> func) {
            X["OK"] = async (args, stream) => {
                var len = uint.Parse(args[1]);
                var bytes = await stream.ReadBeanstalkBody(len);
                var str = BeanstalkConnection.Enc.GetString(bytes);
                await func.Invoke(BeanstalkConnection.YamlDz.Deserialize<TubeStats>(str));
            };
            return this;
        }

        public StatsTube OnNotFound(Func<Task> func) {
            X["NOT_FOUND"] = _g(func);
            return this;
        }

        public override IEnumerable<byte[]> Provide() {
            yield return BeanstalkConnection.Enc.GetBytes($"stats-tube {_tube}");
        }
    }
}