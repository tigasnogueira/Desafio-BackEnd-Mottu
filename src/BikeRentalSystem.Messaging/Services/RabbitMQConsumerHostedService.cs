using BikeRentalSystem.Messaging.Interfaces;
using Microsoft.Extensions.Hosting;

namespace BikeRentalSystem.Messaging.Services;

public class RabbitMQConsumerHostedService : BackgroundService
{
    private readonly IEnumerable<IMessageConsumer> _consumers;

    public RabbitMQConsumerHostedService(IEnumerable<IMessageConsumer> consumers)
    {
        _consumers = consumers;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        foreach (var consumer in _consumers)
        {
            consumer.Consume();
        }
        return Task.CompletedTask;
    }
}
