using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beanstalk.Net.Commands {
    /// <summary>
    /// The pause-tube command can delay any new job being reserved for a given time.
    /// </summary>
    public class PauseTube : BaseCommand {

        private readonly string _tube;

        private readonly uint _delay;

        public PauseTube(string tube, uint delay) {
            _tube = tube;
            _delay = delay;
        }

        public PauseTube(string tube, TimeSpan delay) {
            _tube = tube;
            _delay = (uint)delay.TotalSeconds;
        }

        public PauseTube OnPaused(Func<Task> func) {
            X["PAUSED"] = _g(func);
            return this;
        }

        public PauseTube OnNotFound(Func<Task> func) {
            X["NOT_FOUND"] = _g(func);
            return this;
        }

        public override IEnumerable<byte[]> Provide() {
            yield return BeanstalkConnection.Enc.GetBytes($"pause-tube {_tube} {_delay}");
        }
    }
}