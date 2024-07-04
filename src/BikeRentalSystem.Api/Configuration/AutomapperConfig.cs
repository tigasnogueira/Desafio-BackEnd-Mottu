using AutoMapper;
using System.Reflection;

namespace BikeRentalSystem.Api.Configuration;

public static class AutomapperConfig
{
    public static void ConfigureAutomapper(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var allMappingProfiles = assembly.GetTypes()
            .Where(type => typeof(Profile).IsAssignableFrom(type) && !type.IsAbstract && type.Namespace == "BikeRentalSystem.Api.Mappers")
            .ToArray();

        if (allMappingProfiles.Any())
        {
            services.AddAutoMapper(allMappingProfiles);
        }
    }
}
