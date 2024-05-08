using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Messaging.Configurations;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;

namespace BikeRentalSystem.Messaging.Services;

public class MessagePublisher : IMessagePublisher
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<MessagePublisher> _logger;

    public MessagePublisher(ILogger<MessagePublisher> logger, RabbitMQSettings settings)
    {
        _logger = logger;
        var factory = new ConnectionFactory()
        {
            HostName = settings.HostName,
            UserName = settings.UserName,
            Password = settings.Password
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: settings.ExchangeName, type: ExchangeType.Fanout);
    }

    public async Task PublishAsync(string topic, string message)
    {
        try
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "motorcycle_exchange",
                                  routingKey: "",
                                  basicProperties: null,
                                  body: body);
            _logger.LogInformation($"[x] Sent {message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish message");
        }
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
