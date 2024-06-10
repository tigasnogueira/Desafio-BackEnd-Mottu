using Microsoft.AspNetCore.Identity;

namespace BikeRentalSystem.Api.Extensions;

public class IdentityMessagesEnglish : IdentityErrorDescriber
{
    public override IdentityError DefaultError() => new IdentityError
    {
        Code = nameof(DefaultError),
        Description = "An unknown failure has occurred."
    };

    public override IdentityError ConcurrencyFailure() => new IdentityError
    {
        Code = nameof(ConcurrencyFailure),
        Description = "Optimistic concurrency failure, object has been modified."
    };

    public override IdentityError PasswordMismatch() => new IdentityError
    {
        Code = nameof(PasswordMismatch),
        Description = "Incorrect password."
    };

    public override IdentityError InvalidToken() => new IdentityError
    {
        Code = nameof(InvalidToken),
        Description = "Invalid token."
    };

    public override IdentityError LoginAlreadyAssociated() => new IdentityError
    {
        Code = nameof(LoginAlreadyAssociated),
        Description = "A user with this login already exists."
    };

    public override IdentityError InvalidUserName(string userName) => new IdentityError
    {
        Code = nameof(InvalidUserName),
        Description = $"User name '{userName}' is invalid, can only contain letters or digits."
    };

    public override IdentityError InvalidEmail(string email) => new IdentityError
    {
        Code = nameof(InvalidEmail),
        Description = $"Email '{email}' is invalid."
    };

    public override IdentityError DuplicateUserName(string userName) => new IdentityError
    {
        Code = nameof(DuplicateUserName),
        Description = $"User name '{userName}' is already taken."
    };

    public override IdentityError DuplicateEmail(string email) => new IdentityError
    {
        Code = nameof(DuplicateEmail),
        Description = $"Email '{email}' is already taken."
    };

    public override IdentityError InvalidRoleName(string role) => new IdentityError
    {
        Code = nameof(InvalidRoleName),
        Description = $"Role name '{role}' is invalid."
    };

    public override IdentityError DuplicateRoleName(string role) => new IdentityError
    {
        Code = nameof(DuplicateRoleName),
        Description = $"Role name '{role}' is already taken."
    };

    public override IdentityError UserAlreadyHasPassword() => new IdentityError
    {
        Code = nameof(UserAlreadyHasPassword),
        Description = "User already has a password set."
    };

    public override IdentityError UserLockoutNotEnabled() => new IdentityError
    {
        Code = nameof(UserLockoutNotEnabled),
        Description = "Lockout is not enabled"
    };
}
