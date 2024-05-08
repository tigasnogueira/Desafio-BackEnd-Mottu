using FluentValidation;

namespace BikeRentalSystem.Core.Models.Validations;

public class RentalValidation : AbstractValidator<Rental>
{
    public RentalValidation()
    {
        RuleFor(r => r.StartDate)
            .LessThanOrEqualTo(r => r.EndDate)
            .WithMessage("Start date must be before end date.");

        RuleFor(r => r.EndDate)
            .GreaterThan(r => r.StartDate)
            .WithMessage("End date must be after start date.");

        RuleFor(r => r.Price)
            .GreaterThan(0)
            .WithMessage("Price must be positive.");
    }
}
