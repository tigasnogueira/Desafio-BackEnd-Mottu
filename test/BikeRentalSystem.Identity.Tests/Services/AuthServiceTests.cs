using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Notifications;
using BikeRentalSystem.Identity.Extensions;
using BikeRentalSystem.Identity.Services;
using BikeRentalSystem.Identity.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using NSubstitute;
using System.Security.Claims;

namespace BikeRentalSystem.Identity.Tests.Services;

public class AuthServiceTests
{
    private readonly AuthService _authService;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IOptions<AppSettings> _appSettings;
    private readonly ILogger<AuthService> _logger;
    private readonly INotifier _notifier;

    public AuthServiceTests()
    {
        var userStore = Substitute.For<IUserStore<IdentityUser>>();
        _userManager = Substitute.For<UserManager<IdentityUser>>(
            userStore, null, null, null, null, null, null, null, null);

        var contextAccessor = Substitute.For<IHttpContextAccessor>();
        var userPrincipalFactory = Substitute.For<IUserClaimsPrincipalFactory<IdentityUser>>();
        _signInManager = Substitute.For<SignInManager<IdentityUser>>(
            _userManager, contextAccessor, userPrincipalFactory, null, null, null, null);

        var roleStore = Substitute.For<IRoleStore<IdentityRole>>();
        _roleManager = Substitute.For<RoleManager<IdentityRole>>(
            roleStore, null, null, null, null);

        _appSettings = Substitute.For<IOptions<AppSettings>>();
        _logger = Substitute.For<ILogger<AuthService>>();
        _notifier = Substitute.For<INotifier>();

        var appSettings = new AppSettings
        {
            Secret = "TestSecretWhichIsLongEnoughForHmac",
            Issuer = "TestIssuer",
            ValidAt = "TestValidAt",
            ExpirationHours = 1
        };
        _appSettings.Value.Returns(appSettings);

        _authService = new AuthService(_signInManager, _userManager, _roleManager, _appSettings, _logger, _notifier);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnTrue_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var registerUser = new RegisterUserViewModel { Email = "test@example.com", Password = "Test@123", ConfirmPassword = "Test@123" };
        var identityUser = new IdentityUser { UserName = registerUser.Email, Email = registerUser.Email, EmailConfirmed = true };
        _userManager.CreateAsync(Arg.Any<IdentityUser>(), Arg.Any<string>()).Returns(IdentityResult.Success);
        _signInManager.SignInAsync(identityUser, false).Returns(Task.CompletedTask);

        // Act
        var result = await _authService.RegisterAsync(registerUser);

        // Assert
        result.Should().BeTrue();
        await _userManager.Received(1).CreateAsync(Arg.Any<IdentityUser>(), Arg.Any<string>());
        await _signInManager.Received(1).SignInAsync(Arg.Any<IdentityUser>(), false);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnFalse_WhenRegistrationFails()
    {
        // Arrange
        var registerUser = new RegisterUserViewModel { Email = "test@example.com", Password = "Test@123", ConfirmPassword = "Test@123" };
        var identityResult = IdentityResult.Failed(new IdentityError { Description = "User creation failed" });
        _userManager.CreateAsync(Arg.Any<IdentityUser>(), Arg.Any<string>()).Returns(identityResult);

        // Act
        var result = await _authService.RegisterAsync(registerUser);

        // Assert
        result.Should().BeFalse();
        await _userManager.Received(1).CreateAsync(Arg.Any<IdentityUser>(), Arg.Any<string>());
        _notifier.Received(1).Handle(Arg.Is<string>(s => s.Contains("User creation failed")), Arg.Any<NotificationType>());
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnLoginResponseViewModel_WhenLoginIsSuccessful()
    {
        // Arrange
        var loginUser = new LoginUserViewModel
        {
            Email = "test@example.com",
            Password = "Password123!"
        };

        _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true)
                      .Returns(SignInResult.Success);
        _userManager.FindByEmailAsync(loginUser.Email)
                    .Returns(Task.FromResult(new IdentityUser { Email = loginUser.Email }));
        _userManager.GetClaimsAsync(Arg.Any<IdentityUser>())
                    .Returns(Task.FromResult<IList<Claim>>(new List<Claim> { new Claim(JwtRegisteredClaimNames.Sub, "1") }));
        _userManager.GetRolesAsync(Arg.Any<IdentityUser>())
                    .Returns(Task.FromResult<IList<string>>(new List<string> { "Admin" }));

        // Act
        var result = await _authService.LoginAsync(loginUser);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeEmpty();
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnNull_WhenLoginFails()
    {
        // Arrange
        var loginUser = new LoginUserViewModel { Email = "test@example.com", Password = "Test@123" };
        var signInResult = SignInResult.Failed;
        _signInManager.PasswordSignInAsync(Arg.Any<string>(), Arg.Any<string>(), false, true).Returns(signInResult);

        // Act
        var result = await _authService.LoginAsync(loginUser);

        // Assert
        result.Should().BeNull();
        _notifier.Received(1).Handle(Arg.Is<string>(s => s.Contains("Invalid user or password.")), Arg.Any<NotificationType>());
    }

    [Fact]
    public async Task AddRoleAsync_ShouldReturnTrue_WhenRoleIsAddedSuccessfully()
    {
        // Arrange
        var roleName = "Admin";
        _roleManager.RoleExistsAsync(roleName).Returns(false);
        _roleManager.CreateAsync(Arg.Any<IdentityRole>()).Returns(IdentityResult.Success);

        // Act
        var result = await _authService.AddRoleAsync(roleName);

        // Assert
        result.Should().BeTrue();
        await _roleManager.Received(1).CreateAsync(Arg.Any<IdentityRole>());
    }

    [Fact]
    public async Task AddRoleAsync_ShouldReturnFalse_WhenRoleAlreadyExists()
    {
        // Arrange
        var roleName = "Admin";
        _roleManager.RoleExistsAsync(roleName).Returns(true);

        // Act
        var result = await _authService.AddRoleAsync(roleName);

        // Assert
        result.Should().BeFalse();
        _logger.Received(1).LogWarning($"Role {roleName} already exists.");
    }

    [Fact]
    public async Task AssignRolesAndClaimsAsync_ShouldReturnTrue_WhenRolesAndClaimsAreAssignedSuccessfully()
    {
        // Arrange
        var userId = "testUserId";
        var roles = new List<string> { "Admin" };
        var claims = new List<ClaimViewModel> { new ClaimViewModel { Type = "Permission", Value = "Read" } };
        var user = new IdentityUser { Id = userId };

        _userManager.FindByIdAsync(userId).Returns(user);
        _roleManager.RoleExistsAsync(Arg.Any<string>()).Returns(true);
        _userManager.IsInRoleAsync(Arg.Any<IdentityUser>(), Arg.Any<string>()).Returns(false);
        _userManager.AddToRoleAsync(Arg.Any<IdentityUser>(), Arg.Any<string>()).Returns(IdentityResult.Success);
        _userManager.GetClaimsAsync(user).Returns(new List<Claim>());
        _userManager.AddClaimAsync(Arg.Any<IdentityUser>(), Arg.Any<Claim>()).Returns(IdentityResult.Success);

        // Act
        var result = await _authService.AssignRolesAndClaimsAsync(userId, roles, claims);

        // Assert
        result.Should().BeTrue();
        await _userManager.Received(1).AddToRoleAsync(Arg.Any<IdentityUser>(), Arg.Any<string>());
        await _userManager.Received(1).AddClaimAsync(Arg.Any<IdentityUser>(), Arg.Any<Claim>());
    }

    [Fact]
    public async Task AssignRolesAndClaimsAsync_ShouldReturnFalse_WhenUserNotFound()
    {
        // Arrange
        var userId = "testUserId";
        var roles = new List<string> { "Admin" };
        var claims = new List<ClaimViewModel> { new ClaimViewModel { Type = "Permission", Value = "Read" } };

        _userManager.FindByIdAsync(userId).Returns((IdentityUser)null);

        // Act
        var result = await _authService.AssignRolesAndClaimsAsync(userId, roles, claims);

        // Assert
        result.Should().BeFalse();
        _logger.Received(1).LogWarning("User with ID testUserId not found.");
    }

    [Fact]
    public async Task GenerateJwtAsync_ShouldReturnLoginResponse_WhenUserExists()
    {
        // Arrange
        var email = "test@example.com";
        var user = new IdentityUser { Email = email, Id = "testUserId" };
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64)
        };

        // Configurar os mocks
        _userManager.FindByEmailAsync(email).Returns(user);
        _userManager.GetClaimsAsync(user).Returns(claims);
        _userManager.GetRolesAsync(user).Returns(new List<string>());

        // Act
        var result = await _authService.GenerateJwtAsync(email);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeNullOrEmpty();
        result.UserToken.Email.Should().Be(email);
    }

    [Fact]
    public async Task GenerateJwtAsync_ShouldReturnNull_WhenUserNotFound()
    {
        // Arrange
        var email = "test@example.com";
        _userManager.FindByEmailAsync(email).Returns((IdentityUser)null);

        // Act
        var result = await _authService.GenerateJwtAsync(email);

        // Assert
        result.Should().BeNull();
        _notifier.Received(1).Handle(Arg.Is<string>(s => s.Contains("User not found.")), Arg.Any<NotificationType>());
    }

    private static long ToUnixEpochDate(DateTime date)
    {
        return (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
