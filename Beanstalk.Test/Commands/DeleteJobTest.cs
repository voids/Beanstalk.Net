using Beanstalk.Net;
using Beanstalk.Net.Commands;
using JetBrains.Annotations;

namespace Beanstalk.Test.Commands;

[TestClass]
[TestSubject(typeof(DeleteJob))]
public class DeleteJobTest : TestBase {

    [TestMethod]
    [DataRow("DELETED")]
    public async Task Test(string respond) {
        const long fakeId = 123;
        var cmd = new DeleteJob(fakeId).OnDeleted(() => Task.CompletedTask);
        await _mock(async s => {
            var x = Enc.GetBytes($"delete {fakeId}");
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
        const long fakeId = 123;
        var cmd = new DeleteJob(fakeId);
        await _mock(async s => {
            var x = Enc.GetBytes($"delete {fakeId}");
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