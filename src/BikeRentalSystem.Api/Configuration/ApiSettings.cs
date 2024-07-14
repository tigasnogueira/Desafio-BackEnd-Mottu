using Asp.Versioning.ApiExplorer;
using BikeRentalSystem.Infrastructure.Context;
using BikeRentalSystem.Shared.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace BikeRentalSystem.Api.Configuration;

public class ApiSettings
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetSection("DatabaseSettings:DefaultConnection").Value));

        var azureConnectionString = Environment.GetEnvironmentVariable("AZURE_CONNECTION_STRING");

        services.Configure<AzureBlobStorageSettings>(options =>
        {
            options.ConnectionString = azureConnectionString;
            options.ContainerName = configuration["AzureBlobStorageSettings:ContainerName"];
        });

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.ConfigureAutomapper();
        services.AddDependencyInjection(configuration);
        services.AddApiVersioningConfiguration();
        services.AddSwaggerConfig();
        services.AddHealthCheckConfiguration();
        services.AddIdentityConfig(configuration);

        services.AddAuthorization();
        services.AddAuthentication();
    }

    public void ConfigurePipeline(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwaggerConfig(provider);

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHealthCheckConfiguration();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}
