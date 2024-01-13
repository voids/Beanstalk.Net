using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beanstalk.Net.Commands {
    /// <summary>
    /// The release command puts a reserved job back into the ready queue (and marks
    /// its state as "ready") to be run by any client. It is normally used when the job
    /// fails because of a transitory error. 
    /// </summary>
    public class ReleaseJob : BaseCommand {
        private readonly long _id;

        public ReleaseJob(long id) {
            _id = id;
        }


        private uint _priority = BeanstalkConnection.DefaultPriority;

        public ReleaseJob SetPriority(uint priority) {
            _priority = priority;
            return this;
        }

        private uint _delay = BeanstalkConnection.DefaultDelay;

        public ReleaseJob SetDelay(uint delay) {
            _delay = delay;
            return this;
        }

        public ReleaseJob SetDelay(TimeSpan delay) {
            _delay = (uint)delay.TotalSeconds;
            return this;
        }

        public ReleaseJob OnReleased(Func<Task> func) {
            X["RELEASED"] = _g(func);
            return this;
        }

        public ReleaseJob OnBuried(Func<Task> func) {
            X["BURIED"] = _g(func);
            return this;
        }

        public ReleaseJob OnNotFound(Func<Task> func) {
            X["NOT_FOUND"] = _g(func);
            return this;
        }

        public override IEnumerable<byte[]> Provide() {
            yield return BeanstalkConnection.Enc.GetBytes($"release {_id} {_priority} {_delay}");
        }
    }
}