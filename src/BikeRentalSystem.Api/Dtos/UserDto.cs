using System.ComponentModel.DataAnnotations;

namespace BikeRentalSystem.Api.Dtos;

public class RegisterUserDto
{
    [Required(ErrorMessage = "The {0} field is required.")]
    [EmailAddress(ErrorMessage = "The {0} field is in an invalid format.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "The {0} field is required.")]
    [StringLength(100, ErrorMessage = "The {0} field must be between {2} and {1} characters.", MinimumLength = 6)]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "The passwords do not match.")]
    public string ConfirmPassword { get; set; }
}

public class LoginUserDto
{
    [Required(ErrorMessage = "The {0} field is required.")]
    [EmailAddress(ErrorMessage = "The {0} field is in an invalid format.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "The {0} field is required.")]
    [StringLength(100, ErrorMessage = "The {0} field must be between {2} and {1} characters.", MinimumLength = 6)]
    public string Password { get; set; }
}

public class UserTokenDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public IEnumerable<ClaimDto> Claims { get; set; }
}

public class LoginResponseDto
{
    public string AccessToken { get; set; }
    public double ExpiresIn { get; set; }
    public UserTokenDto UserToken { get; set; }
}

public class ClaimDto
{
    public string Type { get; set; }
    public string Value { get; set; }
}