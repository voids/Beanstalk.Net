using System.Text;
using Beanstalk.Net;
using Beanstalk.Net.Commands;
using JetBrains.Annotations;

namespace Beanstalk.Test.Commands;

[TestClass]
[TestSubject(typeof(ReserveJob))]
public class ReserveJobTest : TestBase {

    [TestMethod]
    [DataRow("RESERVED", 34345L)]
    public async Task Test(string respond, long fakeId) {
        var fakeData = Encoding.Default.GetBytes("This a car 🚗!");
        await _mock(async s => {
            var x = Enc.GetBytes($"reserve-job {fakeId}");
            CollectionAssert.AreEqual(x, await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim($"{respond} {fakeId} {fakeData.Length}"));
            await s.WriteAsync(_add_delim(fakeData));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            var cmd = new ReserveJob(fakeId).OnReserved((id, data) => {
                Assert.AreEqual(fakeId, id);
                CollectionAssert.AreEqual(fakeData, data);
                return Task.CompletedTask;
            });
            await beanstalk.Issue(cmd);
        });
    }

    [TestMethod]
    [DataRow("NOT_FOUND", 49L)]
    public async Task TestException(string respond, long fakeId) {
        await _mock(async s => {
            var x = Enc.GetBytes($"reserve-job {fakeId}");
            CollectionAssert.AreEqual(x, await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim($"{respond}"));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            var cmd = new ReserveJob(fakeId);
            await Assert.ThrowsExceptionAsync<BeanstalkException>(async () => {
                await beanstalk.Issue(cmd);
            });
        });
    }
}