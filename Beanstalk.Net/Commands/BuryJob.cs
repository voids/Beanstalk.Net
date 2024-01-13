using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beanstalk.Net.Commands {
    /// <summary>
    /// The bury command puts a job into the "buried" state. Buried jobs are put into a
    /// FIFO linked list and will not be touched by the server again until a client
    /// kicks them with the "kick" command.
    /// </summary>
    public class BuryJob : BaseCommand {

        private readonly long _id;

        public BuryJob(long id) {
            _id = id;
        }

        private uint _priority = BeanstalkConnection.DefaultPriority;

        public BuryJob SetPriority(uint priority) {
            _priority = priority;
            return this;
        }

        public BuryJob OnBuried(Func<Task> func) {
            X["BURIED"] = _g(func);
            return this;
        }

        public BuryJob OnNotFound(Func<Task> func) {
            X["NOT_FOUND"] = _g(func);
            return this;
        }

        public override IEnumerable<byte[]> Provide() {
            yield return BeanstalkConnection.Enc.GetBytes($"bury {_id} {_priority}");
        }
    }
}