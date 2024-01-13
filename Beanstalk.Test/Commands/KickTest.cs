using Beanstalk.Net;
using Beanstalk.Net.Commands;
using JetBrains.Annotations;

namespace Beanstalk.Test.Commands;

[TestClass]
[TestSubject(typeof(Kick))]
public class KickTest : TestBase {

    [TestMethod]
    [DataRow("KICKED")]
    public async Task Test(string respond) {
        const uint fakeCount = 100;
        const uint bound = 123;
        var cmd = new Kick(bound).OnKicked(count => {
            Assert.AreEqual(fakeCount, count);
            return Task.CompletedTask;
        });
        await _mock(async s => {
            var x = Enc.GetBytes($"kick {bound}");
            CollectionAssert.AreEqual(x, await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim($"{respond} {fakeCount}"));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            await beanstalk.Issue(cmd);
        });
    }
}