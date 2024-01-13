using Beanstalk.Net;
using Beanstalk.Net.Commands;
using JetBrains.Annotations;

namespace Beanstalk.Test.Commands;

[TestClass]
[TestSubject(typeof(TouchJob))]
public class TouchJobTest : TestBase {

    [TestMethod]
    [DataRow("TOUCHED")]
    public async Task Test(string respond) {
        const long fakeId = 123;
        await _mock(async s => {
            var x = Enc.GetBytes($"touch {fakeId}");
            CollectionAssert.AreEqual(x, await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim($"{respond}"));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            var cmd = new TouchJob(fakeId).OnTouched(() => Task.CompletedTask);
            await beanstalk.Issue(cmd);
        });
    }

    [TestMethod]
    [DataRow("NOT_FOUND")]
    public async Task TestException(string respond) {
        const long fakeId = 123;
        await _mock(async s => {
            var x = Enc.GetBytes($"touch {fakeId}");
            CollectionAssert.AreEqual(x, await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim($"{respond}"));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            var cmd = new TouchJob(fakeId);
            await Assert.ThrowsExceptionAsync<BeanstalkException>(async () => {
                await beanstalk.Issue(cmd);
            });
        });
    }
}