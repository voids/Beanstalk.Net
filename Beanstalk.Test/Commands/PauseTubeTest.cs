using Beanstalk.Net;
using Beanstalk.Net.Commands;
using JetBrains.Annotations;

namespace Beanstalk.Test.Commands;

[TestClass]
[TestSubject(typeof(PauseTube))]
public class PauseTubeTest : TestBase {

    [TestMethod]
    [DataRow("PAUSED")]
    public async Task Test(string respond) {
        const string fakeTube = "test-tube";
        const uint fakeDelay = 60;
        var cmd = new PauseTube(fakeTube, fakeDelay).OnPaused(() => Task.CompletedTask);
        await _mock(async s => {
            var x = Enc.GetBytes($"pause-tube {fakeTube} {fakeDelay}");
            CollectionAssert.AreEqual(x, await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim($"{respond}"));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            await beanstalk.Issue(cmd);
        });
    }

    [TestMethod]
    [DataRow("NOT_FOUND")]
    public async Task TestException(string respond) {
        const string fakeTube = "test-tube";
        const uint fakeDelay = 60;
        var cmd = new PauseTube(fakeTube, fakeDelay);
        await _mock(async s => {
            var x = Enc.GetBytes($"pause-tube {fakeTube} {fakeDelay}");
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