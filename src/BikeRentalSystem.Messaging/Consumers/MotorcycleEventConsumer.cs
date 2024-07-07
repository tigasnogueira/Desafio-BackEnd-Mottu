using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Messaging.Configurations;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BikeRentalSystem.Messaging.Consumers;

public class MotorcycleEventConsumer
{
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly ILogger<MotorcycleEventConsumer> _logger;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MotorcycleEventConsumer(IMotorcycleRepository motorcycleRepository, ILogger<MotorcycleEventConsumer> logger, RabbitMQSettings settings)
    {
        _motorcycleRepository = motorcycleRepository;
        _logger = logger;

        var factory = new ConnectionFactory()
        {
            HostName = settings.HostName,
            UserName = settings.UserName,
            Password = settings.Password
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        ConfigureConsumer();
    }

    private void ConfigureConsumer()
    {
        _channel.ExchangeDeclare(exchange: "motorcycle_exchange", type: ExchangeType.Fanout);
        var queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: queueName, exchange: "motorcycle_exchange", routingKey: "");

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            await ProcessMessage(message);
        };

        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        _logger.LogInformation("Motorcycle event consumer configured and listening for messages.");
    }

    private async Task ProcessMessage(string licensePlate)
    {
        try
        {
            var motorcycle = await _motorcycleRepository.GetMotorcycleByLicensePlate(licensePlate);
            if (motorcycle != null && motorcycle.Year == 2024)
            {
                _logger.LogInformation($"Received event for new 2024 motorcycle: License Plate {motorcycle.LicensePlate}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing message for license plate {licensePlate}: {ex.Message}");
        }
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
