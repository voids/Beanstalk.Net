using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beanstalk.Net.Commands {
    /// <summary>
    /// Get a specific reserved job.
    /// </summary>
    public class ReserveJob : BaseCommand {
        
        private readonly long _id;

        public ReserveJob(long id) {
            _id = id;
        }

        public ReserveJob OnReserved(Func<long, byte[], Task> func) {
            X["RESERVED"] = _g(func);
            return this;
        }

        public ReserveJob OnNotFound(Func<Task> func) {
            X["NOT_FOUND"] = _g(func);
            return this;
        }

        public override IEnumerable<byte[]> Provide() {
            yield return BeanstalkConnection.Enc.GetBytes($"reserve-job {_id}");
        }
    }
}