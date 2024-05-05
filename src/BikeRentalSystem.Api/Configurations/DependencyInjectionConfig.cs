using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Infrastructure.Repositories;

namespace BikeRentalSystem.Api.Configurations;

public static class DependencyInjectionConfig
{
    public static void AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRepository<Courier>, CourierRepository>();
        services.AddScoped<IRepository<Motorcycle>, MotorcycleRepository>();
    }
}
