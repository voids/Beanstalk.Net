using Beanstalk.Net;
using Beanstalk.Net.Commands;
using JetBrains.Annotations;

namespace Beanstalk.Test.Commands;

[TestClass]
[TestSubject(typeof(StatsTube))]
public class StatsTubeTest : TestBase {

    [TestMethod]
    public async Task Test() {
        const string fakeTube = "default";
        const string str = """
                           ---
                           name: default
                           current-jobs-urgent: 0
                           current-jobs-ready: 1
                           current-jobs-reserved: 0
                           current-jobs-delayed: 0
                           current-jobs-buried: 0
                           total-jobs: 1
                           current-using: 6
                           current-watching: 1
                           current-waiting: 0
                           cmd-delete: 0
                           cmd-pause-tube: 0
                           pause: 0
                           pause-time-left: 0
                           """;
        var fakeData = Enc.GetBytes(str);
        await _mock(async s => {
            CollectionAssert.AreEqual(Enc.GetBytes($"stats-tube {fakeTube}"), await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim($"OK {fakeData.Length}"));
            await s.WriteAsync(_add_delim(fakeData));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            var cmd = new StatsTube(fakeTube).OnOk(stats => {
                Assert.IsNotNull(stats);
                Assert.AreEqual(fakeTube, stats.Name);
                return Task.CompletedTask;
            });
            await beanstalk.Issue(cmd);
        });
    }

    [TestMethod]
    public async Task TestException() {
        const string fakeTube = "xxxxxx";
        await _mock(async s => {
            CollectionAssert.AreEqual(Enc.GetBytes($"stats-tube {fakeTube}"), await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim("NOT_FOUND"));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            var cmd = new StatsTube(fakeTube);
            await Assert.ThrowsExceptionAsync<BeanstalkException>(async () => {
                await beanstalk.Issue(cmd);
            });
        });
    }
}