using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace BikeRentalSystem.Messaging.Configurations;

public static class RabbitMQConfig
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMQSettings = configuration.GetSection("RabbitMqSettings").Get<RabbitMQSettings>();
        var factory = new ConnectionFactory
        {
            HostName = rabbitMQSettings.HostName,
            Port = rabbitMQSettings.Port,
            UserName = rabbitMQSettings.UserName,
            Password = rabbitMQSettings.Password
        };

        services.AddSingleton(factory);
        services.AddSingleton<IConnection>(sp => sp.GetRequiredService<ConnectionFactory>().CreateConnection());
        services.AddSingleton<IModel>(sp => sp.GetRequiredService<IConnection>().CreateModel());

        return services;
    }
}
