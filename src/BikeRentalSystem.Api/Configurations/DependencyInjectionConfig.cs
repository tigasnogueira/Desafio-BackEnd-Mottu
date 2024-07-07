using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Notifications;
using BikeRentalSystem.Infrastructure.Repositories;
using BikeRentalSystem.Messaging.Services;
using BikeRentalSystem.Services.Services;

namespace BikeRentalSystem.Api.Configurations;

public static class DependencyInjectionConfig
{
    public static void AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<INotifier, Notifier>();
        services.AddScoped<ICourierRepository, CourierRepository>();
        services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
        services.AddScoped<IRentalRepository, RentalRepository>();
        services.AddScoped<ICourierService, CourierService>();
        services.AddScoped<IMotorcycleService, MotorcycleService>();
        services.AddScoped<IRentalService, RentalService>();
        services.AddScoped<IMessagePublisher, MessagePublisher>();
    }
}
