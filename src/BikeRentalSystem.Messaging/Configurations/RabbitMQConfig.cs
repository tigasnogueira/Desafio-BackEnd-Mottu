using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace BikeRentalSystem.Messaging.Configurations;

public static class RabbitMQConfig
{
    public static void AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        var hostName = configuration["RabbitMqSettings:HostName"];
        var port = configuration["RabbitMqSettings:Port"];
        var userName = configuration["RabbitMqSettings:UserName"];
        var password = configuration["RabbitMqSettings:Password"];

        if (string.IsNullOrEmpty(hostName) || string.IsNullOrEmpty(port) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("RabbitMQ settings are not properly configured.");
        }

        var factory = new ConnectionFactory()
        {
            HostName = hostName,
            Port = int.Parse(port),
            UserName = userName,
            Password = password
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
            return connection.CreateModel();
        });
    }
}
