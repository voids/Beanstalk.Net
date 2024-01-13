using Beanstalk.Net;
using Beanstalk.Net.Commands;
using JetBrains.Annotations;

namespace Beanstalk.Test.Commands;

[TestClass]
[TestSubject(typeof(Stats))]
public class StatsTest : TestBase {

    [TestMethod]
    public async Task Test() {
        const string str = """
                           ---
                           current-jobs-urgent: 0
                           current-jobs-ready: 0
                           current-jobs-reserved: 0
                           current-jobs-delayed: 0
                           current-jobs-buried: 0
                           cmd-put: 2433347
                           cmd-peek: 0
                           cmd-peek-ready: 0
                           cmd-peek-delayed: 0
                           cmd-peek-buried: 0
                           cmd-reserve: 0
                           cmd-reserve-with-timeout: 6877030
                           cmd-delete: 2433347
                           cmd-release: 0
                           cmd-use: 2078106
                           cmd-watch: 6877030
                           cmd-ignore: 6877030
                           cmd-bury: 0
                           cmd-kick: 0
                           cmd-touch: 0
                           cmd-stats: 1
                           cmd-stats-job: 0
                           cmd-stats-tube: 30
                           cmd-list-tubes: 6
                           cmd-list-tube-used: 1
                           cmd-list-tubes-watched: 1
                           cmd-pause-tube: 0
                           job-timeouts: 3
                           total-jobs: 2433347
                           max-job-size: 65535
                           current-tubes: 6
                           current-connections: 6
                           current-producers: 0
                           current-workers: 5
                           current-waiting: 5
                           total-connections: 531880
                           pid: 587
                           version: "1.12"
                           rusage-utime: 605.702829
                           rusage-stime: 1736.949530
                           uptime: 20210437
                           binlog-oldest-index: 395
                           binlog-current-index: 395
                           binlog-records-migrated: 0
                           binlog-records-written: 4866694
                           binlog-max-size: 10485760
                           draining: false
                           id: 8888888888888888
                           hostname: ecs-0000
                           os: #1 SMP Debian
                           platform: x86_64
                           """;
        var fakeData = Enc.GetBytes(str);
        await _mock(async s => {
            CollectionAssert.AreEqual(Enc.GetBytes($"stats"), await s.ReadBeanstalkHeader());
            await s.WriteAsync(_add_delim($"OK {fakeData.Length}"));
            await s.WriteAsync(_add_delim(fakeData));
        }, async c => {
            using var beanstalk = new BeanstalkConnection(c);
            var cmd = new Stats().OnOk(stats => {
                Assert.IsNotNull(stats);
                Assert.AreEqual("8888888888888888", stats.Id);
                Assert.AreEqual(587, stats.Pid);
                return Task.CompletedTask;
            });
            await beanstalk.Issue(cmd);
        });
    }
}