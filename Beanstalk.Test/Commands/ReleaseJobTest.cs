using Beanstalk.Net;
using Beanstalk.Net.Commands;
using JetBrains.Annotations;

namespace Beanstalk.Test.Commands;

[TestClass]
[TestSubject(typeof(ReleaseJob))]
public class ReleaseJobTest : TestBase {

    [TestMethod]
    [DataRow("RELEASED")]
    public async Task Test(string respond) {
        const long fakeId = 23424;
        const uint fakePri = 244;
        const uint fakeDelay = 8772;
        var cmd = new ReleaseJob(fakeId).SetPriority(fakePri).SetDelay(fakeDelay).OnReleased(() => Task.CompletedTask);
        await _mock(async s => {
            var x = Enc.GetBytes($"release {fakeId} {fakePri} {fakeDelay}");
            CollectionAssert.AreEqual(x, await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim($"{respond}"));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            await beanstalk.Issue(cmd);
        });
    }
    
    [TestMethod]
    [DataRow("BURIED")]
    [DataRow("NOT_FOUND")]
    public async Task TestException(string respond) {
        const long fakeId = 23424;
        const uint fakePri = 244;
        const uint fakeDelay = 8772;
        var cmd = new ReleaseJob(fakeId).SetPriority(fakePri).SetDelay(fakeDelay);
        await _mock(async s => {
            var x = Enc.GetBytes($"release {fakeId} {fakePri} {fakeDelay}");
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