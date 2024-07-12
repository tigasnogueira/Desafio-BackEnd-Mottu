using Asp.Versioning;
using BikeRentalSystem.Api.Extensions;
using BikeRentalSystem.Api.Models.ViewModels;
using BikeRentalSystem.Core.Interfaces;
using BikeRentalSystem.Core.Interfaces.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BikeRentalSystem.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : MainController
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly AppSettings _appSettings;
    private readonly ILogger _logger;

    public AuthController(INotifier notifier,
                          SignInManager<IdentityUser> signInManager,
                          UserManager<IdentityUser> userManager,
                          RoleManager<IdentityRole> roleManager,
                          IOptions<AppSettings> appSettings,
                          IUser user, ILogger<AuthController> logger) : base(notifier, user)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _appSettings = appSettings.Value;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterUserViewModel registerUser)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var user = new IdentityUser
        {
            UserName = registerUser.Email,
            Email = registerUser.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, registerUser.Password);
        if (result.Succeeded)
        {
            var claims = new List<Claim>
            {
                new Claim("Courier", "Get"),
                new Claim("Courier", "Add"),
                new Claim("Courier", "Update"),
                new Claim("Courier", "Delete"),
                new Claim("Motorcycle", "Get"),
                new Claim("Motorcycle", "Add"),
                new Claim("Motorcycle", "Update"),
                new Claim("Motorcycle", "Delete"),
                new Claim("Rental", "Get"),
                new Claim("Rental", "Add"),
                new Claim("Rental", "Update"),
                new Claim("Rental", "Delete")
            };
            foreach (var claim in claims)
            {
                await _userManager.AddClaimAsync(user, claim);
            }

            var roles = new List<string> { "Courier", "Motorcycle", "Rental" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
                await _userManager.AddToRoleAsync(user, role);
            }


            await _signInManager.SignInAsync(user, false);
            return CustomResponse(await GenerateJwt(user.Email));
        }
        foreach (var error in result.Errors)
        {
            NotifyError(error.Description);
        }

        return CustomResponse(registerUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginUserViewModel loginUser)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

        if (result.Succeeded)
        {
            _logger.LogInformation($"User {loginUser.Email} logged in successfully");
            return CustomResponse(await GenerateJwt(loginUser.Email));
        }
        if (result.IsLockedOut)
        {
            NotifyError("User temporarily locked out due to invalid login attempts");
            return CustomResponse(loginUser);
        }

        NotifyError("Incorrect username or password");
        return CustomResponse(loginUser);
    }

    private async Task<LoginResponseViewModel> GenerateJwt(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var claims = await _userManager.GetClaimsAsync(user);
        var userRoles = await _userManager.GetRolesAsync(user);

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

        foreach (var userRole in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _appSettings.Issuer,
            Audience = _appSettings.ValidAt,
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHours),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var encodedToken = tokenHandler.WriteToken(token);

        var response = new LoginResponseViewModel
        {
            AccessToken = encodedToken,
            ExpiresIn = TimeSpan.FromHours(_appSettings.ExpirationHours).TotalSeconds,
            UserToken = new UserTokenViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Claims = claims.Select(c => new ClaimViewModel { Type = c.Type, Value = c.Value }).ToList()
            }
        };

        return response;
    }

    private static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
}
