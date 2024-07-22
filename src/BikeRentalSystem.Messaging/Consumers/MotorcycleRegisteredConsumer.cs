using BikeRentalSystem.Core.Interfaces.UoW;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Messaging.Events;
using BikeRentalSystem.Messaging.Interfaces;
using Microsoft.Extensions.DependencyInjection;
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
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly string _queueName = "motorcycle_queue";

    public MotorcycleRegisteredConsumer(IModel channel, ILogger<MotorcycleRegisteredConsumer> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _channel = channel ?? throw new ArgumentNullException(nameof(channel));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
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

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    await ProcessMessageAsync(motorcycleRegisteredEvent, unitOfWork);
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message");

                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        _channel.BasicConsume(_queueName, false, consumer);
        await Task.CompletedTask;
    }

    private async Task ProcessMessageAsync(MotorcycleRegistered motorcycleRegisteredEvent, IUnitOfWork unitOfWork)
    {
        _logger.LogInformation("Processing MotorcycleRegistered event: {Event}", motorcycleRegisteredEvent);

        if (motorcycleRegisteredEvent.Year == 2024)
        {
            var existingNotification = await unitOfWork.MotorcycleNotifications
                .Find(mn => mn.MotorcycleId == motorcycleRegisteredEvent.Id && mn.Message == "Motorcycle of year 2024 registered.");

            if (existingNotification.Any())
            {
                _logger.LogInformation("Notification already exists for Motorcycle ID: {MotorcycleId}", motorcycleRegisteredEvent.Id);
                return;
            }

            var notification = new MotorcycleNotification
            {
                MotorcycleId = motorcycleRegisteredEvent.Id,
                Message = "Motorcycle of year 2024 registered.",
                CreatedAt = DateTime.UtcNow,
                CreatedByUser = motorcycleRegisteredEvent.CreatedByUser
            };

            await unitOfWork.MotorcycleNotifications.Add(notification);
            await unitOfWork.SaveAsync();
        }
    }
}
