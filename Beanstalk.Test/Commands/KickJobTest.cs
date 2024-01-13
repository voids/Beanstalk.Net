using Beanstalk.Net;
using Beanstalk.Net.Commands;
using JetBrains.Annotations;

namespace Beanstalk.Test.Commands;

[TestClass]
[TestSubject(typeof(KickJob))]
public class KickJobTest : TestBase {

    [TestMethod]
    [DataRow("KICKED")]
    public async Task Test(string respond) {
        const long fakeId = 123123;
        var cmd = new KickJob(fakeId).OnKicked(() => Task.CompletedTask);
        await _mock(async s => {
            var x = Enc.GetBytes($"kick-job {fakeId}");
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
        const long fakeId = 123123;
        var cmd = new KickJob(fakeId);
        await _mock(async s => {
            var x = Enc.GetBytes($"kick-job {fakeId}");
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