using Beanstalk.Net;
using Beanstalk.Net.Commands;
using JetBrains.Annotations;

namespace Beanstalk.Test.Commands;

[TestClass]
[TestSubject(typeof(IgnoreTube))]
public class IgnoreTubeTest : TestBase {

    [TestMethod]
    [DataRow("WATCHING")]
    public async Task Test(string respond) {
        const uint fakeCount = 2;
        const string tube = "test";
        var cmd = new IgnoreTube(tube).OnWatching(count => {
            Assert.AreEqual(fakeCount, count);
            return Task.CompletedTask;
        });
        await _mock(async s => {
            var x = Enc.GetBytes($"ignore {tube}");
            CollectionAssert.AreEqual(x, await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim($"{respond} {fakeCount}"));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            await beanstalk.Issue(cmd);
        });
    }

    [TestMethod]
    [DataRow("NOT_IGNORED")]
    public async Task TestException(string respond) {
        const string tube = "test";
        var cmd = new IgnoreTube(tube);
        await _mock(async s => {
            var x = Enc.GetBytes($"ignore {tube}");
            CollectionAssert.AreEqual(x, await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim($"{respond}"));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            await Assert.ThrowsExceptionAsync<BeanstalkException>(async () => {
                await beanstalk.Issue(cmd);
            });
        });
    }
}