using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Models.Validations;
using BikeRentalSystem.Core.Tests.Helpers;
using FluentAssertions;

namespace BikeRentalSystem.Core.Tests.Models;

public class MotorcycleTests
{
    private readonly IUnitOfWork _unitOfWorkMock;

    public MotorcycleTests()
    {
        _unitOfWorkMock = UnitOfWorkMock.Create();
    }

    [Fact]
    public void CreateMotorcycle_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var year = 2023;
        var model = "Yamaha";
        var plate = "XYZ1234";

        // Act
        var motorcycle = new Motorcycle(year, model, plate);

        // Assert
        motorcycle.Year.Should().Be(year);
        motorcycle.Model.Should().Be(model);
        motorcycle.Plate.Should().Be(plate);
        motorcycle.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        motorcycle.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void Update_ShouldUpdateUpdatedAt()
    {
        // Arrange
        var motorcycle = new Motorcycle();
        var initialUpdatedAt = motorcycle.UpdatedAt;

        // Act
        motorcycle.Update();

        // Assert
        motorcycle.UpdatedAt.Should().BeAfter(initialUpdatedAt);
        motorcycle.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void IsDeletedToggle_ShouldToggleIsDeleted()
    {
        // Arrange
        var motorcycle = new Motorcycle();

        // Act & Assert
        motorcycle.IsDeletedToggle();
        motorcycle.IsDeleted.Should().BeTrue();

        motorcycle.IsDeletedToggle();
        motorcycle.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateMotorcycle_WithValidData_ShouldPass()
    {
        // Arrange
        var validator = new MotorcycleValidation(_unitOfWorkMock);
        validator.ConfigureRulesForCreate();
        var motorcycle = new Motorcycle(2023, "Yamaha", "XYZ1234");

        // Act
        var result = await validator.ValidateAsync(motorcycle);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateMotorcycle_WithInvalidPlate_ShouldFail()
    {
        // Arrange
        var validator = new MotorcycleValidation(_unitOfWorkMock);
        validator.ConfigureRulesForCreate();
        var motorcycle = new Motorcycle(2023, "Yamaha", "");

        // Act
        var result = await validator.ValidateAsync(motorcycle);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Plate" && e.ErrorMessage == "The Plate cannot be empty.");
    }

    [Fact]
    public async Task ValidateMotorcycle_WithInvalidYear_ShouldFail()
    {
        // Arrange
        var validator = new MotorcycleValidation(_unitOfWorkMock);
        validator.ConfigureRulesForCreate();
        var motorcycle = new Motorcycle(1800, "Yamaha", "XYZ1234");

        // Act
        var result = await validator.ValidateAsync(motorcycle);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Year" && e.ErrorMessage == $"The Year must be between 1900 and {DateTime.Now.Year + 1}.");
    }
}
