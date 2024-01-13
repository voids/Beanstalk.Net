using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beanstalk.Net.Commands {
    /// <summary>
    /// The "ignore" command is for consumers. It removes the named tube from the
    /// watch list for the current connection.
    /// </summary>
    public class IgnoreTube : BaseCommand {

        private readonly string _tube;

        public IgnoreTube(string tube) {
            _tube = tube;
        }

        /// <summary>
        /// "WATCHING &lt;count&gt;\r\n" to indicate success
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public IgnoreTube OnWatching(Func<uint, Task> func) {
            X["WATCHING"] = _g(func);
            return this;
        }

        public IgnoreTube OnNotIgnored(Func<Task> func) {
            X["NOT_IGNORED"] = _g(func);
            return this;
        }

        public override IEnumerable<byte[]> Provide() {
            yield return BeanstalkConnection.Enc.GetBytes($"ignore {_tube}");
        }
    }
}