using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beanstalk.Net.Commands {
    /// <summary>
    /// Inspect a job in the system with specific job id.
    /// </summary>
    public class PeekJob : BaseCommand {

        private readonly long _id;

        public PeekJob(long id) {
            _id = id;
        }

        public PeekJob OnFound(Func<long, byte[], Task> func) {
            X["FOUND"] = _g(func);
            return this;
        }

        public PeekJob OnNotFound(Func<Task> func) {
            X["NOT_FOUND"] = _g(func);
            return this;
        }

        public override IEnumerable<byte[]> Provide() {
            yield return BeanstalkConnection.Enc.GetBytes($"peek {_id}");
        }
    }
}