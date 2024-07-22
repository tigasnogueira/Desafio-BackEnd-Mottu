using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Interfaces.UoW;
using BikeRentalSystem.Core.Models;
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

public class MotorcycleServiceTests
{
    private readonly IMotorcycleService _motorcycleService;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly INotifier _notifierMock;
    private readonly IMessageProducer _messageProducerMock;

    public MotorcycleServiceTests()
    {
        _unitOfWorkMock = UnitOfWorkMock.Create();
        _notifierMock = NotifierMock.Create();
        _messageProducerMock = MessageProducerMock.Create();
        _motorcycleService = new MotorcycleService(_unitOfWorkMock, _messageProducerMock, _notifierMock);
    }

    [Fact]
    public async Task Add_ShouldReturnTrue_WhenMotorcycleIsValid()
    {
        // Arrange
        var motorcycle = new Motorcycle
        {
            Id = Guid.NewGuid(),
            Year = 2023,
            Model = "Yamaha",
            Plate = "XYZ1234"
        };

        var validationResult = new ValidationResult();
        var validationMock = Substitute.For<AbstractValidator<Motorcycle>>();
        validationMock.ValidateAsync(motorcycle).Returns(Task.FromResult(validationResult));

        _unitOfWorkMock.Motorcycles.Add(motorcycle).Returns(Task.CompletedTask);
        _unitOfWorkMock.SaveAsync().Returns(Task.FromResult(1));

        // Act
        var result = await _motorcycleService.Add(motorcycle, "TestUser");

        // Assert
        result.Should().BeTrue();
        _notifierMock.Received().Handle("Motorcycle added successfully");
        await _unitOfWorkMock.Motorcycles.Received().Add(motorcycle);
        await _unitOfWorkMock.Received().SaveAsync();
    }

