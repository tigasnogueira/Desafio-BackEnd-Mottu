using Asp.Versioning;
using BikeRentalSystem.Core.Interfaces;
using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Identity.Extensions;
using BikeRentalSystem.Identity.Interfaces;
using BikeRentalSystem.Identity.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BikeRentalSystem.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : MainController
{
    private readonly IAuthService _authenticationService;
    private readonly ILogger _logger;

    public AuthController(INotifier notifier,
                          IAuthService authenticationService,
                          IUser user,
                          ILogger<AuthController> logger) : base(notifier, user)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterUserViewModel registerUser)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var result = await _authenticationService.RegisterAsync(registerUser);
        if (result)
        {
            return CustomResponse(await _authenticationService.GenerateJwtAsync(registerUser.Email));
        }
        return CustomResponse(registerUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginUserViewModel loginUser)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var result = await _authenticationService.LoginAsync(loginUser);
        if (result != null)
        {
            _logger.LogInformation($"User {loginUser.Email} logged in successfully");
            return CustomResponse(result);
        }

        NotifyError("Incorrect username or password");
        return CustomResponse(loginUser);
    }

    [HttpPost("assign-roles-claims")]
    [ClaimsAuthorize("Admin", "Add")]
    public async Task<ActionResult> AssignRolesAndClaims(AssignRolesAndClaimsViewModel model)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        await _authenticationService.AssignRolesAndClaimsAsync(model.UserId, model.Roles, model.Claims);
        return CustomResponse();
    }
}
