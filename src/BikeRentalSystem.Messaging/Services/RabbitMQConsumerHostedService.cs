using BikeRentalSystem.Messaging.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BikeRentalSystem.Messaging.Services;

public class RabbitMQConsumerHostedService : BackgroundService
{
    private readonly IEnumerable<IMessageConsumer> _consumers;
    private readonly ILogger<RabbitMQConsumerHostedService> _logger;

    public RabbitMQConsumerHostedService(IEnumerable<IMessageConsumer> consumers, ILogger<RabbitMQConsumerHostedService> logger)
    {
        _consumers = consumers ?? throw new ArgumentNullException(nameof(consumers));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var tasks = _consumers.Select(async consumer =>
        {
            try
            {
                await consumer.ConsumeAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while consuming messages.");
            }
        });

        await Task.WhenAll(tasks);
    }
}
