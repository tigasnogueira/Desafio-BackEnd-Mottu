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
