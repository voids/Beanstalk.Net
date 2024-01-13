using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beanstalk.Net.Commands {
    /// <summary>
    /// The list-tube-used command returns the tube currently being used by the client.
    /// </summary>
    public class ListTubeUsed : BaseCommand {

        public ListTubeUsed OnUsing(Func<string, Task> func) {
            X["USING"] = _g(func);
            return this;
        }

        public override IEnumerable<byte[]> Provide() {
            yield return BeanstalkConnection.Enc.GetBytes("list-tube-used");
        }
    }
}