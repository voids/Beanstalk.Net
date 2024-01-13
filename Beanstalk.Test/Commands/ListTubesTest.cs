using Beanstalk.Net;
using Beanstalk.Net.Commands;
using JetBrains.Annotations;

namespace Beanstalk.Test.Commands;

[TestClass]
[TestSubject(typeof(ListTubes))]
public class ListTubesTest : TestBase {

    [TestMethod]
    public async Task Test() {
        var fakeTubes = new List<string> {
            "test1",
            "test2"
        };
        var fakeData = Enc.GetBytes(fakeTubes.Aggregate("---", (s, s1) => s + $"\n- {s1}"));
        var cmd = new ListTubes().OnOk(tubes => {
            CollectionAssert.AreEqual(fakeTubes, tubes);
            return Task.CompletedTask;
        });
        await _mock(async s => {
            var x = Enc.GetBytes("list-tubes");
            CollectionAssert.AreEqual(x, await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim($"OK {fakeData.Length}"));
            await s.WriteAsync(_add_delim(fakeData));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            await beanstalk.Issue(cmd);
        });
    }
}