    [Fact]
    public async Task Add_ShouldReturnFalse_WhenMotorcycleIsInvalid()
    {
        // Arrange
        var motorcycle = new Motorcycle
        {
            Id = Guid.NewGuid(),
            Year = 2023,
            Model = string.Empty,
            Plate = "XYZ1234"
        };

        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure("Model", "The Model cannot be empty."),
            new ValidationFailure("Model", "The Model must be between 1 and 50 characters.")
        });

        var validationMock = Substitute.For<AbstractValidator<Motorcycle>>();
        validationMock.ValidateAsync(motorcycle).Returns(Task.FromResult(validationResult));

        // Act
        var result = await _motorcycleService.Add(motorcycle, "TestUser");

        // Assert
        result.Should().BeFalse();
        _notifierMock.Received().NotifyValidationErrors(Arg.Is<ValidationResult>(v =>
            v.Errors.Any(e => e.ErrorMessage == "The Model cannot be empty.") &&
            v.Errors.Any(e => e.ErrorMessage == "The Model must be between 1 and 50 characters.")
        ));
    }

    [Fact]
    public async Task Update_ShouldReturnTrue_WhenMotorcycleIsValid()
    {
        // Arrange
        var motorcycleId = Guid.NewGuid();
        var existingMotorcycle = new Motorcycle { Id = motorcycleId, Year = 2022, Model = "Honda", Plate = "ABC1234" };
        var updatedMotorcycle = new Motorcycle { Id = motorcycleId, Year = 2023, Model = "Yamaha", Plate = "XYZ1234" };

        _unitOfWorkMock.Motorcycles.GetById(motorcycleId).Returns(Task.FromResult(existingMotorcycle));
        _unitOfWorkMock.SaveAsync().Returns(Task.FromResult(1));

        // Act
        var result = await _motorcycleService.Update(updatedMotorcycle, "TestUser");

        // Assert
        result.Should().BeTrue();
        _notifierMock.Received().Handle("Motorcycle updated successfully");
        await _unitOfWorkMock.Motorcycles.Received().Update(existingMotorcycle);
        await _unitOfWorkMock.Received().SaveAsync();
    }

    [Fact]
    public async Task Update_ShouldReturnFalse_WhenMotorcycleIsInvalid()
    {
        // Arrange
        var motorcycleId = Guid.NewGuid();
        var existingMotorcycle = new Motorcycle { Id = motorcycleId, Year = 2022, Model = "Honda", Plate = "ABC1234" };
        var updatedMotorcycle = new Motorcycle { Id = motorcycleId, Year = 2023, Model = string.Empty, Plate = "XYZ1234" };

        _unitOfWorkMock.Motorcycles.GetById(motorcycleId).Returns(Task.FromResult(existingMotorcycle));

        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure("Model", "The Model cannot be empty."),
            new ValidationFailure("Model", "The Model must be between 1 and 50 characters.")
        });

        var validationMock = Substitute.For<AbstractValidator<Motorcycle>>();
        validationMock.ValidateAsync(updatedMotorcycle).Returns(Task.FromResult(validationResult));

        // Act
        var result = await _motorcycleService.Update(updatedMotorcycle, "TestUser");

        // Assert
        result.Should().BeFalse();
        _notifierMock.Received().NotifyValidationErrors(Arg.Is<ValidationResult>(v =>
            v.Errors.Any(e => e.ErrorMessage == "The Model cannot be empty.") &&
            v.Errors.Any(e => e.ErrorMessage == "The Model must be between 1 and 50 characters.")
        ));
        _unitOfWorkMock.Motorcycles.DidNotReceive().Update(Arg.Any<Motorcycle>());
    }

    [Fact]
    public async Task GetById_ShouldReturnMotorcycle_WhenMotorcycleExists()
    {
        // Arrange
        var motorcycleId = Guid.NewGuid();
        var motorcycle = new Motorcycle { Id = motorcycleId, Year = 2023, Model = "Yamaha", Plate = "XYZ1234" };

        _unitOfWorkMock.Motorcycles.GetById(motorcycleId).Returns(Task.FromResult(motorcycle));

        // Act
        var result = await _motorcycleService.GetById(motorcycleId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(motorcycleId);
    }

    [Fact]
    public async Task GetById_ShouldReturnNull_WhenMotorcycleDoesNotExist()
    {
        // Arrange
        var motorcycleId = Guid.NewGuid();
        _unitOfWorkMock.Motorcycles.GetById(motorcycleId).Returns(Task.FromResult<Motorcycle>(null));

        // Act
        var result = await _motorcycleService.GetById(motorcycleId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task SoftDelete_ShouldReturnTrue_WhenMotorcycleExists()
    {
        // Arrange
        var motorcycleId = Guid.NewGuid();
        var motorcycle = new Motorcycle { Id = motorcycleId, Year = 2023, Model = "Yamaha", Plate = "XYZ1234" };

        _unitOfWorkMock.Motorcycles.GetById(motorcycleId).Returns(Task.FromResult(motorcycle));
        _unitOfWorkMock.SaveAsync().Returns(Task.FromResult(1));

        // Act
        var result = await _motorcycleService.SoftDelete(motorcycleId, "TestUser");

        // Assert
        result.Should().BeTrue();
        _notifierMock.Received().Handle("Motorcycle soft deleted successfully");
        await _unitOfWorkMock.Motorcycles.Received().Update(motorcycle);
        await _unitOfWorkMock.Received().SaveAsync();
    }

    [Fact]
    public async Task SoftDelete_ShouldReturnFalse_WhenMotorcycleDoesNotExist()
    {
        // Arrange
        var motorcycleId = Guid.NewGuid();
        _unitOfWorkMock.Motorcycles.GetById(motorcycleId).Returns(Task.FromResult<Motorcycle>(null));

        // Act
        var result = await _motorcycleService.SoftDelete(motorcycleId, "TestUser");

        // Assert
        result.Should().BeFalse();
        _notifierMock.Received().Handle("Motorcycle not found", NotificationType.Error);
        _unitOfWorkMock.Motorcycles.DidNotReceive().Update(Arg.Any<Motorcycle>());
    }
}
