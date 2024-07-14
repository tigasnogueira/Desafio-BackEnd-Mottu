﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace BikeRentalSystem.Identity.Extensions;

public class CustomAuthorization
{
    public static bool ValidateUserClaims(HttpContext context, string claimName, string claimValue)
    {
        return context.User.Identity.IsAuthenticated &&
               context.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue));
    }
}

public class ClaimsAuthorizeAttribute : TypeFilterAttribute
{
    public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequireClaimFilter))
    {
        Arguments = new object[] { new Claim(claimName, claimValue) };
    }
}

public class RequireClaimFilter : IAuthorizationFilter
{
    private readonly Claim _claim;

    public RequireClaimFilter(Claim claim)
    {
        _claim = claim;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            return;
        }

        if (!CustomAuthorization.ValidateUserClaims(context.HttpContext, _claim.Type, _claim.Value))
        {
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
        }
    }
}
