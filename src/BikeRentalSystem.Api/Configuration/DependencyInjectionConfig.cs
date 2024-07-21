using BikeRentalSystem.Core.Interfaces;
using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Interfaces.UoW;
using BikeRentalSystem.Core.Notifications;
using BikeRentalSystem.Identity.Extensions;
using BikeRentalSystem.Identity.Interfaces;
using BikeRentalSystem.Identity.Services;
using BikeRentalSystem.Infrastructure.Repositories;
using BikeRentalSystem.Infrastructure.UoW;
using BikeRentalSystem.Messaging.Configurations;
using BikeRentalSystem.Messaging.Interfaces;
using BikeRentalSystem.Messaging.Services;
using BikeRentalSystem.RentalServices.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BikeRentalSystem.Api.Configuration;

public static class DependencyInjectionConfig
{
    public static void AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterManualServices(services);

        services.Scan(scan => scan
            .FromAssemblies(
                typeof(INotifier).Assembly,
                typeof(IRepository<>).Assembly,
                typeof(BaseService).Assembly,
                typeof(IAspNetUser).Assembly,
                typeof(IMessageProducer).Assembly,
                typeof(IMessageConsumer).Assembly,
                typeof(IValidator<>).Assembly
            )
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service") ||
                                                         type.Name.EndsWith("Repository") ||
                                                         type.Name.EndsWith("UnitOfWork") ||
                                                         type.Name.EndsWith("Validation")))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Producer") ||
                                                         type.Name.EndsWith("Consumer") ||
                                                         typeof(IHostedService).IsAssignableFrom(type)))
                .AsImplementedInterfaces()
                .WithSingletonLifetime()
        );

        AddMessagingServices(services, configuration);

        services.AddTransient(provider =>
        {
            var message = "Default message";
            return new Notification(message, NotificationType.Information);
        });
    }

    private static void RegisterManualServices(IServiceCollection services)
    {
        services.TryAddScoped<INotifier, Notifier>();
        services.TryAddScoped<IUnitOfWork, UnitOfWork>();
        services.TryAddScoped<IMotorcycleRepository, MotorcycleRepository>();
        services.TryAddScoped<ICourierRepository, CourierRepository>();
        services.TryAddScoped<IRentalRepository, RentalRepository>();
        services.TryAddScoped<IAspNetUser, AspNetUser>();
        services.TryAddScoped<IAuthService, AuthService>();
        services.TryAddScoped<RoleManager<IdentityRole>>();
        services.AddAuthorization();
        services.TryAddSingleton<IRedisCacheService, RedisCacheService>();
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.TryAddSingleton<IMessageProducer, RabbitMQProducer>();
    }

    private static void AddMessagingServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddRabbitMQ(configuration);
    }
}
