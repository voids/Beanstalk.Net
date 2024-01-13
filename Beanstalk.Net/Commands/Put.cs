using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beanstalk.Net.Commands {
    /// <summary>
    /// The "put" command is for any process that wants to insert a job into the queue.
    /// </summary>
    public class Put : BaseCommand {

        private readonly byte[] _data;

        public Put(byte[] data) {
            _data = data;
        }

        private uint _priority = BeanstalkConnection.DefaultPriority;

        public Put SetPriority(uint priority) {
            _priority = priority;
            return this;
        }

        private uint _delay = BeanstalkConnection.DefaultDelay;

        public Put SetDelay(TimeSpan delay) {
            _delay = (uint)delay.TotalSeconds;
            return this;
        }

        /// <summary>
        /// set delay
        /// </summary>
        /// <param name="delay">second</param>
        /// <returns></returns>
        public Put SetDelay(uint delay) {
            _delay = delay;
            return this;
        }

        private uint _ttr = BeanstalkConnection.DefaultTtr;

        public Put SetTtr(TimeSpan ttr) {
            _ttr = (uint)ttr.TotalSeconds;
            return this;
        }

        /// <summary>
        /// set Time to run
        /// </summary>
        /// <param name="ttr">second</param>
        /// <returns></returns>
        public Put SetTtr(uint ttr) {
            _ttr = ttr;
            return this;
        }

        public Put OnInserted(Func<long, Task> func) {
            X["INSERTED"] = _g(func);
            return this;
        }

        public Put OnBuried(Func<long, Task> func) {
            X["BURIED"] = _g(func);
            return this;
        }

        public Put OnExpectedCrlf(Func<Task> func) {
            X["EXPECTED_CRLF"] = _g(func);
            return this;
        }

        public Put OnJobTooBig(Func<Task> func) {
            X["JOB_TOO_BIG"] = _g(func);
            return this;
        }

        public Put OnDraining(Func<Task> func) {
            X["DRAINING"] = _g(func);
            return this;
        }

        public override IEnumerable<byte[]> Provide() {
            yield return BeanstalkConnection.Enc.GetBytes($"put {_priority} {_delay} {_ttr} {_data.Length}");
            yield return _data;
        }
    }
}