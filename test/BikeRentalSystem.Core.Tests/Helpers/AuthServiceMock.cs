using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace BikeRentalSystem.Core.Tests.Helpers;

public static class AuthServiceMock
{
    public static async Task<IdentityUser> RegisterMockUser(UserManager<IdentityUser> userManager)
    {
        var mockUser = new IdentityUser
        {
            UserName = "testuser@example.com",
            Email = "testuser@example.com",
            EmailConfirmed = true
        };

        userManager.CreateAsync(mockUser, "Test@123").Returns(Task.FromResult(IdentityResult.Success));
        userManager.FindByEmailAsync(mockUser.Email).Returns(Task.FromResult(mockUser));
        userManager.FindByNameAsync(mockUser.UserName).Returns(Task.FromResult(mockUser));

        await userManager.CreateAsync(mockUser, "Test@123");
        return mockUser;
    }
}
