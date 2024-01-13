using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beanstalk.Net.Commands {
    /// <summary>
    /// The kick-job command is a variant of kick that operates with a single job
    /// identified by its job id. If the given job id exists and is in a buried or
    /// delayed state, it will be moved to the ready queue of the the same tube where it
    /// currently belongs.
    /// </summary>
    public class KickJob : BaseCommand {
        private readonly long _id;

        public KickJob(long id) {
            _id = id;
        }

        public KickJob OnKicked(Func<Task> func) {
            X["KICKED"] = _g(func);
            return this;
        }

        public KickJob OnNotFound(Func<Task> func) {
            X["NOT_FOUND"] = _g(func);
            return this;
        }

        public override IEnumerable<byte[]> Provide() {
            yield return BeanstalkConnection.Enc.GetBytes($"kick-job {_id}");
        }
    }
}