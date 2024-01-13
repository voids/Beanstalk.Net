using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beanstalk.Net.Commands {
    /// <summary>
    /// The stats command gives statistical information about the system as a whole.
    /// </summary>
    public class Stats : BaseCommand {

        public Stats OnOk(Func<BeanstalkdStats, Task> func) {
            X["OK"] = async (args, stream) => {
                var len = uint.Parse(args[1]);
                var bytes = await stream.ReadBeanstalkBody(len);
                var str = BeanstalkConnection.Enc.GetString(bytes);
                await func.Invoke(BeanstalkConnection.YamlDz.Deserialize<BeanstalkdStats>(str));
            };
            return this;
        }

        public override IEnumerable<byte[]> Provide() {
            yield return BeanstalkConnection.Enc.GetBytes("stats");
        }
    }
}