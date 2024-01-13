# Beanstalk.Net
> [Beanstalkd](https://github.com/beanstalkd/beanstalkd) C# client based on .NET Standard 2.0.

## Installing
Install the [Beanstalk.Net NuGet package](https://www.nuget.org/packages/Beanstalk.Net):
```bash
dotnet add package Beanstalk.Net
```

## Quick start

Produce
```csharp
protected async Task AfterBooking(Order order) {
    // after booking
    await beanstalk.Issue(new UseTube("payment-check").OnUsing(null));
    // write order id as bytes
    var data = BitConverter.GetBytes(order.Id);
    // tell the consumer to check the payment status after 15 minutes
    var putCmd = new Put(data).SetDelay(TimeSpan.FromMinutes(15)).OnInserted(async id => {
        _logger.LogDebug("Put ==> Job <{id}>", id);
        // do sth async...
    });
    await beanstalk.Issue(put);
}
```

Consume
```csharp
protected override async Task ExecuteAsync(CancellationToken token) {
    await beanstalk.Watch("payment-check");
    while (!token.IsCancellationRequested) {
        try {
            var reserve = new Reserve(30).OnReserved(async (id, data) => {
                // convert bytes to order id 
                var orderId = BitConverter.ToInt64(data, 0);
                _logger.LogDebug("Reserve ==> job <{id}>", id);
                // close order if not paid
                await HandleOrder(orderid);
                // delete job
                beanstalk.Issue(new DeleteJob(id).OnDeleted(null));
            }).OnTimedOut(null);
            await beanstalk.Issue(put);
        } catch (Exception e) {
            _logger.LogError("{e}", e);
        }
    }
}
```
