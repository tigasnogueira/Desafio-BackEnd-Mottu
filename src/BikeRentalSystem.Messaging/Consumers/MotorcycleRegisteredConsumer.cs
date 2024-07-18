using BikeRentalSystem.Messaging.Events;
using BikeRentalSystem.Messaging.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BikeRentalSystem.Messaging.Consumers;

public class MotorcycleRegisteredConsumer : IMessageConsumer
{
    private readonly IModel _channel;
    private readonly ILogger<MotorcycleRegisteredConsumer> _logger;
    private readonly string _queueName = "rental_queue";

    public MotorcycleRegisteredConsumer(IModel channel, ILogger<MotorcycleRegisteredConsumer> logger)
    {
        _channel = channel ?? throw new ArgumentNullException(nameof(channel));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task ConsumeAsync()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var motorcycleRegisteredEvent = JsonConvert.DeserializeObject<MotorcycleRegistered>(message);

                await ProcessMessageAsync(motorcycleRegisteredEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message");
            }
        };

        _channel.BasicConsume(_queueName, true, consumer);
        await Task.CompletedTask;
    }

    private Task ProcessMessageAsync(MotorcycleRegistered motorcycleRegisteredEvent)
    {
        _logger.LogInformation("Processing MotorcycleRegistered event: {Event}", motorcycleRegisteredEvent);
        return Task.CompletedTask;
    }
}
