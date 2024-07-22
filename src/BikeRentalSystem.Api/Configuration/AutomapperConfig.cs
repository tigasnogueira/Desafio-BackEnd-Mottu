using AutoMapper;
using System.Reflection;

namespace BikeRentalSystem.Api.Configuration;

public static class AutomapperConfig
{
    public static void ConfigureAutomapper(this IServiceCollection services)
    {
        var mappingProfiles = GetMappingProfiles();
        if (mappingProfiles.Any())
        {
            services.AddAutoMapper(mappingProfiles);
        }
    }

    private static Type[] GetMappingProfiles()
    {
        var assembly = Assembly.GetExecutingAssembly();
        return assembly.GetTypes()
            .Where(type => typeof(Profile).IsAssignableFrom(type) && !type.IsAbstract && type.Namespace == "BikeRentalSystem.Api.Mappers")
            .ToArray();
    }
}
