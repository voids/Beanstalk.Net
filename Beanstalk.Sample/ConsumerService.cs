using System.Text;
using Beanstalk.Net;

namespace Beanstalk.Sample;

/// <summary>
///  Consumer service example customize extension
/// </summary>
/// <param name="beanstalk"></param>
public class ConsumerService(BeanstalkConnection beanstalk) : BackgroundService {

    protected override async Task ExecuteAsync(CancellationToken token) {
        Console.WriteLine("Start consumer");
        await beanstalk.Watch("test");
        while (true) {
            if (token.IsCancellationRequested) return;
            try {
                await beanstalk.Reserve(TimeSpan.FromSeconds(10), HandleJob);
            } catch (BeanstalkException e) {
                Console.WriteLine(e);
            }
        }
    }

    private async Task HandleJob(long id, byte[] data) {
        var message = Encoding.Default.GetString(data);
        Console.WriteLine($"Reserve ==> job <{id}>: {message}");
        await beanstalk.Delete(id);
    }
}