using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace BikeRentalSystem.Core.Tests.Helpers;

public static class UserManagerMock
{
    public static UserManager<IdentityUser> Create()
    {
        var store = Substitute.For<IUserStore<IdentityUser>>();

        var options = Substitute.For<IOptions<IdentityOptions>>();
        options.Value.Returns(new IdentityOptions());

        var passwordHasher = Substitute.For<IPasswordHasher<IdentityUser>>();
        var userValidators = new List<IUserValidator<IdentityUser>>();
        var passwordValidators = new List<IPasswordValidator<IdentityUser>>();
        var keyNormalizer = Substitute.For<ILookupNormalizer>();
        var errors = Substitute.For<IdentityErrorDescriber>();
        var services = Substitute.For<IServiceProvider>();
        var logger = Substitute.For<ILogger<UserManager<IdentityUser>>>();

        return new UserManager<IdentityUser>(
            store, options, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger);
    }
}
