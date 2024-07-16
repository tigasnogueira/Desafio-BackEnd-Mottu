using BikeRentalSystem.Core.Interfaces.Repositories;
using FluentValidation;

namespace BikeRentalSystem.Core.Models.Validations;

public class MotorcycleValidation : AbstractValidator<Motorcycle>
{
    private readonly IUnitOfWork _unitOfWork;

    public MotorcycleValidation(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void ConfigureRulesForCreate()
    {
        RuleFor(m => m.Plate)
            .NotEmpty().WithMessage("The Plate cannot be empty.")
            .Length(1, 10).WithMessage("The Plate must be between 1 and 10 characters.")
            .MustAsync(async (plate, cancellation) =>
            {
                var existingMotorcycle = await _unitOfWork.Motorcycles.Find(m => m.Plate == plate);
                return !existingMotorcycle.Any();
            }).WithMessage("The Plate already exists.");

        ConfigureCommonRules();
    }

    public void ConfigureRulesForUpdate(Motorcycle existingMotorcycle)
    {
        RuleFor(m => m.Plate)
            .NotEmpty().WithMessage("The Plate cannot be empty.")
            .Length(1, 10).WithMessage("The Plate must be between 1 and 10 characters.")
            .MustAsync(async (motorcycle, plate, cancellation) =>
            {
                var existingPlate = await _unitOfWork.Motorcycles.Find(m => m.Plate == plate);
                return !existingPlate.Any() || existingPlate.First().Id == motorcycle.Id;
            }).WithMessage("The Plate already exists.")
            .When(m => existingMotorcycle.Plate != m.Plate);

        ConfigureCommonRules();
    }

    public void ConfigureCommonRules()
    {
        RuleFor(m => m.Year)
            .NotEmpty().WithMessage("The Year cannot be empty.")
            .InclusiveBetween(1900, DateTime.Now.Year + 1).WithMessage($"The Year must be between 1900 and {DateTime.Now.Year + 1}.");

        RuleFor(m => m.Model)
            .NotEmpty().WithMessage("The Model cannot be empty.")
            .Length(1, 50).WithMessage("The Model must be between 1 and 50 characters.");
    }
}
