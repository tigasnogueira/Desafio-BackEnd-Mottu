﻿using BikeRentalSystem.Core.Interfaces.Repositories;
using FluentValidation;

namespace BikeRentalSystem.Core.Models.Validations;

public class CourierValidation : AbstractValidator<Courier>
{
    private readonly ICourierRepository _courierRepository;

    public CourierValidation(ICourierRepository courierRepository)
    {
        _courierRepository = courierRepository;

        RuleFor(c => c.Identifier)
            .NotEmpty().WithMessage("The Identifier cannot be empty.");

        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The Name cannot be empty.")
            .Length(1, 100).WithMessage("The Name must be between 1 and 100 characters.");

        RuleFor(c => c.Cnpj)
            .NotEmpty().WithMessage("The CNPJ cannot be empty.")
            .Length(14).WithMessage("The CNPJ must be 14 characters long.")
            .MustAsync(async (cnpj, cancellation) => await CNPJIsUnique(cnpj)).WithMessage("The CNPJ already exists.");

        RuleFor(c => c.BirthDate)
            .NotEmpty().WithMessage("The Date of Birth cannot be empty.")
            .LessThan(DateTime.Now).WithMessage("The Date of Birth must be in the past.");

        RuleFor(c => c.CnhNumber)
            .NotEmpty().WithMessage("The CNH Number cannot be empty.")
            .MustAsync(async (cnh, cancellation) => await CNHIsUnique(cnh)).WithMessage("The CNH Number already exists.");

        RuleFor(c => c.CnhType)
            .NotEmpty().WithMessage("The CNH Type cannot be empty.")
            .Must(type => type == "A" || type == "B" || type == "A+B").WithMessage("The CNH Type must be 'A', 'B', or 'A+B'.");

        RuleFor(c => c.CnhImage)
            .NotEmpty().WithMessage("The CNH Image cannot be empty.")
            .Must(image => image.EndsWith(".png") || image.EndsWith(".bmp")).WithMessage("The CNH Image must be a PNG or BMP file.");
    }

    private async Task<bool> CNPJIsUnique(string cnpj)
    {
        var existingCourier = await _courierRepository.Find(c => c.Cnpj == cnpj);
        return existingCourier == null;
    }

    private async Task<bool> CNHIsUnique(string cnh)
    {
        var existingCourier = await _courierRepository.Find(c => c.CnhNumber == cnh);
        return existingCourier == null;
    }
}
