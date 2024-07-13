using BikeRentalSystem.Identity.ViewModels;
using System.Security.Claims;

namespace BikeRentalSystem.Identity.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterUserViewModel registerUser);
    Task<LoginResponseViewModel> LoginAsync(LoginUserViewModel loginUser);
    Task<bool> AddRoleAsync(string roleName);
    Task<bool> AddClaimAsync(ClaimViewModel claim);
    Task<bool> AssignRolesAndClaimsAsync(string userId, IEnumerable<string> roles, IEnumerable<ClaimViewModel> claims);
    Task<LoginResponseViewModel> GenerateJwtAsync(string email);
}
