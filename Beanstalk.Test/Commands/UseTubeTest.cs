using Beanstalk.Net;
using Beanstalk.Net.Commands;
using JetBrains.Annotations;

namespace Beanstalk.Test.Commands;

[TestClass]
[TestSubject(typeof(UseTube))]
public class UseTubeTest : TestBase {
    [TestMethod]
    public async Task Test() {
        const string fakeTube = "test-tube";
        await _mock(async s => {
            CollectionAssert.AreEqual(Enc.GetBytes($"use {fakeTube}"), await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim($"USING {fakeTube}"));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            var cmd = new UseTube(fakeTube).OnUsing(tube => {
                Assert.AreEqual(fakeTube, tube);
                return Task.CompletedTask;
            });
            await beanstalk.Issue(cmd);
        });
    }
}