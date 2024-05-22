using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Services.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace BikeRentalSystem.InfrastructureTests.Services;

public class RentalServiceTests
{
    private readonly Mock<IRentalRepository> _mockRepo;
    private readonly Mock<IMessagePublisher> _mockPublisher;
    private readonly Mock<ILogger<RentalService>> _mockLogger;
    private readonly Mock<INotifier> _mockNotifier;
    private readonly RentalService _service;

    public RentalServiceTests()
    {
        _mockRepo = new Mock<IRentalRepository>();
        _mockPublisher = new Mock<IMessagePublisher>();
        _mockLogger = new Mock<ILogger<RentalService>>();
        _mockNotifier = new Mock<INotifier>();
        _service = new RentalService(_mockRepo.Object, _mockPublisher.Object, _mockLogger.Object, _mockNotifier.Object);
    }

    [Fact]
    public async Task GetRentalByIdAsync_ReturnsRental()
    {
        var rentalId = Guid.NewGuid();
        var rental = new Rental { Id = rentalId, StartDate = DateTime.UtcNow };

        _mockRepo.Setup(x => x.GetByIdAsync(rentalId)).ReturnsAsync(rental);

        var result = await _service.GetRentalByIdAsync(rentalId);

        Assert.NotNull(result);
        Assert.Equal(rentalId, result.Id);
        _mockNotifier.Verify(x => x.Handle(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task AddRentalAsync_ReturnsRental()
    {
        var rental = new Rental { StartDate = DateTime.UtcNow };

        _mockRepo.Setup(x => x.AddAsync(rental)).ReturnsAsync(rental);

        var result = await _service.AddRentalAsync(rental);

        Assert.NotNull(result);
        Assert.Equal(rental.StartDate, result.StartDate);
        _mockNotifier.Verify(x => x.Handle(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsRentals()
    {
        var rentals = new List<Rental>
        {
            new Rental { Id = Guid.NewGuid(), StartDate = DateTime.UtcNow },
            new Rental { Id = Guid.NewGuid(), StartDate = DateTime.UtcNow }
        };

        _mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(rentals);

        var result = await _service.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(rentals.Count, result.Count());
        _mockNotifier.Verify(x => x.Handle(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdateRentalAsync_ReturnsRental()
    {
        var rental = new Rental { Id = Guid.NewGuid(), StartDate = DateTime.UtcNow };

        _mockRepo.Setup(x => x.UpdateAsync(rental)).ReturnsAsync(rental);

        var result = await _service.UpdateRentalAsync(rental);

        Assert.NotNull(result);
        Assert.Equal(rental.Id, result.Id);
        _mockNotifier.Verify(x => x.Handle(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task DeleteRentalAsync_ReturnsRental()
    {
        var rentalId = Guid.NewGuid();
        var rental = new Rental { Id = rentalId, StartDate = DateTime.UtcNow };

        _mockRepo.Setup(x => x.DeleteAsync(rentalId)).ReturnsAsync(rental);

        var result = await _service.DeleteRentalAsync(rentalId);

        Assert.NotNull(result);
        Assert.Equal(rentalId, result.Id);
        _mockNotifier.Verify(x => x.Handle(It.IsAny<string>()), Times.Once);
    }
}
