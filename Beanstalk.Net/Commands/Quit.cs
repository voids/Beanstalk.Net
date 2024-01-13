using System.Collections.Generic;

namespace Beanstalk.Net.Commands {
    /// <summary>
    /// The quit command simply closes the connection.
    /// </summary>
    public class Quit : BaseCommand {

        public override IEnumerable<byte[]> Provide() {
            yield return BeanstalkConnection.Enc.GetBytes("quit");
        }
    }
}