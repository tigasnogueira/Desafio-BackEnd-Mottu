using BikeRentalSystem.Api.Models.Response;
using BikeRentalSystem.Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

namespace BikeRentalSystem.Api.Configuration;

public class ApiSettings
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetSection("DatabaseSettings:DefaultConnection").Value));

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.ConfigureAutomapper();
        services.AddDependencyInjection(configuration);
        services.AddApiVersioningConfiguration();
        services.AddSwaggerConfiguration();
        services.AddHealthCheckConfiguration();

        var keycloakConfig = configuration.GetSection("Keycloak").Get<KeycloakConfigResponse>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = keycloakConfig!.Authority;
                options.Audience = keycloakConfig.Resource;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                };
                options.EventsType = typeof(CustomJwtBearerEvents);
            });

        services.AddAuthorization();
    }

    public void ConfigurePipeline(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwaggerConfiguration();

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHealthCheckConfiguration();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}
