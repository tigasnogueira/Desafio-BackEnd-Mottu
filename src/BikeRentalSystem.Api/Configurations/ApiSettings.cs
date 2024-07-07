using Asp.Versioning.ApiExplorer;
using BikeRentalSystem.Messaging.Configurations;

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

        var databaseSettings = configuration.GetSection("DatabaseSettings");
        services.Configure<DatabaseSettings>(databaseSettings);

        var rabbitMqSettings = configuration.GetSection("RabbitMqSettings");
        services.Configure<RabbitMQSettings>(rabbitMqSettings);
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
