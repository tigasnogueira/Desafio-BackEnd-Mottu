using AutoMapper;
using BikeRentalSystem.Api.Controllers.V1;
using BikeRentalSystem.Api.Dtos;
using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BikeRentalSystem.ApplicationTests.Controllers;

public class MotorcycleControllerTests
{
    private readonly Mock<IMotorcycleService> _mockService = new Mock<IMotorcycleService>();
    private readonly Mock<ILogger<MotorcycleController>> _mockLogger = new Mock<ILogger<MotorcycleController>>();
    private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();
    private readonly Mock<INotifier> _mockNotifier = new Mock<INotifier>();
    private readonly MotorcycleController _controller;

    public MotorcycleControllerTests()
    {
        _controller = new MotorcycleController(_mockService.Object, _mockLogger.Object, _mockMapper.Object, _mockNotifier.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsExpectedMotorcycles()
    {
        // Arrange
        var motorcycles = new List<Motorcycle> { new Motorcycle { Id = Guid.NewGuid(), Model = "Model X" } };
        var motorcycleDtos = new List<MotorcycleDto> { new MotorcycleDto { Id = motorcycles[0].Id, Model = motorcycles[0].Model } };

        _mockService.Setup(service => service.GetAllAsync()).ReturnsAsync(motorcycles);
        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<MotorcycleDto>>(motorcycles)).Returns(motorcycleDtos);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<MotorcycleDto>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(motorcycleDtos, okResult.Value);
    }

    [Fact]
    public async Task GetById_ReturnsExpectedMotorcycle()
    {
        // Arrange
        var motorcycle = new Motorcycle { Id = Guid.NewGuid(), Model = "Model X" };
        var motorcycleDto = new MotorcycleDto { Id = motorcycle.Id, Model = motorcycle.Model };

        _mockService.Setup(service => service.GetMotorcycleByIdAsync(motorcycle.Id)).ReturnsAsync(motorcycle);
        _mockMapper.Setup(mapper => mapper.Map<MotorcycleDto>(motorcycle)).Returns(motorcycleDto);

        // Act
        var result = await _controller.GetById(motorcycle.Id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<MotorcycleDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(motorcycleDto, okResult.Value);
    }

    [Fact]
    public async Task Add_ReturnsExpectedMotorcycle()
    {
        // Arrange
        var motorcycleDto = new MotorcycleDto { Model = "Model X" };
        var motorcycle = new Motorcycle { Model = motorcycleDto.Model };

        _mockMapper.Setup(mapper => mapper.Map<Motorcycle>(motorcycleDto)).Returns(motorcycle);
        _mockService.Setup(service => service.AddMotorcycleAsync(motorcycle)).ReturnsAsync(motorcycle);

        // Act
        var result = await _controller.Add(motorcycleDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<MotorcycleDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(motorcycleDto, okResult.Value);
    }

    [Fact]
    public async Task Update_ReturnsExpectedMotorcycle()
    {
        // Arrange
        var motorcycleDto = new MotorcycleDto { Id = Guid.NewGuid(), Model = "Model X" };
        var motorcycle = new Motorcycle { Id = motorcycleDto.Id, Model = motorcycleDto.Model };

        _mockMapper.Setup(mapper => mapper.Map<Motorcycle>(motorcycleDto)).Returns(motorcycle);
        _mockService.Setup(service => service.UpdateMotorcycleAsync(motorcycle)).ReturnsAsync(motorcycle);

        // Act
        var result = await _controller.Update(motorcycleDto.Id, motorcycleDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<MotorcycleDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(motorcycleDto, okResult.Value);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdDoesNotMatch()
    {
        // Arrange
        var motorcycleDto = new MotorcycleDto { Id = Guid.NewGuid(), Model = "Model X" };

        // Act
        var result = await _controller.Update(Guid.NewGuid(), motorcycleDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<MotorcycleDto>>(result);
        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdIsNull()
    {
        // Arrange
        var motorcycleDto = new MotorcycleDto { Model = "Model X" };

        // Act
        var result = await _controller.Update(Guid.Empty, motorcycleDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<MotorcycleDto>>(result);
        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task Delete_ReturnsExpectedMotorcycle()
    {
        // Arrange
        var motorcycle = new Motorcycle { Id = Guid.NewGuid(), Model = "Model X" };
        var motorcycleDto = new MotorcycleDto { Id = motorcycle.Id, Model = motorcycle.Model };

        _mockService.Setup(service => service.DeleteMotorcycleAsync(motorcycle.Id)).ReturnsAsync(motorcycle);

        // Act
        var result = await _controller.Delete(motorcycle.Id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<MotorcycleDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(motorcycleDto, okResult.Value);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenMotorcycleIsNull()
    {
        // Arrange
        var motorcycleId = Guid.NewGuid();

        // Act
        var result = await _controller.Delete(motorcycleId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<MotorcycleDto>>(result);
        Assert.IsType<NotFoundObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task Delete_ReturnsBadRequest_WhenMotorcycleIdIsEmpty()
    {
        // Act
        var result = await _controller.Delete(Guid.Empty);

        // Assert
        var actionResult = Assert.IsType<ActionResult<MotorcycleDto>>(result);
        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task Delete_ReturnsExpectedMotorcycle_WhenMotorcycleIsNull()
    {
        // Arrange
        var motorcycle = new Motorcycle { Id = Guid.NewGuid(), Model = "Model X" };
        var motorcycleDto = new MotorcycleDto { Id = motorcycle.Id, Model = motorcycle.Model };

        _mockService.Setup(service => service.GetMotorcycleByIdAsync(motorcycle.Id)).ReturnsAsync((Motorcycle)null);
        _mockService.Setup(service => service.DeleteMotorcycleAsync(motorcycle.Id)).ReturnsAsync(motorcycle);

        // Act
        var result = await _controller.Delete(motorcycle.Id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<MotorcycleDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(motorcycleDto, okResult.Value);
    }
}
