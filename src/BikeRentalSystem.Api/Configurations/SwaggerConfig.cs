using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace BikeRentalSystem.Api.Configurations;

public static class SwaggerConfig
{
    public static void AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "API v1", Version = "v1" });
        });
    }

    public static IApplicationBuilder UseSwaggerUIConfiguration(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"Bike Rental System {description.ApiVersion}");
            }

            options.RoutePrefix = "swagger";
        });

        return app;
    }
}
