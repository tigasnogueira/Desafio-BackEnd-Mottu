using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Services.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace BikeRentalSystem.InfrastructureTests.Services;

public class MotorcycleServiceTests
{
    private readonly Mock<IMotorcycleRepository> _mockRepo;
    private readonly Mock<IMessagePublisher> _mockPublisher;
    private readonly Mock<ILogger<MotorcycleService>> _mockLogger;
    private readonly Mock<INotifier> _mockNotifier;
    private readonly MotorcycleService _service;

    public MotorcycleServiceTests()
    {
        _mockRepo = new Mock<IMotorcycleRepository>();
        _mockPublisher = new Mock<IMessagePublisher>();
        _mockLogger = new Mock<ILogger<MotorcycleService>>();
        _mockNotifier = new Mock<INotifier>();
        _service = new MotorcycleService(_mockRepo.Object, _mockPublisher.Object, _mockLogger.Object, _mockNotifier.Object);
    }

    [Fact]
    public async Task GetMotorcycleByIdAsync_ReturnsMotorcycle()
    {
        var motorcycleId = Guid.NewGuid();
        var motorcycle = new Motorcycle { Id = motorcycleId, LicensePlate = "123ABC" };

        _mockRepo.Setup(x => x.GetByIdAsync(motorcycleId)).ReturnsAsync(motorcycle);

        var result = await _service.GetMotorcycleByIdAsync(motorcycleId);

        Assert.NotNull(result);
        Assert.Equal(motorcycleId, result.Id);
        _mockNotifier.Verify(x => x.Handle(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task AddMotorcycleAsync_ReturnsMotorcycle()
    {
        var motorcycle = new Motorcycle { LicensePlate = "123ABC" };

        _mockRepo.Setup(x => x.AddAsync(motorcycle)).ReturnsAsync(motorcycle);

        var result = await _service.AddMotorcycleAsync(motorcycle);

        Assert.NotNull(result);
        Assert.Equal(motorcycle.LicensePlate, result.LicensePlate);
        _mockNotifier.Verify(x => x.Handle(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMotorcycles()
    {
        var motorcycles = new List<Motorcycle>
        {
            new Motorcycle { Id = Guid.NewGuid(), LicensePlate = "123ABC" },
            new Motorcycle { Id = Guid.NewGuid(), LicensePlate = "456DEF" }
        };

        _mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(motorcycles);

        var result = await _service.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(motorcycles.Count, result.Count());
        _mockNotifier.Verify(x => x.Handle(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task AddMotorcycleAsync_2024Motorcycle_ReturnsMotorcycle()
    {
        var motorcycle = new Motorcycle { LicensePlate = "123ABC", Year = 2024 };

        _mockRepo.Setup(x => x.AddAsync(motorcycle)).ReturnsAsync(motorcycle);

        var result = await _service.AddMotorcycleAsync(motorcycle);

        Assert.NotNull(result);
        Assert.Equal(motorcycle.LicensePlate, result.LicensePlate);
        _mockNotifier.Verify(x => x.Handle(It.IsAny<string>()), Times.Exactly(2));
        _mockPublisher.Verify(x => x.PublishAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdateMotorcycleAsync_ReturnsMotorcycle()
    {
        var motorcycle = new Motorcycle { Id = Guid.NewGuid(), LicensePlate = "123ABC" };

        _mockRepo.Setup(x => x.UpdateAsync(motorcycle)).ReturnsAsync(motorcycle);

        var result = await _service.UpdateMotorcycleAsync(motorcycle);

        Assert.NotNull(result);
        Assert.Equal(motorcycle.LicensePlate, result.LicensePlate);
        _mockNotifier.Verify(x => x.Handle(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task DeleteMotorcycleAsync_ReturnsMotorcycle()
    {
        var motorcycleId = Guid.NewGuid();
        var motorcycle = new Motorcycle { Id = motorcycleId, LicensePlate = "123ABC" };

        _mockRepo.Setup(x => x.DeleteAsync(motorcycleId)).ReturnsAsync(motorcycle);

        var result = await _service.DeleteMotorcycleAsync(motorcycleId);

        Assert.NotNull(result);
        Assert.Equal(motorcycleId, result.Id);
        _mockNotifier.Verify(x => x.Handle(It.IsAny<string>()), Times.Once);
    }
}
