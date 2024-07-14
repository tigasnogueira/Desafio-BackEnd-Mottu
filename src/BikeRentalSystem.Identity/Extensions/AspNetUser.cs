﻿using BikeRentalSystem.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BikeRentalSystem.Identity.Extensions;

public class AspNetUser : IUser
{
    private readonly IHttpContextAccessor _accessor;

    public AspNetUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public string Name => _accessor.HttpContext?.User?.Identity?.Name;

    public Guid GetUserId()
    {
        return IsAuthenticated() ? Guid.Parse(_accessor.HttpContext?.User?.GetUserId()) : Guid.Empty;
    }

    public string GetUserEmail()
    {
        return IsAuthenticated() ? _accessor.HttpContext?.User?.GetUserEmail() : string.Empty;
    }

    public bool IsAuthenticated()
    {
        return _accessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }

    public bool IsInRole(string role)
    {
        return _accessor.HttpContext?.User?.IsInRole(role) ?? false;
    }

    public IEnumerable<Claim> GetClaimsIdentity()
    {
        return _accessor.HttpContext?.User?.Claims;
    }
}

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
        return claim?.Value;
    }

    public static string GetUserEmail(this ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        var claim = principal.FindFirst(ClaimTypes.Email);
        return claim?.Value;
    }
}
