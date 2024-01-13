using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beanstalk.Net.Commands {
    /// <summary>
    /// Get a newly reserved job.
    /// </summary>
    public class Reserve : BaseCommand {

        private readonly uint? _timeout;

        public Reserve() { }

        public Reserve(uint timeout) {
            _timeout = timeout;
        }

        public Reserve(TimeSpan timeout) {
            _timeout = (uint)timeout.TotalSeconds;
        }

        public Reserve OnReserved(Func<long, byte[], Task> func) {
            X["RESERVED"] = _g(func);
            return this;
        }

        public Reserve OnDeadlineSoon(Func<Task> func) {
            X["DEADLINE_SOON"] = _g(func);
            return this;
        }

        public Reserve OnTimedOut(Func<Task> func) {
            X["TIMED_OUT"] = _g(func);
            return this;
        }

        public override IEnumerable<byte[]> Provide() {
            var cmd = "reserve";
            if (_timeout.HasValue) cmd = $"reserve-with-timeout {_timeout.Value}";
            yield return BeanstalkConnection.Enc.GetBytes(cmd);
        }
    }
}