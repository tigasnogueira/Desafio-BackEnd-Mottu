using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Notifications;
using BikeRentalSystem.Infrastructure.Database;
using BikeRentalSystem.Infrastructure.Repositories;
using BikeRentalSystem.Messaging.Configurations;
using BikeRentalSystem.Messaging.Services;
using BikeRentalSystem.Services.Services;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalSystem.Api.Configurations;

public static class DependencyInjectionConfig
{
    public static void AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        var dbSettings = configuration.GetSection("DatabaseSettings");

        services.AddDbContext<BikeRentalDbContext>(options =>
        {
            options.UseNpgsql(dbSettings.Get<DatabaseSettings>().ConnectionString);
        });

        var rabbitMqSettings = configuration.GetSection("RabbitMqSettings");
        services.Configure<RabbitMQSettings>(rabbitMqSettings);

        services.AddIdentityServerConfiguration();

        services.AddSingleton<INotifier, Notifier>();
        services.AddScoped<ICourierRepository, CourierRepository>();
        services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
        services.AddScoped<IRentalRepository, RentalRepository>();
        services.AddScoped<ICourierService, CourierService>();
        services.AddScoped<IMotorcycleService, MotorcycleService>();
        services.AddScoped<IRentalService, RentalService>();
        services.AddScoped<IMessagePublisher, MessagePublisher>();
    }

    public static void AddIdentityServerConfiguration(this IServiceCollection services)
    {
        services.AddIdentityServer()
            .AddDeveloperSigningCredential() // Use only for development
            .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
            .AddInMemoryApiScopes(IdentityServerConfig.GetApiScopes())
            .AddInMemoryClients(IdentityServerConfig.GetClients());
    }
}
