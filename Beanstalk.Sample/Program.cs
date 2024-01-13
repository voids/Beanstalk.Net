using Beanstalk.Net;
using Beanstalk.Sample;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices((_, services) => {
    services.AddHostedService<ConsumerService>();
    services.AddHostedService<ProducerService>();
    // each producer/consumer should use a new connection.
    services.AddTransient<BeanstalkConnection>(_ => new BeanstalkConnection("localhost", 11300));
});
var host = builder.Build();
host.Run();