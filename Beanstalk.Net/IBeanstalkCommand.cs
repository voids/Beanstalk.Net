using System.Collections.Generic;

namespace Beanstalk.Net {
    public interface IBeanstalkCommand {

        IEnumerable<byte[]> Provide();

        IReadOnlyDictionary<string, Handler> Handlers { get; }
    }
}