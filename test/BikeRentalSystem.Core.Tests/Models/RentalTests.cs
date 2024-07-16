using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Models.Enums;
using BikeRentalSystem.Core.Models.Validations;
using BikeRentalSystem.Core.Tests.Helpers;
using FluentAssertions;

namespace BikeRentalSystem.Core.Tests.Models;

public class RentalTests
{
    private readonly IUnitOfWork _unitOfWorkMock;

    public RentalTests()
    {
        _unitOfWorkMock = UnitOfWorkMock.Create();
    }

    [Fact]
    public void CreateRental_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var courierId = Guid.NewGuid();
        var motorcycleId = Guid.NewGuid();
        var startDate = DateTime.UtcNow.AddDays(1);
        var endDate = DateTime.UtcNow.AddDays(8);
        var expectedEndDate = DateTime.UtcNow.AddDays(7);
        var dailyRate = 30m;
        var plan = RentalPlan.SevenDays;

        // Act
        var rental = new Rental(courierId, motorcycleId, startDate, endDate, expectedEndDate, dailyRate, plan);

        // Assert
        rental.CourierId.Should().Be(courierId);
        rental.MotorcycleId.Should().Be(motorcycleId);
        rental.StartDate.Should().Be(startDate);
        rental.EndDate.Should().Be(endDate);
        rental.ExpectedEndDate.Should().Be(expectedEndDate);
        rental.DailyRate.Should().Be(dailyRate);
        rental.Plan.Should().Be(plan);
        rental.TotalCost.Should().Be(210m);
        rental.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        rental.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void Update_ShouldUpdateUpdatedAt()
    {
        // Arrange
        var rental = new Rental();
        var initialUpdatedAt = rental.UpdatedAt;

        // Act
        rental.Update();

        // Assert
        rental.UpdatedAt.Should().BeAfter(initialUpdatedAt);
        rental.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void IsDeletedToggle_ShouldToggleIsDeleted()
    {
        // Arrange
        var rental = new Rental();

        // Act & Assert
        rental.IsDeletedToggle();
        rental.IsDeleted.Should().BeTrue();

        rental.IsDeletedToggle();
        rental.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateRental_WithValidData_ShouldPass()
    {
        // Arrange
        var validator = new RentalValidation(_unitOfWorkMock);
        var rental = new Rental(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(8), DateTime.UtcNow.AddDays(7), 30m, RentalPlan.SevenDays);

        // Act
        var result = await validator.ValidateAsync(rental);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateRental_WithInvalidStartDate_ShouldFail()
    {
        // Arrange
        var validator = new RentalValidation(_unitOfWorkMock);
        var rental = new Rental(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(8), DateTime.UtcNow.AddDays(7), 30m, RentalPlan.SevenDays);

        // Act
        var result = await validator.ValidateAsync(rental);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "StartDate" && e.ErrorMessage == "The Start Date must be in the future.");
    }

    [Fact]
    public async Task ValidateRental_WithInvalidEndDate_ShouldFail()
    {
        // Arrange
        var validator = new RentalValidation(_unitOfWorkMock);
        var rental = new Rental(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(1), DateTime.UtcNow, DateTime.UtcNow.AddDays(7), 30m, RentalPlan.SevenDays);

        // Act
        var result = await validator.ValidateAsync(rental);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "EndDate" && e.ErrorMessage == "The End Date must be after the Start Date.");
    }

    [Fact]
    public async Task ValidateRental_WithInvalidDailyRate_ShouldFail()
    {
        // Arrange
        var validator = new RentalValidation(_unitOfWorkMock);
        var rental = new Rental(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(8), DateTime.UtcNow.AddDays(7), -10m, RentalPlan.SevenDays);

        // Act
        var result = await validator.ValidateAsync(rental);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "DailyRate" && e.ErrorMessage == "The Daily Rate must be a positive number.");
    }
}
