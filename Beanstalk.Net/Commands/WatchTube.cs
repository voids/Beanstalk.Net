using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beanstalk.Net.Commands {
    /// <summary>
    /// The "watch" command adds the named tube to the watch list for the current
    /// connection. A reserve command will take a job from any of the tubes in the
    /// watch list. For each new connection, the watch list initially consists of one
    /// tube, named "default".
    /// </summary>
    public class WatchTube : BaseCommand {

        private readonly string _tube;

        public WatchTube(string tube) {
            _tube = tube;
        }

        public WatchTube OnWatching(Func<uint, Task> func) {
            X["WATCHING"] = _g(func);
            return this;
        }

        public override IEnumerable<byte[]> Provide() {
            yield return BeanstalkConnection.Enc.GetBytes($"watch {_tube}");
        }
    }
}