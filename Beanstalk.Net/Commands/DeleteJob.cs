using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beanstalk.Net.Commands {
    /// <summary>
    /// The delete command removes a job from the server entirely. It is normally used
    /// by the client when the job has successfully run to completion. A client can
    /// delete jobs that it has reserved, ready jobs, delayed jobs, and jobs that are
    /// buried.
    /// </summary>
    public class DeleteJob : BaseCommand {

        private readonly long _id;

        public DeleteJob(long id) {
            _id = id;
        }

        public DeleteJob OnDeleted(Func<Task> func) {
            X["DELETED"] = _g(func);
            return this;
        }

        public DeleteJob OnNotFound(Func<Task> func) {
            X["NOT_FOUND"] = _g(func);
            return this;
        }

        public override IEnumerable<byte[]> Provide() {
            yield return BeanstalkConnection.Enc.GetBytes($"delete {_id}");
        }
    }
}