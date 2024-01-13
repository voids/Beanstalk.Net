using Beanstalk.Net;
using Beanstalk.Net.Commands;
using JetBrains.Annotations;

namespace Beanstalk.Test.Commands;

[TestClass]
[TestSubject(typeof(WatchTube))]
public class WatchTubeTest : TestBase {

    [TestMethod]
    public async Task Test() {
        const string fakeTube = "xxxxxxxxxx";
        const uint fakeCount = 30;
        await _mock(async s => {
            var x = Enc.GetBytes($"watch {fakeTube}");
            CollectionAssert.AreEqual(x, await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim($"WATCHING {fakeCount}"));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            var cmd = new WatchTube(fakeTube).OnWatching(count => {
                Assert.AreEqual(fakeCount, count);
                return Task.CompletedTask;
            });
            await beanstalk.Issue(cmd);
        });
    }
}