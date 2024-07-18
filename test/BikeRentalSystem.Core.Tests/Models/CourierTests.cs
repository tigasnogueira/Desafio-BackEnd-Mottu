using BikeRentalSystem.Core.Interfaces.UoW;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Models.Validations;
using BikeRentalSystem.Core.Tests.Helpers;
using FluentAssertions;

namespace BikeRentalSystem.Core.Tests.Models;

public class CourierTests
{
    private readonly IUnitOfWork _unitOfWorkMock;

    public CourierTests()
    {
        _unitOfWorkMock = UnitOfWorkMock.Create();
    }

    [Fact]
    public void CreateCourier_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var name = "John Doe";
        var cnpj = "12345678901234";
        var birthDate = new DateOnly(1990, 1, 1);
        var cnhNumber = "AB123456";
        var cnhType = "A";
        var cnhImage = "cnh.png";

        // Act
        var courier = new Courier(name, cnpj, birthDate, cnhNumber, cnhType, cnhImage);

        // Assert
        courier.Name.Should().Be(name);
        courier.Cnpj.Should().Be(cnpj);
        courier.BirthDate.Should().Be(birthDate);
        courier.CnhNumber.Should().Be(cnhNumber);
        courier.CnhType.Should().Be(cnhType);
        courier.CnhImage.Should().Be(cnhImage);
        courier.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        courier.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void Update_ShouldUpdateUpdatedAt()
    {
        // Arrange
        var courier = new Courier();
        var initialUpdatedAt = courier.UpdatedAt;

        // Act
        courier.Update();

        // Assert
        if (initialUpdatedAt.HasValue)
            courier.UpdatedAt.Should().BeAfter(initialUpdatedAt.Value);
        courier.UpdatedAt.Should().HaveValue().And.BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void IsDeletedToggle_ShouldToggleIsDeleted()
    {
        // Arrange
        var courier = new Courier();

        // Act & Assert
        courier.ToggleIsDeleted();
        courier.IsDeleted.Should().BeTrue();

        courier.ToggleIsDeleted();
        courier.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateCourier_WithValidData_ShouldPass()
    {
        // Arrange
        var validator = new CourierValidation(_unitOfWorkMock);
        validator.ConfigureRulesForCreate();
        var courier = new Courier("John Doe", "12345678901234", new DateOnly(1990, 1, 1), "AB123456", "A", "cnh.png");

        // Act
        var result = await validator.ValidateAsync(courier);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateCourier_WithInvalidCnpj_ShouldFail()
    {
        // Arrange
        var validator = new CourierValidation(_unitOfWorkMock);
        validator.ConfigureRulesForCreate();
        var courier = new Courier("John Doe", "123", new DateOnly(1990, 1, 1), "AB123456", "A", "cnh.png");

        // Act
        var result = await validator.ValidateAsync(courier);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Cnpj" && e.ErrorMessage == "The CNPJ must be 14 characters long.");
    }

    [Fact]
    public async Task ValidateImageAsync_WithInvalidFormat_ShouldFail()
    {
        // Arrange
        var courier = new Courier { CnhImage = "invalid_image.jpg" };
        var validator = new CourierValidation(_unitOfWorkMock);

        // Act
        var result = await validator.ValidateImageAsync(courier);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "CnhImage" && e.ErrorMessage == "The CNH Image must be a PNG or BMP file.");
    }
}
