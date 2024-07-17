﻿using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Models.Validations;
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

public class CourierServiceTests
{
    private readonly ICourierService _courierService;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IMessageProducer _messageProducerMock;
    private readonly INotifier _notifierMock;

    public CourierServiceTests()
    {
        _unitOfWorkMock = UnitOfWorkMock.Create();
        _messageProducerMock = MessageProducerMock.Create();
        _notifierMock = NotifierMock.Create();
        _courierService = new CourierService(_unitOfWorkMock, _messageProducerMock, _notifierMock);
    }

    [Fact]
    public async Task GetById_ShouldReturnCourier_WhenCourierExists()
    {
        // Arrange
        var courierId = Guid.NewGuid();
        var courier = new Courier { Id = courierId, Name = "John Doe", Cnpj = "12345678901234" };
        _unitOfWorkMock.Couriers.GetById(courierId).Returns(courier);

        // Act
        var result = await _courierService.GetById(courierId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(courierId);
        _notifierMock.Received().Handle("Getting courier by ID");
    }

    [Fact]
    public async Task GetById_ShouldReturnNull_WhenCourierDoesNotExist()
    {
        // Arrange
        var courierId = Guid.NewGuid();
        _unitOfWorkMock.Couriers.GetById(courierId).Returns((Courier)null);

        // Act
        var result = await _courierService.GetById(courierId);

        // Assert
        result.Should().BeNull();
        _notifierMock.Received().Handle("Getting courier by ID");
    }

    [Fact]
    public async Task Add_ShouldReturnTrue_WhenCourierIsValid()
    {
        // Arrange
        var courier = new Courier
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Cnpj = "12345678901234",
            CnhNumber = "AB123456",
            CnhType = "A",
            BirthDate = new DateTime(1990, 1, 1)
        };

        var validator = new CourierValidation(_unitOfWorkMock);
        validator.ConfigureRulesForCreate();

        var validationResult = new ValidationResult();
        var validationMock = Substitute.For<AbstractValidator<Courier>>();
        validationMock.ValidateAsync(courier).Returns(validationResult);

        _unitOfWorkMock.Couriers.Add(courier).Returns(Task.CompletedTask);
        _unitOfWorkMock.SaveAsync().Returns(1);

        // Act
        var result = await _courierService.Add(courier);

        // Assert
        result.Should().BeTrue();
        _notifierMock.Received().Handle("Courier added successfully");
        await _unitOfWorkMock.Couriers.Received().Add(courier);
        await _unitOfWorkMock.Received().SaveAsync();
    }

