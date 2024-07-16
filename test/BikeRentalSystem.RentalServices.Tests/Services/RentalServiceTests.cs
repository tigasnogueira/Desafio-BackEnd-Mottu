﻿using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Models.Enums;
using BikeRentalSystem.Core.Notifications;
using BikeRentalSystem.Core.Tests.Helpers;
using BikeRentalSystem.Messaging.Interfaces;
using BikeRentalSystem.RentalServices.Services;
using BikeRentalSystem.RentalServices.Tests.Helpers;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;

namespace BikeRentalSystem.RentalServices.Tests.Services;

public class RentalServiceTests
{
    private readonly IRentalService _rentalService;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly INotifier _notifierMock;
    private readonly IMessageProducer _messageProducerMock;

    public RentalServiceTests()
    {
        _unitOfWorkMock = UnitOfWorkMock.Create();
        _notifierMock = NotifierMock.Create();
        _messageProducerMock = MessageProducerMock.Create();
        _rentalService = new RentalService(_unitOfWorkMock, _messageProducerMock, _notifierMock);
    }

    [Fact]
    public async Task Add_ShouldReturnTrue_WhenRentalIsValid()
    {
        // Arrange
        var rental = new Rental
        {
            Id = Guid.NewGuid(),
            CourierId = Guid.NewGuid(),
            MotorcycleId = Guid.NewGuid(),
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(8),
            ExpectedEndDate = DateTime.UtcNow.AddDays(7),
            DailyRate = 30m,
            Plan = RentalPlan.SevenDays
        };

        var validationResult = new ValidationResult();
        var validatorMock = Substitute.For<AbstractValidator<Rental>>();
        validatorMock.ValidateAsync(rental).Returns(validationResult);

        _unitOfWorkMock.Rentals.Add(rental).Returns(Task.CompletedTask);
        _unitOfWorkMock.SaveAsync().Returns(1);

        // Act
        var result = await _rentalService.Add(rental);

        // Assert
        result.Should().BeTrue();
        _notifierMock.Received().Handle("Rental added successfully");
        await _unitOfWorkMock.Rentals.Received().Add(rental);
        await _unitOfWorkMock.Received().SaveAsync();
    }

