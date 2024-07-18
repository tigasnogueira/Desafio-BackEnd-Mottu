using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BikeRentalSystem.Api.Configuration;

public static class HealthCheckConfig
{
    public static IServiceCollection AddHealthCheckConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        AddHealthChecks(services, configuration);
        AddHealthChecksUI(services);
        return services;
    }

    private static void AddHealthChecks(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddCheck<CustomHealthCheck>("custom_health_check")
            .AddNpgSql(
                connectionString: configuration.GetConnectionString("DefaultConnection"),
                name: "postgresql",
                healthQuery: "SELECT 1;",
                failureStatus: HealthStatus.Degraded,
                tags: new[] { "db", "sql", "postgresql" });
    }

    private static void AddHealthChecksUI(IServiceCollection services)
    {
        services.AddHealthChecksUI()
            .AddInMemoryStorage();
    }

    public static void UseHealthCheckConfiguration(this IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints =>
        {
            MapHealthChecks(endpoints);
            MapHealthChecksUI(endpoints);
        });
    }

    private static void MapHealthChecks(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }

    private static void MapHealthChecksUI(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecksUI(options =>
        {
            options.UIPath = "/health-ui";
            options.ApiPath = "/health-ui-api";
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
