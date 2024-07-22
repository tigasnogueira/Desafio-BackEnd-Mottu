using BikeRentalSystem.Api.Controllers.V1;
using BikeRentalSystem.Identity.Interfaces;
using BikeRentalSystem.Identity.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace BikeRentalSystem.Api.Tests.Controllers.V1;

public class AuthControllerTests : BaseControllerTests<AuthController>
{
    private readonly IAuthService _authServiceMock;
    private readonly ILogger<AuthController> _loggerMock;

    public AuthControllerTests()
    {
        _authServiceMock = Substitute.For<IAuthService>();
        _loggerMock = Substitute.For<ILogger<AuthController>>();

        controller = new AuthController(_notifierMock, _authServiceMock, _userMock, _loggerMock)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext
            }
        };
    }

    [Fact]
    public async Task Register_ShouldReturnSuccess_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var registerUser = new RegisterUserViewModel { Email = "test@example.com", Password = "Password123!" };
        _authServiceMock.RegisterAsync(registerUser).Returns(Task.FromResult(true));
        var loginResponse = new LoginResponseViewModel
        {
            AccessToken = "fake-jwt-token",
            ExpiresIn = 0.0,
            UserToken = new UserTokenViewModel
            {
                Id = "",
                Email = "",
                Claims = new List<ClaimViewModel>()
            }
        };
        _authServiceMock.GenerateJwtAsync(registerUser.Email).Returns(Task.FromResult(loginResponse));

        // Act
        var result = await controller.Register(registerUser);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;

        var expectedResponse = new
        {
            success = true,
            data = loginResponse
        };

        okResult.Value.Should().BeEquivalentTo(expectedResponse);
        _loggerMock.Received(1).LogInformation($"User {registerUser.Email} registered successfully");
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        var registerUser = new RegisterUserViewModel();
        controller.ModelState.AddModelError("Email", "Email is required");

        // Act
        var result = await controller.Register(registerUser);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;

        var expectedResponse = new
        {
            success = false,
            errors = new List<string> { "Email is required" }
        };

        badRequestResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequest_WhenRegistrationFails()
    {
        // Arrange
        var registerUser = new RegisterUserViewModel { Email = "test@example.com", Password = "Password123!" };
        _authServiceMock.RegisterAsync(registerUser).Returns(Task.FromResult(false));

        // Act
        var result = await controller.Register(registerUser);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;

        var expectedResponse = new
        {
            success = false,
            errors = "Error registering the user"
        };

        badRequestResult.Value.Should().BeEquivalentTo(expectedResponse);
        _loggerMock.Received(1).LogError("Error registering the user.");
    }

    [Fact]
    public async Task Login_ShouldReturnSuccess_WhenLoginIsSuccessful()
    {
        // Arrange
        var loginUser = new LoginUserViewModel { Email = "test@example.com", Password = "Password123!" };
        var fakeJwtToken = new LoginResponseViewModel
        {
            AccessToken = "fake-jwt-token",
            ExpiresIn = 3600,
            UserToken = new UserTokenViewModel
            {
                Id = "user-id",
                Email = "test@example.com",
                Claims = new List<ClaimViewModel>()
            }
        };
        _authServiceMock.LoginAsync(loginUser).Returns(Task.FromResult(fakeJwtToken));

        // Act
        var result = await controller.Login(loginUser);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;

        var expectedResponse = new
        {
            success = true,
            data = fakeJwtToken
        };

        okResult.Value.Should().BeEquivalentTo(expectedResponse);
        _loggerMock.Received(1).LogInformation($"User {loginUser.Email} logged in successfully");
    }

    [Fact]
    public async Task Login_ShouldReturnBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        var loginUser = new LoginUserViewModel();
        controller.ModelState.AddModelError("Email", "Email is required");

        // Act
        var result = await controller.Login(loginUser);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;

        var expectedResponse = new
        {
            success = false,
            errors = new List<string> { "Email is required" }
        };

        badRequestResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task Login_ShouldReturnBadRequest_WhenLoginFails()
    {
        // Arrange
        var loginUser = new LoginUserViewModel { Email = "test@example.com", Password = "Password123!" };
        _authServiceMock.LoginAsync(loginUser).Returns(Task.FromResult<LoginResponseViewModel>(null));

        // Act
        var result = await controller.Login(loginUser);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;

        var expectedResponse = new
        {
            success = false,
            errors = "Incorrect username or password"
        };

        badRequestResult.Value.Should().BeEquivalentTo(expectedResponse);
        _loggerMock.Received(1).LogError("Incorrect username or password");
    }

    [Fact]
    public async Task AddRole_ShouldReturnSuccess_WhenRoleIsAddedSuccessfully()
    {
        // Arrange
        var roleName = "Admin";
        _authServiceMock.AddRoleAsync(roleName).Returns(Task.FromResult(true));

        // Act
        var result = await controller.AddRole(roleName);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;

        var expectedResponse = new
        {
            data = true,
            success = true
        };

        okResult.Value.Should().BeEquivalentTo(expectedResponse);
        _loggerMock.Received(1).LogInformation($"Role {roleName} added successfully");
    }

    [Fact]
    public async Task AddRole_ShouldReturnBadRequest_WhenRoleNameIsEmpty()
    {
        // Arrange
        var roleName = string.Empty;

        // Act
        var result = await controller.AddRole(roleName);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;

        var expectedResponse = new
        {
            errors = roleName,
            success = false
        };

        badRequestResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task AddRole_ShouldReturnBadRequest_WhenAddingRoleFails()
    {
        // Arrange
        var roleName = "Admin";
        _authServiceMock.AddRoleAsync(roleName).Returns(Task.FromResult(false));

        // Act
        var result = await controller.AddRole(roleName);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;

        var expectedResponse = new
        {
            errors = roleName,
            success = false
        };

        badRequestResult.Value.Should().BeEquivalentTo(expectedResponse);
        _loggerMock.Received(1).LogError("An error occurred while adding the role.");
    }

    [Fact]
    public async Task AssignRolesAndClaims_ShouldReturnSuccess_WhenAssignmentIsSuccessful()
    {
        // Arrange
        var model = new AssignRolesAndClaimsViewModel
        {
            UserId = "1",
            Roles = new List<string> { "Admin" },
            Claims = new List<ClaimViewModel> { new ClaimViewModel { Type = "ClaimType", Value = "ClaimValue" } }
        };
        _authServiceMock.AssignRolesAndClaimsAsync(model.UserId, model.Roles, model.Claims).Returns(Task.FromResult(true));

        // Act
        var result = await controller.AssignRolesAndClaims(model);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;

        var expectedResponse = new
        {
            success = true,
            data = true
        };

        okResult.Value.Should().BeEquivalentTo(expectedResponse);
        _loggerMock.Received(1).LogInformation($"Roles and/or claims assigned successfully to user {model.UserId}");
    }

    [Fact]
    public async Task AssignRolesAndClaims_ShouldReturnBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        var model = new AssignRolesAndClaimsViewModel();
        controller.ModelState.AddModelError("UserId", "UserId is required");

        // Act
        var result = await controller.AssignRolesAndClaims(model);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        var expectedResponse = new
        {
            success = false,
            errors = new List<string> { "UserId is required" }
        };
        badRequestResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task AssignRolesAndClaims_ShouldReturnBadRequest_WhenAssignmentFails()
    {
        // Arrange
        var model = new AssignRolesAndClaimsViewModel
        {
            UserId = "1",
            Roles = new List<string> { "Admin" },
            Claims = new List<ClaimViewModel> { new ClaimViewModel { Type = "ClaimType", Value = "ClaimValue" } }
        };
        _authServiceMock.AssignRolesAndClaimsAsync(model.UserId, model.Roles, model.Claims).Returns(Task.FromResult(false));

        // Act
        var result = await controller.AssignRolesAndClaims(model);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;

        var expectedResponse = new
        {
            success = false,
            errors = "Error assigning roles and/or claims."
        };

        badRequestResult.Value.Should().BeEquivalentTo(expectedResponse);
        _loggerMock.Received(1).LogError("Error assigning roles and/or claims.");
    }
}
