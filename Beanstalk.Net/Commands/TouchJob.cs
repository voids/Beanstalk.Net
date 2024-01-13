using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beanstalk.Net.Commands {
    /// <summary>
    /// The "touch" command allows a worker to request more time to work on a job.
    /// This is useful for jobs that potentially take a long time, but you still want
    /// the benefits of a TTR pulling a job away from an unresponsive worker.  A worker
    /// may periodically tell the server that it's still alive and processing a job
    /// (e.g. it may do this on DEADLINE_SOON). The command postpones the auto
    /// release of a reserved job until TTR seconds from when the command is issued.
    /// </summary>
    public class TouchJob : BaseCommand {

        private readonly long _id;

        public TouchJob(long id) {
            _id = id;
        }

        public TouchJob OnTouched(Func<Task> func) {
            X["TOUCHED"] = _g(func);
            return this;
        }


        public TouchJob OnNotFound(Func<Task> func) {
            X["NOT_FOUND"] = _g(func);
            return this;
        }

        public override IEnumerable<byte[]> Provide() {
            yield return BeanstalkConnection.Enc.GetBytes($"touch {_id}");
        }
    }
}