    [Fact]
    public async Task Add_ShouldReturnFalse_WhenCourierIsInvalid()
    {
        // Arrange
        var courier = new Courier { Id = Guid.NewGuid(), Name = "", Cnpj = "12345678901234", CnhNumber = "AB123456", CnhType = "A" };
        var validator = new CourierValidation(_unitOfWorkMock);
        validator.ConfigureRulesForCreate();
        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure("Name", "The Name cannot be empty."),
            new ValidationFailure("Name", "The Name must be between 1 and 100 characters."),
            new ValidationFailure("BirthDate", "The Date of Birth cannot be empty.")
        });

        // Mocking the validation
        var validationMock = Substitute.For<AbstractValidator<Courier>>();
        validationMock.ValidateAsync(courier).Returns(validationResult);

        // Act
        var result = await _courierService.Add(courier);

        // Assert
        result.Should().BeFalse();
        _notifierMock.Received().NotifyValidationErrors(Arg.Is<ValidationResult>(v =>
            v.Errors.Any(e => e.PropertyName == "Name" && e.ErrorMessage == "The Name cannot be empty.") &&
            v.Errors.Any(e => e.PropertyName == "Name" && e.ErrorMessage == "The Name must be between 1 and 100 characters.") &&
            v.Errors.Any(e => e.PropertyName == "BirthDate" && e.ErrorMessage == "The Date of Birth cannot be empty.")
        ));
        _unitOfWorkMock.Couriers.DidNotReceive().Add(Arg.Any<Courier>());
    }

    [Fact]
    public async Task Update_ShouldReturnTrue_WhenCourierIsValid()
    {
        // Arrange
        var courierId = Guid.NewGuid();
        var existingCourier = new Courier { Id = courierId, Name = "John Doe", Cnpj = "12345678901234" };
        var updatedCourier = new Courier { Id = courierId, Name = "Jane Doe", Cnpj = "12345678901234", CnhNumber = "AB123456", CnhType = "A", BirthDate = new DateTime(1990, 1, 1) };

        _unitOfWorkMock.Couriers.GetById(courierId).Returns(existingCourier);
        _unitOfWorkMock.SaveAsync().Returns(1);

        // Act
        var result = await _courierService.Update(updatedCourier);

        // Assert
        result.Should().BeTrue();
        _notifierMock.Received().Handle("Courier updated successfully");
        _unitOfWorkMock.Couriers.Received().Update(existingCourier);
        await _unitOfWorkMock.Received().SaveAsync();
    }

    [Fact]
    public async Task Update_ShouldReturnFalse_WhenCourierIsInvalid()
    {
        // Arrange
        var courierId = Guid.NewGuid();
        var existingCourier = new Courier { Id = courierId, Name = "John Doe", Cnpj = "12345678901234" };
        var updatedCourier = new Courier { Id = courierId, Name = "", Cnpj = "12345678901234", CnhNumber = "AB123456", CnhType = "A" };
        _unitOfWorkMock.Couriers.GetById(courierId).Returns(existingCourier);
        var validator = new CourierValidation(_unitOfWorkMock);
        validator.ConfigureRulesForUpdate(existingCourier);
        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure("Name", "The Name cannot be empty.")
        });

        // Mocking the validation
        var validationMock = Substitute.For<AbstractValidator<Courier>>();
        validationMock.ValidateAsync(updatedCourier).Returns(validationResult);

        // Act
        var result = await _courierService.Update(updatedCourier);

        // Assert
        result.Should().BeFalse();
        _notifierMock.Received().NotifyValidationErrors(Arg.Is<ValidationResult>(v =>
            v.Errors.Any(e => e.PropertyName == "Name" && e.ErrorMessage == "The Name cannot be empty.")
        ));
        _unitOfWorkMock.Couriers.DidNotReceive().Update(Arg.Any<Courier>());
    }

    [Fact]
    public async Task SoftDelete_ShouldReturnTrue_WhenCourierExists()
    {
        // Arrange
        var courierId = Guid.NewGuid();
        var courier = new Courier { Id = courierId, Name = "John Doe", Cnpj = "12345678901234" };
        _unitOfWorkMock.Couriers.GetById(courierId).Returns(courier);
        _unitOfWorkMock.SaveAsync().Returns(1);

        // Act
        var result = await _courierService.SoftDelete(courierId);

        // Assert
        result.Should().BeTrue();
        _notifierMock.Received().Handle("Courier soft deleted successfully");
        _unitOfWorkMock.Couriers.Received().Update(courier);
        await _unitOfWorkMock.Received().SaveAsync();
    }

    [Fact]
    public async Task SoftDelete_ShouldReturnFalse_WhenCourierDoesNotExist()
    {
        // Arrange
        var courierId = Guid.NewGuid();
        _unitOfWorkMock.Couriers.GetById(courierId).Returns((Courier)null);

        // Act
        var result = await _courierService.SoftDelete(courierId);

        // Assert
        result.Should().BeFalse();
        _notifierMock.Received().Handle("Courier not found", NotificationType.Error);
        _unitOfWorkMock.Couriers.DidNotReceive().Update(Arg.Any<Courier>());
    }

    [Fact]
    public async Task AddOrUpdateCnhImage_ShouldReturnTrue_WhenCnhImageIsUpdated()
    {
        // Arrange
        var cnpj = "12345678901234";
        var courier = new Courier { Cnpj = cnpj, Name = "John Doe", CnhNumber = "AB123456", CnhType = "A" };
        _unitOfWorkMock.Couriers.GetByCnpj(cnpj).Returns(courier);
        _unitOfWorkMock.Couriers.AddOrUpdateCnhImage(cnpj, Arg.Any<Stream>()).Returns("http://example.com/AB123456.png");
        _unitOfWorkMock.SaveAsync().Returns(1);

        // Act
        var result = await _courierService.AddOrUpdateCnhImage(cnpj, new MemoryStream());

        // Assert
        result.Should().BeTrue();
        _notifierMock.Received().Handle("CNH image updated successfully");
        await _unitOfWorkMock.Received().SaveAsync();
    }

    [Fact]
    public async Task AddOrUpdateCnhImage_ShouldReturnFalse_WhenCourierDoesNotExist()
    {
        // Arrange
        var cnpj = "12345678901234";
        _unitOfWorkMock.Couriers.GetByCnpj(cnpj).Returns((Courier)null);

        // Act
        var result = await _courierService.AddOrUpdateCnhImage(cnpj, new MemoryStream());

        // Assert
        result.Should().BeFalse();
        _notifierMock.Received().Handle("Courier with CNPJ 12345678901234 not found.", NotificationType.Error);
        _unitOfWorkMock.Couriers.DidNotReceive().AddOrUpdateCnhImage(Arg.Any<string>(), Arg.Any<Stream>());
    }
}