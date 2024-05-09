using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Messaging.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace BikeRentalSystem.Messaging.Services;

public class MessagePublisher : IMessagePublisher
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<MessagePublisher> _logger;

    public MessagePublisher(ILogger<MessagePublisher> logger, IOptions<RabbitMQSettings> settings)
    {
        _logger = logger;
        RabbitMQSettings settingsValue = settings.Value;
        var factory = new ConnectionFactory()
        {
            HostName = settingsValue.HostName,
            UserName = settingsValue.UserName,
            Password = settingsValue.Password
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: settingsValue.ExchangeName, type: ExchangeType.Fanout);
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
