using Beanstalk.Net;
using Beanstalk.Net.Commands;
using JetBrains.Annotations;

namespace Beanstalk.Test.Commands;

[TestClass]
[TestSubject(typeof(StatsJob))]
public class StatsJobTest : TestBase {

    [TestMethod]
    public async Task Test() {
        const long fakeId = 12993278;
        const string str = """
                           ---
                           id: 12993278
                           tube: default
                           state: ready
                           pri: 1024
                           age: 16
                           delay: 0
                           ttr: 1
                           time-left: 0
                           file: 395
                           reserves: 0
                           timeouts: 0
                           releases: 0
                           buries: 0
                           kicks: 0
                           """;
        var fakeData = Enc.GetBytes(str);
        await _mock(async s => {
            CollectionAssert.AreEqual(Enc.GetBytes($"stats-job {fakeId}"), await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim($"OK {fakeData.Length}"));
            await s.WriteAsync(_add_delim(fakeData));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            var cmd = new StatsJob(fakeId).OnOk(stats => {
                Assert.IsNotNull(stats);
                Assert.AreEqual(fakeId, stats.Id);
                Assert.AreEqual("default", stats.Tube);
                return Task.CompletedTask;
            });
            await beanstalk.Issue(cmd);
        });
    }

    [TestMethod]
    public async Task TestException() {
        const long fakeId = 445;
        await _mock(async s => {
            CollectionAssert.AreEqual(Enc.GetBytes($"stats-job {fakeId}"), await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim("NOT_FOUND"));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            var cmd = new StatsJob(fakeId);
            await Assert.ThrowsExceptionAsync<BeanstalkException>(async () => {
                await beanstalk.Issue(cmd);
            });
        });
    }
}