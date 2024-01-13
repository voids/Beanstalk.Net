using Beanstalk.Net;
using Beanstalk.Net.Commands;

namespace Beanstalk.Sample;

/// <summary>
/// May implements our convenient extensions
/// </summary>
public static class CustomizeExtension {
    public static async Task Watch(this BeanstalkConnection beanstalk, string tube) {
        var watch = new WatchTube(tube).OnWatching(null);
        await beanstalk.Issue(watch);
    }

    public static async Task Reserve(this BeanstalkConnection beanstalk, TimeSpan timeout, Func<long, byte[], Task> func) {
        // Do nothing on timeout prevent exception
        var reserve = new Reserve(timeout).OnReserved(func).OnTimedOut(null);
        await beanstalk.Issue(reserve);
    }

    public static async Task Delete(this BeanstalkConnection beanstalk, long id) {
        var delete = new DeleteJob(id).OnDeleted(null);
        await beanstalk.Issue(delete);
    }
}