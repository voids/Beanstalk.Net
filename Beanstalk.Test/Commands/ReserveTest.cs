using System.Text;
using Beanstalk.Net;
using Beanstalk.Net.Commands;
using JetBrains.Annotations;

namespace Beanstalk.Test.Commands;

[TestClass]
[TestSubject(typeof(Reserve))]
public class ReserveTest : TestBase {

    [TestMethod]
    [DataRow("RESERVED")]
    public async Task Test(string respond) {
        const long fakeId = 325;
        var fakeData = Encoding.Default.GetBytes("This a car 🚗!");
        await _mock(async s => {
            var x = Enc.GetBytes($"reserve");
            CollectionAssert.AreEqual(x, await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim($"{respond} {fakeId} {fakeData.Length}"));
            await s.WriteAsync(_add_delim(fakeData));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            var cmd = new Reserve().OnReserved((id, data) => {
                Assert.AreEqual(fakeId, id);
                CollectionAssert.AreEqual(fakeData, data);
                return Task.CompletedTask;
            });
            await beanstalk.Issue(cmd);
        });
    }


    [TestMethod]
    [DataRow("RESERVED", 234U)]
    public async Task TestTimeout(string respond, uint timeout) {
        const long fakeId = 325;
        var fakeData = Encoding.Default.GetBytes("This a car 🚗!");
        await _mock(async s => {
            var x = Enc.GetBytes($"reserve-with-timeout {timeout}");
            CollectionAssert.AreEqual(x, await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim($"{respond} {fakeId} {fakeData.Length}"));
            await s.WriteAsync(_add_delim(fakeData));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            var cmd = new Reserve(timeout).OnReserved((id, data) => {
                Assert.AreEqual(fakeId, id);
                CollectionAssert.AreEqual(fakeData, data);
                return Task.CompletedTask;
            });
            await beanstalk.Issue(cmd);
        });
    }

    [TestMethod]
    [DataRow("DEADLINE_SOON")]
    [DataRow("TIMED_OUT")]
    public async Task TestException(string respond) {
        await _mock(async s => {
            var x = Enc.GetBytes($"reserve");
            CollectionAssert.AreEqual(x, await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim($"{respond}"));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            var cmd = new Reserve();
            await Assert.ThrowsExceptionAsync<BeanstalkException>(async () => {
                await beanstalk.Issue(cmd);
            });
        });
    }
}