using BikeRentalSystem.Core.Interfaces.Repositories;
using FluentValidation;

namespace BikeRentalSystem.Core.Models.Validations;

public class CourierValidation : AbstractValidator<Courier>
{
    public CourierValidation(ICourierRepository courierRepository)
    {
        RuleFor(c => c.CNPJ)
            .NotEmpty().WithMessage("CNPJ is required.")
        .MustAsync(async (cnpj, cancellation) =>
                await courierRepository.IsCNPJUniqueAsync(cnpj))
            .WithMessage("CNPJ must be unique.");

        RuleFor(c => c.DriverLicenseNumber)
            .NotEmpty().WithMessage("Driver license number is required.")
            .MustAsync(async (licenseNumber, cancellation) =>
                await courierRepository.IsDriverLicenseNumberUniqueAsync(licenseNumber))
            .WithMessage("Driver license number must be unique.");

        RuleFor(c => c.DriverLicenseType)
            .NotEmpty().WithMessage("Driver license type is required.")
            .Must(licenseType => licenseType == "A" || licenseType == "B" || licenseType == "A+B")
            .WithMessage("Driver license type must be A, B, or A+B.");
    }
}
