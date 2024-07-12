using System.Security.Claims;

namespace BikeRentalSystem.Identity.ViewModels;

public class AssignRolesAndClaimsViewModel
{
    public string UserId { get; set; }
    public List<string> Roles { get; set; }
    public List<Claim> Claims { get; set; }
}
