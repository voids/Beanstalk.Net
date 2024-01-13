using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beanstalk.Net.Commands {
    /// <summary>
    /// The list-tubes-watched command returns a list tubes currently being watched by the client.
    /// </summary>
    public class ListTubesWatched : BaseCommand {

        public ListTubesWatched OnOk(Func<List<string>, Task> func) {
            X["OK"] = _g(func);
            return this;
        }

        public override IEnumerable<byte[]> Provide() {
            yield return BeanstalkConnection.Enc.GetBytes("list-tubes-watched");
        }
    }
}