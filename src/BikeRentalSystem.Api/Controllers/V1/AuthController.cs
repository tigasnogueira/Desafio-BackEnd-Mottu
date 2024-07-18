using Asp.Versioning;
using BikeRentalSystem.Core.Interfaces;
using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Identity.Interfaces;
using BikeRentalSystem.Identity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeRentalSystem.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : MainController
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(INotifier notifier,
                          IAuthService authenticationService,
                          IAspNetUser user,
                          ILogger<AuthController> logger) : base(notifier, user)
    {
        _authService = authenticationService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterUserViewModel registerUser)
    {
        return await HandleAuthOperationAsync(
            async () =>
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);

                var result = await _authService.RegisterAsync(registerUser);
                if (result)
                {
                    _logger.LogInformation($"User {registerUser.Email} registered successfully");
                    return CustomResponse(await _authService.GenerateJwtAsync(registerUser.Email));
                }

                NotifyError("Error registering the user.");
                return CustomResponse(registerUser);
            },
            ex =>
            {
                NotifyError("An error occurred while registering the user.");
                _logger.LogError(ex, "An error occurred while registering the user.");
                return CustomResponse(registerUser);
            }
        );
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginUserViewModel loginUser)
    {
        return await HandleAuthOperationAsync(
            async () =>
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);

                var result = await _authService.LoginAsync(loginUser);
                if (result != null)
                {
                    _logger.LogInformation($"User {loginUser.Email} logged in successfully");
                    return CustomResponse(result);
                }

                NotifyError("Incorrect username or password");
                return CustomResponse(loginUser);
            },
            ex =>
            {
                NotifyError("An error occurred while logging in.");
                _logger.LogError(ex, "An error occurred while logging in.");
                return CustomResponse(loginUser);
            }
        );
    }

    [HttpPost("add-role")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> AddRole([FromBody] string roleName)
    {
        return await HandleAuthOperationAsync(
            async () =>
            {
                if (string.IsNullOrWhiteSpace(roleName))
                {
                    NotifyError("Role name is required.");
                    return CustomResponse(roleName);
                }

                var result = await _authService.AddRoleAsync(roleName);
                if (result)
                {
                    _logger.LogInformation($"Role {roleName} added successfully");
                    return CustomResponse(result);
                }

                NotifyError("Error adding role.");
                return CustomResponse(roleName);
            },
            ex =>
            {
                NotifyError("An error occurred while adding the role.");
                _logger.LogError(ex, "An error occurred while adding the role.");
                return CustomResponse(roleName);
            }
        );
    }

    [HttpPost("assign-roles-claims")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> AssignRolesAndClaims(AssignRolesAndClaimsViewModel model)
    {
        return await HandleAuthOperationAsync(
            async () =>
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);

                var result = await _authService.AssignRolesAndClaimsAsync(model.UserId, model.Roles, model.Claims);

                if (!result)
                {
                    NotifyError("Error assigning roles and/or claims.");
                    return CustomResponse(model);
                }

                _logger.LogInformation($"Roles and/or claims assigned successfully to user {model.UserId}");
                return CustomResponse(result);
            },
            ex =>
            {
                NotifyError("An error occurred while assigning roles and claims.");
                _logger.LogError(ex, "An error occurred while assigning roles and claims.");
                return CustomResponse(model);
            }
        );
    }

    private async Task<ActionResult> HandleAuthOperationAsync(Func<Task<ActionResult>> operation, Func<Exception, ActionResult> handleException)
    {
        try
        {
            return await operation();
        }
        catch (Exception ex)
        {
            return handleException(ex);
        }
    }
}
