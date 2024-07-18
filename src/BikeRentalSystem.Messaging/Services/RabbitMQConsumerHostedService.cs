using BikeRentalSystem.Messaging.Interfaces;
using Microsoft.Extensions.Hosting;

namespace BikeRentalSystem.Messaging.Services;

public class RabbitMQConsumerHostedService : BackgroundService
{
    private readonly IEnumerable<IMessageConsumer> _consumers;

    public RabbitMQConsumerHostedService(IEnumerable<IMessageConsumer> consumers)
    {
        _consumers = consumers ?? throw new ArgumentNullException(nameof(consumers));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var tasks = _consumers.Select(consumer => consumer.ConsumeAsync());

        await Task.WhenAll(tasks);
    }
}
