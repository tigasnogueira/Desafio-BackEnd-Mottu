using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Models.Validations;
using BikeRentalSystem.Core.Notifications;
using BikeRentalSystem.Infrastructure.Context;
using BikeRentalSystem.Infrastructure.Repositories;
using BikeRentalSystem.RentalServices.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalSystem.Api.Configuration;

public static class DependencyInjectionConfig
{
    public static void AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<INotifier, Notifier>();
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
        services.AddScoped<ICourierRepository, CourierRepository>();
        services.AddScoped<IRentalRepository, RentalRepository>();

        services.AddScoped<IMotorcycleService, MotorcycleService>();
        services.AddScoped<ICourierService, CourierService>();
        services.AddScoped<IRentalService, RentalService>();

        services.AddScoped<IValidator<Motorcycle>, MotorcycleValidation>();
        services.AddScoped<IValidator<Courier>, CourierValidation>();
        services.AddScoped<IValidator<Rental>, RentalValidation>();

        services.AddScoped<CustomJwtBearerEvents>();
    }
}
