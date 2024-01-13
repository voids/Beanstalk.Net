using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beanstalk.Net.Commands {
    /// <summary>
    /// The "use" command is for producers. Subsequent put commands will put jobs into
    /// the tube specified by this command. If no use command has been issued, jobs
    /// will be put into the tube named "default".
    /// </summary>
    public class UseTube : BaseCommand {
        private readonly string _tube;

        public UseTube(string tube) {
            _tube = tube;
        }

        public UseTube OnUsing(Func<string, Task> func) {
            X["USING"] = _g(func);
            return this;
        }

        public override IEnumerable<byte[]> Provide() {
            yield return BeanstalkConnection.Enc.GetBytes($"use {_tube}");
        }
    }
}