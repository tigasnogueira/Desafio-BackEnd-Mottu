using BikeRentalSystem.Api.Data;
using BikeRentalSystem.Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BikeRentalSystem.Api.Configurations;

public static class IdentityServerConfig
{
    public static IServiceCollection AddIdentityConfig(this IServiceCollection services,
            IConfiguration configuration)
    {
        services.AddDbContext<AuthDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddDefaultIdentity<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddErrorDescriber<IdentityMessagesEnglish>()
            .AddDefaultTokenProviders();

        // JWT

        var appSettingsSection = configuration.GetSection("AppSettings");
        services.Configure<AppSettings>(appSettingsSection);

        var appSettings = appSettingsSection.Get<AppSettings>();
        var key = Encoding.ASCII.GetBytes(appSettings.Secret);

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = true;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = appSettings.ValidAt,
                ValidIssuer = appSettings.Issuer
            };
        });

        return services;
    }
}