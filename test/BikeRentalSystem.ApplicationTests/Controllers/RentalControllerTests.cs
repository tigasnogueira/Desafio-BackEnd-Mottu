using AutoMapper;
using BikeRentalSystem.Api.Controllers.V1;
using BikeRentalSystem.Api.Dtos;
using BikeRentalSystem.Core.Interfaces;
using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BikeRentalSystem.ApplicationTests.Controllers;

public class RentalControllerTests
{
    private readonly Mock<IRentalService> _mockService = new Mock<IRentalService>();
    private readonly Mock<ILogger<RentalController>> _mockLogger = new Mock<ILogger<RentalController>>();
    private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();
    private readonly Mock<INotifier> _mockNotifier = new Mock<INotifier>();
    private readonly Mock<IUser> _mockUser = new Mock<IUser>();
    private readonly RentalController _controller;

    public RentalControllerTests()
    {
        _controller = new RentalController(_mockService.Object, _mockLogger.Object, _mockMapper.Object, _mockNotifier.Object, _mockUser.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsExpectedRentals()
    {
        // Arrange
        var rentals = new List<Rental> { new Rental { Id = Guid.NewGuid(), StartDate = DateTime.Now } };
        var rentalDtos = new List<RentalDto> { new RentalDto { Id = rentals[0].Id, StartDate = rentals[0].StartDate } };

        _mockService.Setup(service => service.GetAllAsync()).ReturnsAsync(rentals);
        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<RentalDto>>(rentals)).Returns(rentalDtos);

        // Act
        var result = await _controller.GetAll(null, null);

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<RentalDto>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(rentalDtos, okResult.Value);
    }

    [Fact]
    public async Task GetAll_ReturnsExpectedRentalsPaged()
    {
        // Arrange
        var rentals = new List<Rental> { new Rental { Id = Guid.NewGuid(), StartDate = DateTime.Now } };
        var rentalDtos = new List<RentalDto> { new RentalDto { Id = rentals[0].Id, StartDate = rentals[0].StartDate } };

        _mockService.Setup(service => service.GetAllAsync()).ReturnsAsync(rentals);
        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<RentalDto>>(rentals)).Returns(rentalDtos);

        // Act
        var result = await _controller.GetAll(1, 20);

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<RentalDto>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(rentalDtos, okResult.Value);
    }

    [Fact]
    public async Task GetById_ReturnsExpectedRental()
    {
        // Arrange
        var rental = new Rental { Id = Guid.NewGuid(), StartDate = DateTime.Now };
        var rentalDto = new RentalDto { Id = rental.Id, StartDate = rental.StartDate };

        _mockService.Setup(service => service.GetRentalByIdAsync(rental.Id)).ReturnsAsync(rental);
        _mockMapper.Setup(mapper => mapper.Map<RentalDto>(rental)).Returns(rentalDto);

        // Act
        var result = await _controller.GetById(rental.Id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<RentalDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(rentalDto, okResult.Value);
    }

    [Fact]
    public async Task Add_ReturnsExpectedRental()
    {
        // Arrange
        var rentalDto = new RentalDto { Id = Guid.NewGuid(), StartDate = DateTime.Now };
        var rental = new Rental { Id = rentalDto.Id, StartDate = rentalDto.StartDate };

        _mockMapper.Setup(mapper => mapper.Map<Rental>(rentalDto)).Returns(rental);
        _mockService.Setup(service => service.AddRentalAsync(rental)).ReturnsAsync(rental);

        // Act
        var result = await _controller.Add(rentalDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<RentalDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(rentalDto, okResult.Value);
    }

    [Fact]
    public async Task Update_ReturnsExpectedRental()
    {
        // Arrange
        var rentalDto = new RentalDto { Id = Guid.NewGuid(), StartDate = DateTime.Now };
        var rental = new Rental { Id = rentalDto.Id, StartDate = rentalDto.StartDate };

        _mockMapper.Setup(mapper => mapper.Map<Rental>(rentalDto)).Returns(rental);
        _mockService.Setup(service => service.UpdateRentalAsync(rental)).ReturnsAsync(rental);

        // Act
        var result = await _controller.Update(rentalDto.Id, rentalDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<RentalDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(rentalDto, okResult.Value);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdDoesNotMatch()
    {
        // Arrange
        var rentalDto = new RentalDto { Id = Guid.NewGuid(), StartDate = DateTime.Now };

        // Act
        var result = await _controller.Update(Guid.NewGuid(), rentalDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<RentalDto>>(result);
        Assert.IsType<BadRequestResult>(actionResult.Result);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenRentalIsNull()
    {
        // Arrange
        var rentalDto = new RentalDto { Id = Guid.NewGuid(), StartDate = DateTime.Now };

        _mockService.Setup(service => service.GetRentalByIdAsync(rentalDto.Id)).ReturnsAsync((Rental)null);

        // Act
        var result = await _controller.Update(rentalDto.Id, rentalDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<RentalDto>>(result);
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    [Fact]
    public async Task Delete_ReturnsExpectedRental()
    {
        // Arrange
        var rental = new Rental { Id = Guid.NewGuid(), StartDate = DateTime.Now };
        var rentalDto = new RentalDto { Id = rental.Id, StartDate = rental.StartDate };

        _mockService.Setup(service => service.GetRentalByIdAsync(rental.Id)).ReturnsAsync(rental);

        // Act
        var result = await _controller.Delete(rental.Id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<RentalDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(rentalDto, okResult.Value);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenRentalIsNull()
    {
        // Arrange
        var rentalId = Guid.NewGuid();

        _mockService.Setup(service => service.GetRentalByIdAsync(rentalId)).ReturnsAsync((Rental)null);

        // Act
        var result = await _controller.Delete(rentalId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<RentalDto>>(result);
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    [Fact]
    public async Task Delete_ReturnsExpectedRental_WhenRentalIsDeleted()
    {
        // Arrange
        var rental = new Rental { Id = Guid.NewGuid(), StartDate = DateTime.Now };
        var rentalDto = new RentalDto { Id = rental.Id, StartDate = rental.StartDate };

        _mockService.Setup(service => service.GetRentalByIdAsync(rental.Id)).ReturnsAsync(rental);
        _mockService.Setup(service => service.DeleteRentalAsync(rental.Id)).ReturnsAsync(rental);

        // Act
        var result = await _controller.Delete(rental.Id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<RentalDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(rentalDto, okResult.Value);
    }

    [Fact]
    public async Task Delete_ReturnsBadRequest_WhenRentalIdIsEmpty()
    {
        // Act
        var result = await _controller.Delete(Guid.Empty);

        // Assert
        var actionResult = Assert.IsType<ActionResult<RentalDto>>(result);
        Assert.IsType<BadRequestResult>(actionResult.Result);
    }

    [Fact]
    public async Task Delete_ReturnsBadRequest_WhenRentalIsNull()
    {
        // Arrange
        var rentalId = Guid.NewGuid();

        _mockService.Setup(service => service.GetRentalByIdAsync(rentalId)).ReturnsAsync((Rental)null);

        // Act
        var result = await _controller.Delete(rentalId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<RentalDto>>(result);
        Assert.IsType<BadRequestResult>(actionResult.Result);
    }

    [Fact]
    public async Task Delete_ReturnsBadRequest_WhenRentalIdIsNull()
    {
        // Act
        var result = await _controller.Delete(Guid.Empty);

        // Assert
        var actionResult = Assert.IsType<ActionResult<RentalDto>>(result);
        Assert.IsType<BadRequestResult>(actionResult.Result);
    }
}