    [Fact]
    public async Task Add_ShouldReturnFalse_WhenRentalIsInvalid()
    {
        // Arrange
        var rental = new Rental
        {
            Id = Guid.NewGuid(),
            CourierId = Guid.NewGuid(),
            MotorcycleId = Guid.NewGuid(),
            StartDate = DateTime.UtcNow.AddDays(-1),
            EndDate = DateTime.UtcNow.AddDays(8),
            ExpectedEndDate = DateTime.UtcNow.AddDays(7),
            DailyRate = 30m,
            Plan = RentalPlan.SevenDays
        };

        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure("StartDate", "The Start Date must be in the future.")
        });

        var validatorMock = Substitute.For<AbstractValidator<Rental>>();
        validatorMock.ValidateAsync(rental).Returns(validationResult);

        // Act
        var result = await _rentalService.Add(rental);

        // Assert
        result.Should().BeFalse();
        _notifierMock.Received().NotifyValidationErrors(Arg.Is<ValidationResult>(v =>
            v.Errors.Any(e => e.ErrorMessage == "The Start Date must be in the future.")
        ));
    }

    [Fact]
    public async Task Update_ShouldReturnTrue_WhenRentalIsValid()
    {
        // Arrange
        var rentalId = Guid.NewGuid();
        var existingRental = new Rental
        {
            Id = rentalId,
            CourierId = Guid.NewGuid(),
            MotorcycleId = Guid.NewGuid(),
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(8),
            ExpectedEndDate = DateTime.UtcNow.AddDays(7),
            DailyRate = 30m,
            Plan = RentalPlan.SevenDays,
            TotalCost = 210m
        };

        var updatedRental = new Rental
        {
            Id = rentalId,
            CourierId = existingRental.CourierId,
            MotorcycleId = existingRental.MotorcycleId,
            StartDate = DateTime.UtcNow.AddDays(2),
            EndDate = DateTime.UtcNow.AddDays(9),
            ExpectedEndDate = DateTime.UtcNow.AddDays(8),
            DailyRate = 30m,
            Plan = RentalPlan.SevenDays,
            TotalCost = 240m
        };

        var validationResult = new ValidationResult();

        var validationMock = Substitute.For<AbstractValidator<Rental>>();
        validationMock.ValidateAsync(updatedRental).Returns(validationResult);

        _unitOfWorkMock.Rentals.GetById(rentalId).Returns(existingRental);
        _unitOfWorkMock.SaveAsync().Returns(1);

        // Act
        var result = await _rentalService.Update(updatedRental);

        // Assert
        result.Should().BeTrue();
        _notifierMock.Received().Handle("Rental updated successfully");
        await _unitOfWorkMock.Rentals.Received().Update(existingRental);
        await _unitOfWorkMock.Received().SaveAsync();
    }

    [Fact]
    public async Task Update_ShouldReturnFalse_WhenRentalIsInvalid()
    {
        // Arrange
        var rentalId = Guid.NewGuid();
        var existingRental = new Rental { Id = rentalId, StartDate = DateTime.UtcNow.AddDays(1) };
        var updatedRental = new Rental { Id = rentalId, StartDate = DateTime.UtcNow.AddDays(-1) };

        _unitOfWorkMock.Rentals.GetById(rentalId).Returns(existingRental);

        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure("StartDate", "The Start Date must be in the future.")
        });

        var validatorMock = Substitute.For<AbstractValidator<Rental>>();
        validatorMock.ValidateAsync(updatedRental).Returns(validationResult);

        // Act
        var result = await _rentalService.Update(updatedRental);

        // Assert
        result.Should().BeFalse();
        _notifierMock.Received().NotifyValidationErrors(Arg.Is<ValidationResult>(v =>
            v.Errors.Any(e => e.ErrorMessage == "The Start Date must be in the future.")
        ));
    }

    [Fact]
    public async Task SoftDelete_ShouldReturnTrue_WhenRentalExists()
    {
        // Arrange
        var rentalId = Guid.NewGuid();
        var rental = new Rental { Id = rentalId, StartDate = DateTime.UtcNow.AddDays(1) };

        _unitOfWorkMock.Rentals.GetById(rentalId).Returns(rental);
        _unitOfWorkMock.SaveAsync().Returns(1);

        // Act
        var result = await _rentalService.SoftDelete(rentalId);

        // Assert
        result.Should().BeTrue();
        _notifierMock.Received().Handle("Rental soft deleted successfully");
        await _unitOfWorkMock.Rentals.Received().Update(rental);
        await _unitOfWorkMock.Received().SaveAsync();
    }

    [Fact]
    public async Task SoftDelete_ShouldReturnFalse_WhenRentalDoesNotExist()
    {
        // Arrange
        var rentalId = Guid.NewGuid();
        _unitOfWorkMock.Rentals.GetById(rentalId).Returns(Task.FromResult<Rental>(null));

        // Act
        var result = await _rentalService.SoftDelete(rentalId);

        // Assert
        result.Should().BeFalse();
        _notifierMock.Received().Handle("Rental not found", NotificationType.Error);
    }

    [Fact]
    public async Task CalculateRentalCost_ShouldReturnCorrectCost_ForEarlyReturn()
    {
        // Arrange
        var rentalId = Guid.NewGuid();
        var expectedCost = 150m;

        _unitOfWorkMock.Rentals.CalculateRentalCost(rentalId).Returns(expectedCost);

        // Act
        var result = await _rentalService.CalculateRentalCost(rentalId);

        // Assert
        result.Should().Be(expectedCost);
        _unitOfWorkMock.Rentals.Received(1).CalculateRentalCost(rentalId);
    }

    [Fact]
    public async Task CalculateRentalCost_ShouldReturnCorrectCost_ForLateReturn()
    {
        // Arrange
        var rentalId = Guid.NewGuid();
        var expectedCost = 330m;

        _unitOfWorkMock.Rentals.CalculateRentalCost(rentalId).Returns(expectedCost);

        // Act
        var result = await _rentalService.CalculateRentalCost(rentalId);

        // Assert
        result.Should().Be(expectedCost);
        _unitOfWorkMock.Rentals.Received(1).CalculateRentalCost(rentalId);
    }

    [Fact]
    public async Task GetById_ShouldReturnRental_WhenRentalExists()
    {
        // Arrange
        var rentalId = Guid.NewGuid();
        var rental = new Rental { Id = rentalId, StartDate = DateTime.UtcNow.AddDays(1) };

        _unitOfWorkMock.Rentals.GetById(rentalId).Returns(Task.FromResult(rental));

        // Act
        var result = await _rentalService.GetById(rentalId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(rentalId);
    }

    [Fact]
    public async Task GetById_ShouldReturnNull_WhenRentalDoesNotExist()
    {
        // Arrange
        var rentalId = Guid.NewGuid();
        _unitOfWorkMock.Rentals.GetById(rentalId).Returns(Task.FromResult<Rental>(null));

        // Act
        var result = await _rentalService.GetById(rentalId);

        // Assert
        result.Should().BeNull();
    }
}
