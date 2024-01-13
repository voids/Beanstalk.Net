using Beanstalk.Net;
using Beanstalk.Net.Commands;
using JetBrains.Annotations;

namespace Beanstalk.Test.Commands;

[TestClass]
[TestSubject(typeof(ListTubeUsed))]
public class ListTubeUsedTest : TestBase {

    [TestMethod]
    public async Task Test() {
        const string fakeTube = "test-tube";
        var cmd = new ListTubeUsed().OnUsing(tube => {
            Assert.AreEqual(fakeTube, tube);
            return Task.CompletedTask;
        });
        await _mock(async s => {
            var x = Enc.GetBytes("list-tube-used");
            CollectionAssert.AreEqual(x, await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim($"USING {fakeTube}"));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            await beanstalk.Issue(cmd);
        });
    }
}