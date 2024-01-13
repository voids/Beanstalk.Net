using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beanstalk.Net.Commands {
    /// <summary>
    /// The list-tubes command returns a list of all existing tubes.
    /// </summary>
    public class ListTubes : BaseCommand {

        public ListTubes OnOk(Func<List<string>, Task> func) {
            X["OK"] = _g(func);
            return this;
        }

        public override IEnumerable<byte[]> Provide() {
            yield return BeanstalkConnection.Enc.GetBytes("list-tubes");
        }
    }
}