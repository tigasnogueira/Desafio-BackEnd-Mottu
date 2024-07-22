using BikeRentalSystem.Core.Interfaces;
using NSubstitute;
using System.Security.Claims;

namespace BikeRentalSystem.Identity.Tests.Helpers;

public static class AspNetUserMock
{
    public static IAspNetUser Create()
    {
        var userMock = Substitute.For<IAspNetUser>();
        userMock.Name.Returns("Test User");
        userMock.GetUserId().Returns(Guid.NewGuid());
        userMock.GetUserEmail().Returns("testuser@example.com");
        userMock.IsAuthenticated().Returns(true);
        userMock.IsInRole(Arg.Any<string>()).Returns(true);
        userMock.GetClaimsIdentity().Returns(new List<Claim>
        {
            new Claim(ClaimTypes.Name, "Test User"),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, "testuser@example.com"),
        });

        return userMock;
    }
}
