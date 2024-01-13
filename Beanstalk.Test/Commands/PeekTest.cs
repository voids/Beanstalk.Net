using Beanstalk.Net;
using Beanstalk.Net.Commands;
using Beanstalk.Net.Models;
using JetBrains.Annotations;

namespace Beanstalk.Test.Commands;

[TestClass]
[TestSubject(typeof(Peek))]
public class PeekTest : TestBase {

    [TestMethod]
    [DataRow("FOUND", State.Delayed, "peek-delayed")]
    [DataRow("FOUND", State.Ready, "peek-ready")]
    [DataRow("FOUND", State.Buried, "peek-buried")]
    public async Task Test(string respond, State state, string x) {
        const long fakeId = 23424;
        var fakeData = new byte[] { 3, 5, 89, 23, 3, 4 };
        var cmd = new Peek(state).OnFound((id, data) => {
            Assert.AreEqual(fakeId, id);
            CollectionAssert.AreEqual(fakeData, data);
            return Task.CompletedTask;
        });
        await _mock(async s => {
            CollectionAssert.AreEqual(Enc.GetBytes(x), await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim($"{respond} {fakeId} {fakeData.Length}"));
            await s.WriteAsync(_add_delim(fakeData));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            await beanstalk.Issue(cmd);
        });
    }

    [TestMethod]
    [DataRow("NOT_FOUND", State.Delayed, "peek-delayed")]
    [DataRow("NOT_FOUND", State.Ready, "peek-ready")]
    [DataRow("NOT_FOUND", State.Buried, "peek-buried")]
    public async Task TestException(string respond, State state, string x) {
        var cmd = new Peek(state);
        await _mock(async s => {
            CollectionAssert.AreEqual(Enc.GetBytes(x), await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim($"{respond}"));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            await Assert.ThrowsExceptionAsync<BeanstalkException>(async () => {
                await beanstalk.Issue(cmd);
            });
        });
    }
}