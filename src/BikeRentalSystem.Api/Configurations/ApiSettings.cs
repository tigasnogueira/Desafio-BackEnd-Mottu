using Asp.Versioning.ApiExplorer;

namespace BikeRentalSystem.Api.Configurations;

public class ApiSettings
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddApiVersioningConfiguration();
        services.AddDependencyInjection(configuration);
        services.AddSwaggerConfiguration();
        services.AddHealthChecksConfiguration(configuration);
        services.AddAutoMapper(typeof(AutomapperConfig));

        services.AddIdentityServer()
            .AddInMemoryClients(Config.GetClients())
            .AddInMemoryApiResources(Config.GetApiResources())
            .AddInMemoryApiScopes(Config.GetApiScopes())
            .AddInMemoryIdentityResources(Config.GetIdentityResources())
            .AddInMemoryPersistedGrants()
            .AddDeveloperSigningCredential();
    }

    public void ConfigurePipeline(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
            app.UseSwaggerUIConfiguration(provider);
        }

        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseCors();
        app.UseAuthorization();
        app.UseIdentityServer();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
