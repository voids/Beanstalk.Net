using System.Globalization;
using System.Text;
using Beanstalk.Net;
using Beanstalk.Net.Commands;

namespace Beanstalk.Sample;

/// <summary>
/// Producer service example without customize extension
/// </summary>
/// <param name="beanstalk"></param>
public class ProducerService(BeanstalkConnection beanstalk) : BackgroundService {

    protected override async Task ExecuteAsync(CancellationToken token) {
        Console.WriteLine("Start producer");
        await beanstalk.Issue(new UseTube("test").OnUsing(null));
        while (true) {
            if (token.IsCancellationRequested) return;
            Thread.Sleep(TimeSpan.FromSeconds(5));
            var data = Encoding.Default.GetBytes(DateTime.Now.ToString(CultureInfo.InvariantCulture));
            var put = new Put(data).SetTtr(TimeSpan.FromMinutes(2)).OnInserted(id => {
                Console.WriteLine($"Put ==> Job <{id}>");
                return Task.CompletedTask;
            });
            await beanstalk.Issue(put);
        }
    }
}