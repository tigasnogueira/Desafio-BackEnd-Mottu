using BikeRentalSystem.Core.Interfaces.Repositories;
using FluentValidation;

namespace BikeRentalSystem.Core.Models.Validations;

public class MotorcycleValidation : AbstractValidator<Motorcycle>
{
    public MotorcycleValidation(IMotorcycleRepository motorcycleRepository)
    {
        RuleFor(m => m.LicensePlate)
            .NotEmpty().WithMessage("License plate is required.")
        .MustAsync(async (licensePlate, cancellation) =>
                await motorcycleRepository.IsLicensePlateUniqueAsync(licensePlate))
            .WithMessage("License plate must be unique.");

        RuleFor(m => m.Model)
            .NotEmpty().WithMessage("Model is required.");

        RuleFor(m => m.Year)
            .InclusiveBetween(1900, DateTime.Now.Year)
            .WithMessage("Year must be valid.");
    }
}
