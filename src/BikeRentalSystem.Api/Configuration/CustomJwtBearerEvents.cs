using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace BikeRentalSystem.Api.Configuration;

public class CustomJwtBearerEvents : JwtBearerEvents
{
    private readonly ILogger<CustomJwtBearerEvents> _logger;

    public CustomJwtBearerEvents(ILogger<CustomJwtBearerEvents> logger)
    {
        _logger = logger;
    }

    public override async Task TokenValidated(TokenValidatedContext context)
    {
        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;

        if (claimsIdentity != null)
        {
            var realmAccessClaim = context.Principal.Claims.FirstOrDefault(c => c.Type == "realm_access");

            if (realmAccessClaim != null)
            {
                var realmAccessAsJson = JObject.Parse(realmAccessClaim.Value);
                var roles = realmAccessAsJson["roles"].ToObject<List<string>>();

                foreach (var role in roles)
                {
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
                }
            }
        }

        _logger.LogInformation("Token validated successfully for user: {user}", context.Principal.Identity.Name);
        await base.TokenValidated(context);
    }

    public override Task AuthenticationFailed(AuthenticationFailedContext context)
    {
        _logger.LogError("Authentication failed: {error}", context.Exception.Message);
        return base.AuthenticationFailed(context);
    }

    public override Task Challenge(JwtBearerChallengeContext context)
    {
        _logger.LogWarning("Token challenge triggered: {error}", context.AuthenticateFailure?.Message);
        return base.Challenge(context);
    }

    public override Task Forbidden(ForbiddenContext context)
    {
        _logger.LogWarning("Access forbidden: {user}", context.HttpContext.User.Identity.Name);
        return base.Forbidden(context);
    }
}
