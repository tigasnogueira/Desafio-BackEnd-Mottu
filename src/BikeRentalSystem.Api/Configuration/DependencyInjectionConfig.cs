using BikeRentalSystem.Core.Interfaces;
using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Models.Validations;
using BikeRentalSystem.Core.Notifications;
using BikeRentalSystem.Identity.Extensions;
using BikeRentalSystem.Identity.Interfaces;
using BikeRentalSystem.Identity.Services;
using BikeRentalSystem.Infrastructure.Context;
using BikeRentalSystem.Infrastructure.Repositories;
using BikeRentalSystem.Messaging.Configurations;
using BikeRentalSystem.Messaging.Consumers;
using BikeRentalSystem.Messaging.Interfaces;
using BikeRentalSystem.Messaging.Services;
using BikeRentalSystem.RentalServices.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalSystem.Api.Configuration;

public static class DependencyInjectionConfig
{
    public static void AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<INotifier, Notifier>();
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
        services.AddScoped<ICourierRepository, CourierRepository>();
        services.AddScoped<IRentalRepository, RentalRepository>();

        services.AddScoped<IMotorcycleService, MotorcycleService>();
        services.AddScoped<ICourierService, CourierService>();
        services.AddScoped<IRentalService, RentalService>();

        services.AddScoped<IValidator<Motorcycle>, MotorcycleValidation>();
        services.AddScoped<IValidator<Courier>, CourierValidation>();
        services.AddScoped<IValidator<Rental>, RentalValidation>();

        services.AddScoped<IBlobStorageService, BlobStorageService>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUser, AspNetUser>();
        services.AddScoped<RoleManager<IdentityRole>>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddAuthorization();

        services.AddRabbitMQ(configuration);
        services.AddSingleton<IMessageProducer, RabbitMQProducer>();
        services.AddSingleton<IMessageConsumer, MotorcycleRegisteredConsumer>();
        services.AddSingleton<IMessageConsumer, CourierRegisteredConsumer>();
        services.AddSingleton<IMessageConsumer, RentalRegisteredConsumer>();
    }
}
