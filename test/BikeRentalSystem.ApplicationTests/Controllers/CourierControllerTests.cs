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

public class CourierControllerTests
{
    private readonly Mock<ICourierService> _mockService = new Mock<ICourierService>();
    private readonly Mock<ILogger<CourierController>> _mockLogger = new Mock<ILogger<CourierController>>();
    private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();
    private readonly Mock<INotifier> _mockNotifier = new Mock<INotifier>();
    private readonly CourierController _controller;

    public CourierControllerTests()
    {
        _controller = new CourierController(_mockService.Object, _mockLogger.Object, _mockMapper.Object, _mockNotifier.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsExpectedCouriers()
    {
        // Arrange
        var couriers = new List<Courier> { new Courier { Id = Guid.NewGuid(), FirstName = "John" } };
        var courierDtos = new List<CourierDto> { new CourierDto { Id = couriers[0].Id, FirstName = couriers[0].FirstName } };

        _mockService.Setup(service => service.GetAllAsync()).ReturnsAsync(couriers);
        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<CourierDto>>(couriers)).Returns(courierDtos);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<CourierDto>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(courierDtos, okResult.Value);
    }

    [Fact]
    public async Task GetById_ReturnsExpectedCourier()
    {
        // Arrange
        var courier = new Courier { Id = Guid.NewGuid(), FirstName = "John" };
        var courierDto = new CourierDto { Id = courier.Id, FirstName = courier.FirstName };

        _mockService.Setup(service => service.GetCourierByIdAsync(courier.Id)).ReturnsAsync(courier);
        _mockMapper.Setup(mapper => mapper.Map<CourierDto>(courier)).Returns(courierDto);

        // Act
        var result = await _controller.GetById(courier.Id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<CourierDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(courierDto, okResult.Value);
    }

    [Fact]
    public async Task Add_ReturnsExpectedCourier()
    {
        // Arrange
        var courierDto = new CourierDto { FirstName = "John" };
        var courier = new Courier { FirstName = courierDto.FirstName };

        _mockMapper.Setup(mapper => mapper.Map<Courier>(courierDto)).Returns(courier);
        _mockService.Setup(service => service.AddCourierAsync(courier)).ReturnsAsync(courier);

        // Act
        var result = await _controller.Add(courierDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<CourierDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(courierDto, okResult.Value);
    }

    [Fact]
    public async Task Update_ReturnsExpectedCourier()
    {
        // Arrange
        var courierDto = new CourierDto { Id = Guid.NewGuid(), FirstName = "John" };
        var courier = new Courier { Id = courierDto.Id, FirstName = courierDto.FirstName };

        _mockMapper.Setup(mapper => mapper.Map<Courier>(courierDto)).Returns(courier);
        _mockService.Setup(service => service.UpdateCourierAsync(courier)).ReturnsAsync(courier);

        // Act
        var result = await _controller.Update(courierDto.Id, courierDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<CourierDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(courierDto, okResult.Value);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdDoesNotMatch()
    {
        // Arrange
        var courierDto = new CourierDto { Id = Guid.NewGuid(), FirstName = "John" };

        // Act
        var result = await _controller.Update(Guid.NewGuid(), courierDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<CourierDto>>(result);
        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task Update_ReturnsExpectedCourier_WhenIdMatches()
    {
        // Arrange
        var courierDto = new CourierDto { Id = Guid.NewGuid(), FirstName = "John" };
        var courier = new Courier { Id = courierDto.Id, FirstName = courierDto.FirstName };

        _mockMapper.Setup(mapper => mapper.Map<Courier>(courierDto)).Returns(courier);
        _mockService.Setup(service => service.UpdateCourierAsync(courier)).ReturnsAsync(courier);

        // Act
        var result = await _controller.Update(courierDto.Id, courierDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<CourierDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(courierDto, okResult.Value);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdDoesNotMatch_WhenIdMatches()
    {
        // Arrange
        var courierDto = new CourierDto { Id = Guid.NewGuid(), FirstName = "John" };

        // Act
        var result = await _controller.Update(Guid.NewGuid(), courierDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<CourierDto>>(result);
        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task Delete_ReturnsExpectedCourier()
    {
        // Arrange
        var courier = new Courier { Id = Guid.NewGuid(), FirstName = "John" };
        var courierDto = new CourierDto { Id = courier.Id, FirstName = courier.FirstName };

        _mockService.Setup(service => service.DeleteCourierAsync(courier.Id)).ReturnsAsync(courier);
        _mockMapper.Setup(mapper => mapper.Map<CourierDto>(courier)).Returns(courierDto);

        // Act
        var result = await _controller.Delete(courier.Id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<CourierDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(courierDto, okResult.Value);
    }

    [Fact]
    public async Task Delete_ReturnsExpectedCourier_WhenIdMatches()
    {
        // Arrange
        var courier = new Courier { Id = Guid.NewGuid(), FirstName = "John" };
        var courierDto = new CourierDto { Id = courier.Id, FirstName = courier.FirstName };

        _mockService.Setup(service => service.DeleteCourierAsync(courier.Id)).ReturnsAsync(courier);
        _mockMapper.Setup(mapper => mapper.Map<CourierDto>(courier)).Returns(courierDto);

        // Act
        var result = await _controller.Delete(courier.Id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<CourierDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(courierDto, okResult.Value);
    }

    [Fact]
    public async Task Delete_ReturnsBadRequest_WhenIdDoesNotMatch()
    {
        // Arrange
        var courier = new Courier { Id = Guid.NewGuid(), FirstName = "John" };

        // Act
        var result = await _controller.Delete(Guid.NewGuid());

        // Assert
        var actionResult = Assert.IsType<ActionResult<CourierDto>>(result);
        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task Delete_ReturnsBadRequest_WhenIdIsNull()
    {
        // Act
        var result = await _controller.Delete(Guid.Empty);

        // Assert
        var actionResult = Assert.IsType<ActionResult<CourierDto>>(result);
        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }
}
