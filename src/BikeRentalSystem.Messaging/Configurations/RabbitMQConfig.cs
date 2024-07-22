using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace BikeRentalSystem.Messaging.Configurations;

public static class RabbitMQConfig
{
    public static void AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMQSettings = configuration.GetSection("RabbitMqSettings").Get<RabbitMQSettings>();

        var factory = new ConnectionFactory()
        {
            HostName = rabbitMQSettings.HostName,
            Port = rabbitMQSettings.Port,
            UserName = rabbitMQSettings.UserName,
            Password = rabbitMQSettings.Password
        };

        services.AddSingleton(factory);

        services.AddSingleton(sp =>
        {
            var connFactory = sp.GetRequiredService<ConnectionFactory>();
            return connFactory.CreateConnection();
        });

        services.AddSingleton<IModel>(sp =>
        {
            var connection = sp.GetRequiredService<IConnection>();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare("courier_exchange", ExchangeType.Direct, true);
            channel.ExchangeDeclare("motorcycle_exchange", ExchangeType.Direct, true);
            channel.ExchangeDeclare("rental_exchange", ExchangeType.Direct, true);

            channel.QueueDeclare("courier_queue", true, false, false, null);
            channel.QueueDeclare("motorcycle_queue", true, false, false, null);
            channel.QueueDeclare("rental_queue", true, false, false, null);

            channel.QueueBind("courier_queue", "courier_exchange", "courier_routingKey");
            channel.QueueBind("motorcycle_queue", "motorcycle_exchange", "motorcycle_routingKey");
            channel.QueueBind("rental_queue", "rental_exchange", "rental_routingKey");

            return channel;
        });
    }
}
