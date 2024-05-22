using Asp.Versioning.ApiExplorer;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BikeRentalSystem.Api.Configurations;

public static class HealthCheckConfig
{
    public static IServiceCollection AddHealthChecksConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks().AddCheck<CustomHealthCheck>("custom_health_check");

        services.AddHealthChecksUI().AddInMemoryStorage();

        return services;
    }

    public static void UseHealthChecksConfiguration(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            endpoints.MapHealthChecksUI(options =>
            {
                options.UIPath = "/health-ui";
                options.ApiPath = "/health-ui-api";
            });
        });
    }
}

public class CustomHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var healthCheckResultHealthy = true;

        if (healthCheckResultHealthy)
        {
            return Task.FromResult(HealthCheckResult.Healthy("The check indicates a healthy result."));
        }

        return Task.FromResult(HealthCheckResult.Unhealthy("The check indicates an unhealthy result."));
    }
}
