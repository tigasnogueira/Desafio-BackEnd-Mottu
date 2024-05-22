using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Services.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace BikeRentalSystem.InfrastructureTests.Services;

public class CourierServiceTests
{
    private readonly Mock<ICourierRepository> _mockRepo;
    private readonly Mock<IMessagePublisher> _mockPublisher;
    private readonly Mock<ILogger<CourierService>> _mockLogger;
    private readonly Mock<INotifier> _mockNotifier;
    private readonly CourierService _service;

    public CourierServiceTests()
    {
        _mockRepo = new Mock<ICourierRepository>();
        _mockPublisher = new Mock<IMessagePublisher>();
        _mockLogger = new Mock<ILogger<CourierService>>();
        _mockNotifier = new Mock<INotifier>();
        _service = new CourierService(_mockRepo.Object, _mockPublisher.Object, _mockLogger.Object, _mockNotifier.Object);
    }

    [Fact]
    public async Task GetCourierByIdAsync_ReturnsCourier()
    {
        var courierId = Guid.NewGuid();
        var courier = new Courier { Id = courierId, FirstName = "John", LastName = "Doe" };

        _mockRepo.Setup(x => x.GetByIdAsync(courierId)).ReturnsAsync(courier);

        var result = await _service.GetCourierByIdAsync(courierId);

        Assert.NotNull(result);
        Assert.Equal(courierId, result.Id);
        _mockNotifier.Verify(x => x.Handle(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task AddCourierAsync_ReturnsCourier()
    {
        var courier = new Courier { FirstName = "John", LastName = "Doe" };

        _mockRepo.Setup(x => x.AddAsync(courier)).ReturnsAsync(courier);

        var result = await _service.AddCourierAsync(courier);

        Assert.NotNull(result);
        Assert.Equal(courier.FirstName, result.FirstName);
        _mockNotifier.Verify(x => x.Handle(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsCouriers()
    {
        var couriers = new List<Courier>
        {
            new Courier { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" },
            new Courier { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Doe" }
        };

        _mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(couriers);

        var result = await _service.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(couriers.Count, result.Count());
        _mockNotifier.Verify(x => x.Handle(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdateCourierAsync_ReturnsCourier()
    {
        var courier = new Courier { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" };

        _mockRepo.Setup(x => x.UpdateAsync(courier)).ReturnsAsync(courier);

        var result = await _service.UpdateCourierAsync(courier);

        Assert.NotNull(result);
        Assert.Equal(courier.Id, result.Id);
        _mockNotifier.Verify(x => x.Handle(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task DeleteCourierAsync_ReturnsCourier()
    {
        var courierId = Guid.NewGuid();
        var courier = new Courier { Id = courierId, FirstName = "John", LastName = "Doe" };

        _mockRepo.Setup(x => x.DeleteAsync(courierId)).ReturnsAsync(courier);

        var result = await _service.DeleteCourierAsync(courierId);

        Assert.NotNull(result);
        Assert.Equal(courierId, result.Id);
        _mockNotifier.Verify(x => x.Handle(It.IsAny<string>()), Times.Once);
    }
}
