using BikeRentalSystem.Identity.ViewModels;
using System.Security.Claims;

namespace BikeRentalSystem.Identity.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterUserViewModel registerUser);
    Task<LoginResponseViewModel> LoginAsync(LoginUserViewModel loginUser);
    Task AssignRolesAndClaimsAsync(string userId, IEnumerable<string> roles, IEnumerable<Claim> claims);
    Task<LoginResponseViewModel> GenerateJwtAsync(string email);
}
