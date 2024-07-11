using Microsoft.AspNetCore.Identity;

namespace BikeRentalSystem.Api.Data;

public static class ApplicationDbInitializer
{
    public static async Task SeedRolesAndClaims(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        string[] roleNames = { "Courier", "Motorcycle", "Rental" };
        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        var user = new IdentityUser
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            EmailConfirmed = true,
        };

        string userPassword = "P@ssw0rd!";
        var userExist = await userManager.FindByEmailAsync("admin@example.com");

        if (userExist == null)
        {
            var createUser = await userManager.CreateAsync(user, userPassword);
            if (createUser.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Courier", "Get"));
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Courier", "Add"));
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Courier", "Update"));
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Courier", "Delete"));

                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Motorcycle", "Get"));
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Motorcycle", "Add"));
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Motorcycle", "Update"));
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Motorcycle", "Delete"));

                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Rental", "Get"));
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Rental", "Add"));
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Rental", "Update"));
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Rental", "Delete"));
            }
        }
    }
}
