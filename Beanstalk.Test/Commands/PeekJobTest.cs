using Beanstalk.Net;
using Beanstalk.Net.Commands;
using JetBrains.Annotations;

namespace Beanstalk.Test.Commands;

[TestClass]
[TestSubject(typeof(PeekJob))]
public class PeekJobTest : TestBase {

    [TestMethod]
    [DataRow("FOUND", 1234L)]
    public async Task Test(string respond, long fakeId) {
        var fakeData = new byte[] { 3, 5, 89, 23, 3, 4 };
        var cmd = new PeekJob(fakeId).OnFound((id, data) => {
            Assert.AreEqual(fakeId, id);
            CollectionAssert.AreEqual(fakeData, data);
            return Task.CompletedTask;
        });
        await _mock(async s => {
            CollectionAssert.AreEqual(Enc.GetBytes($"peek {fakeId}"), await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim($"{respond} {fakeId} {fakeData.Length}"));
            await s.WriteAsync(_add_delim(fakeData));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            await beanstalk.Issue(cmd);
        });
    }

    [TestMethod]
    [DataRow("NOT_FOUND", 90934L)]
    public async Task TestException(string respond, long fakeId) {
        var cmd = new PeekJob(fakeId);
        await _mock(async s => {
            CollectionAssert.AreEqual(Enc.GetBytes($"peek {fakeId}"), await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim($"{respond}"));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            await Assert.ThrowsExceptionAsync<BeanstalkException>(async () => {
                await beanstalk.Issue(cmd);
            });
        });
    }
}