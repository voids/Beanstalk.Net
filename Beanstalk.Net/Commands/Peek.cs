using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Beanstalk.Net.Models;

namespace Beanstalk.Net.Commands {
    /// <summary>
    /// Inspect a job on the currently used tube.
    /// </summary>
    public class Peek : BaseCommand {

        private readonly State _state;

        public Peek(State state) {
            _state = state;
        }

        public Peek OnFound(Func<long, byte[], Task> func) {
            X["FOUND"] = _g(func);
            return this;
        }

        public Peek OnNotFound(Func<Task> func) {
            X["NOT_FOUND"] = _g(func);
            return this;
        }

        public override IEnumerable<byte[]> Provide() {
            yield return BeanstalkConnection.Enc.GetBytes(
                _state == State.Ready ? "peek-ready"
                : _state == State.Delayed ? "peek-delayed"
                : _state == State.Buried ? "peek-buried"
                : throw new ArgumentOutOfRangeException(_state.ToString())
            );
        }
    }
}