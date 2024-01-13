using System.Text;
using Beanstalk.Net;
using Beanstalk.Net.Commands;
using JetBrains.Annotations;

namespace Beanstalk.Test.Commands;

[TestClass]
[TestSubject(typeof(Put))]
public class PutTest : TestBase {

    [TestMethod]
    [DataRow("INSERTED", 1024U, 24U, 120U, "你好")]
    [DataRow("BURIED", 1U, 100U, 30U, "Hello world")]
    public async Task TestId(string respond, uint pri, uint delay, uint ttr, string data) {
        const long fakeId = 12345;
        var bytes = Encoding.Default.GetBytes(data);
        var cmd = new Put(bytes).SetPriority(pri).SetDelay(delay).SetTtr(ttr)
            .OnInserted(id => {
                Assert.AreEqual(fakeId, id);
                return Task.CompletedTask;
            }).OnBuried(id => {
                Assert.AreEqual(fakeId, id);
                return Task.CompletedTask;
            });
        await _mock(async s => {
            var x = Enc.GetBytes($"put {pri} {delay} {ttr} {bytes.Length}");
            CollectionAssert.AreEqual(x, await s.ReadBeanstalkHeader());
            CollectionAssert.AreEqual(bytes, await s.ReadBeanstalkBody((uint)bytes.Length));
            await s.WriteAsync(_add_delim($"{respond} {fakeId}"));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            await beanstalk.Issue(cmd);
        });
    }

    [TestMethod]
    [DataRow("OUT_OF_MEMORY")]
    [DataRow("INTERNAL_ERROR")]
    [DataRow("BAD_FORMAT")]
    [DataRow("UNKNOWN_COMMAND")]
    [DataRow("EXPECTED_CRLF")]
    [DataRow("JOB_TOO_BIG")]
    [DataRow("DRAINING")]
    public async Task TestException(string respond) {
        var data = new byte[] { 1, 3, 34, 4, 5, 1, 46, 7 };
        var cmd = new Put(data);
        await _mock(async s => {
            var x = Enc.GetBytes($"put {Pri} {Delay} {Ttr} {data.Length}");
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