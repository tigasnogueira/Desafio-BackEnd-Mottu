using BikeRentalSystem.Core.Interfaces;
using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Interfaces.UoW;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Models.Validations;
using BikeRentalSystem.Core.Notifications;
using BikeRentalSystem.Identity.Extensions;
using BikeRentalSystem.Identity.Interfaces;
using BikeRentalSystem.Identity.Services;
using BikeRentalSystem.Infrastructure.Repositories;
using BikeRentalSystem.Infrastructure.UoW;
using BikeRentalSystem.Messaging.Configurations;
using BikeRentalSystem.Messaging.Consumers;
using BikeRentalSystem.Messaging.Interfaces;
using BikeRentalSystem.Messaging.Services;
using BikeRentalSystem.RentalServices.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace BikeRentalSystem.Api.Configuration;

public static class DependencyInjectionConfig
{
    public static void AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        AddScopedServices(services);
        AddSingletonServices(services);
        AddRepositories(services);
        AddServices(services);
        AddValidators(services);
        AddMessagingServices(services, configuration);
    }

    private static void AddScopedServices(IServiceCollection services)
    {
        services.AddScoped<INotifier, Notifier>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAspNetUser, AspNetUser>();
        services.AddScoped<RoleManager<IdentityRole>>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddAuthorization();
    }

    private static void AddSingletonServices(IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IMessageProducer, RabbitMQProducer>();
        services.AddSingleton<IMessageConsumer, MotorcycleRegisteredConsumer>();
        services.AddSingleton<IMessageConsumer, CourierRegisteredConsumer>();
        services.AddSingleton<IMessageConsumer, RentalRegisteredConsumer>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
        services.AddScoped<ICourierRepository, CourierRepository>();
        services.AddScoped<IRentalRepository, RentalRepository>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IMotorcycleService, MotorcycleService>();
        services.AddScoped<ICourierService, CourierService>();
        services.AddScoped<IRentalService, RentalService>();
        services.AddScoped<IBlobStorageService, BlobStorageService>();
    }

    private static void AddValidators(IServiceCollection services)
    {
        services.AddScoped<IValidator<Motorcycle>, MotorcycleValidation>();
        services.AddScoped<IValidator<Courier>, CourierValidation>();
        services.AddScoped<IValidator<Rental>, RentalValidation>();
    }

    private static void AddMessagingServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddRabbitMQ(configuration);
    }
}
