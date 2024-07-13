using BikeRentalSystem.Identity.Extensions;
using BikeRentalSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BikeRentalSystem.Api.Configuration;

public static class IdentityConfig
{
    public static IServiceCollection AddIdentityConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetSection("DatabaseSettings:DefaultConnection").Value));

        services.AddDefaultIdentity<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddErrorDescriber<IdentityMessagesEnglish>()
            .AddDefaultTokenProviders();

        var appSettingsSection = configuration.GetSection("AppSettings");
        var appSettings = appSettingsSection.Get<AppSettings>();
        appSettings.Secret = Environment.GetEnvironmentVariable("APP_SECRET");
        var key = Encoding.ASCII.GetBytes(appSettings.Secret);

        if (key.Length < 16)
        {
            throw new ArgumentException("The secret key must be at least 16 characters long.");
        }

        services.Configure<AppSettings>(options =>
        {
            options.Secret = appSettings.Secret;
            options.ExpirationHours = appSettings.ExpirationHours;
            options.Issuer = appSettings.Issuer;
            options.ValidAt = appSettings.ValidAt;
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
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
