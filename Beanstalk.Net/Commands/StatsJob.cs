using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Beanstalk.Net.Models;

namespace Beanstalk.Net.Commands {
    /// <summary>
    /// The stats-job command gives statistical information about the specified job if it exists.
    /// </summary>
    public class StatsJob : BaseCommand {

        private readonly long _id;

        public StatsJob(long id) {
            _id = id;
        }

        public StatsJob OnOk(Func<JobStats, Task> func) {
            X["OK"] = async (args, stream) => {
                var len = uint.Parse(args[1]);
                var bytes = await stream.ReadBeanstalkBody(len);
                var str = BeanstalkConnection.Enc.GetString(bytes);
                await func.Invoke(BeanstalkConnection.YamlDz.Deserialize<JobStats>(str));
            };
            return this;
        }

        public StatsJob OnNotFound(Func<Task> func) {
            X["NOT_FOUND"] = _g(func);
            return this;
        }

        public override IEnumerable<byte[]> Provide() {
            yield return BeanstalkConnection.Enc.GetBytes($"stats-job {_id}");
        }
    }
}