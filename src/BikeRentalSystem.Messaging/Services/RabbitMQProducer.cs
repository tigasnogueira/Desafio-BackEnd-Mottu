using BikeRentalSystem.Messaging.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace BikeRentalSystem.Messaging.Services;

public class RabbitMQProducer : IMessageProducer
{
    private readonly IModel _channel;

    public RabbitMQProducer(IModel channel)
    {
        _channel = channel;
    }

    public void Publish<T>(T message, string exchange, string routingKey) where T : class
    {
        var messageBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
        _channel.BasicPublish(exchange, routingKey, null, messageBody);
    }
}
