using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beanstalk.Net.Commands {
    /// <summary>
    /// Applies only to the currently used tube. It moves jobs into the ready queue.
    /// If there are any buried jobs, it will only kick buried jobs.
    /// Otherwise it will kick delayed jobs.
    /// </summary>
    public class Kick : BaseCommand {

        private readonly uint _bound;

        public Kick(uint bound) {
            _bound = bound;
        }

        public Kick OnKicked(Func<uint, Task> func) {
            X["KICKED"] = _g(func);
            return this;
        }

        public override IEnumerable<byte[]> Provide() {
            yield return BeanstalkConnection.Enc.GetBytes($"kick {_bound}");
        }
    }
}