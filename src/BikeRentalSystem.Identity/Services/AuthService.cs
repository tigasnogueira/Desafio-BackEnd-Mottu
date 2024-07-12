using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Identity.Extensions;
using BikeRentalSystem.Identity.Interfaces;
using BikeRentalSystem.Identity.ViewModels;
using BikeRentalSystem.RentalServices.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BikeRentalSystem.Identity.Services;

public class AuthService : BaseService, IAuthService
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly AppSettings _appSettings;
    private readonly ILogger _logger;

    public AuthService(SignInManager<IdentityUser> signInManager,
                                 UserManager<IdentityUser> userManager,
                                 RoleManager<IdentityRole> roleManager,
                                 IOptions<AppSettings> appSettings,
                                 ILogger<AuthService> logger,
                                 INotifier notifier) : base(notifier)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _appSettings = appSettings.Value;
        _logger = logger;
    }

    public async Task<bool> RegisterAsync(RegisterUserViewModel registerUser)
    {
        try
        {
            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return true;
            }
            foreach (var error in result.Errors)
            {
                HandleException(new Exception(error.Description));
            }
            return false;
        }
        catch (Exception ex)
        {
            HandleException(ex);
            _logger.LogError(ex, "An error occurred while registering the user.");
            return false;
        }
    }

    public async Task<LoginResponseViewModel> LoginAsync(LoginUserViewModel loginUser)
    {
        try
        {
            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);
            if (result.Succeeded)
            {
                _logger.LogInformation($"User {loginUser.Email} logged in successfully");
                return await GenerateJwtAsync(loginUser.Email);
            }
            if (result.IsLockedOut)
            {
                HandleException(new Exception("User temporarily locked out due to multiple failed login attempts."));
            }
            else
            {
                HandleException(new Exception("Invalid user or password."));
            }
            return null;
        }
        catch (Exception ex)
        {
            HandleException(ex);
            _logger.LogError(ex, "An error occurred while logging in the user.");
            return null;
        }
    }

    public async Task AssignRolesAndClaimsAsync(string userId, IEnumerable<string> roles, IEnumerable<Claim> claims)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                foreach (var claim in claims)
                {
                    await _userManager.AddClaimAsync(user, claim);
                }
                foreach (var role in roles)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }
                    await _userManager.AddToRoleAsync(user, role);
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
            _logger.LogError(ex, "An error occurred while assigning roles and claims to the user.");
        }
    }

    public async Task<LoginResponseViewModel> GenerateJwtAsync(string email)
    {
        try
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

            return new LoginResponseViewModel
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
        }
        catch (Exception ex)
        {
            HandleException(ex);
            _logger.LogError(ex, "An error occurred while generating the JWT token.");
            return null;
        }
    }

    private static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
}
