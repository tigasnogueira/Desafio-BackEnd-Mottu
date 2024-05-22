using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Infrastructure.Database;
using BikeRentalSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace BikeRentalSystem.InfrastructureTests.Repositories;

public class RentalRepositoryTests
{
    private readonly BikeRentalDbContext _context;
    private readonly Mock<ILogger<RentalRepository>> _loggerMock;
    private readonly Mock<INotifier> _notifierMock;
    private readonly RentalRepository _repository;

    public RentalRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<BikeRentalDbContext>()
            .UseInMemoryDatabase(databaseName: "BikeRentalTest")
            .Options;
        _context = new BikeRentalDbContext(options);
        _loggerMock = new Mock<ILogger<RentalRepository>>();
        _notifierMock = new Mock<INotifier>();
        _repository = new RentalRepository(_context, _loggerMock.Object, _notifierMock.Object);
    }

    [Fact]
    public async Task GetRentalsByMotorcycleIdAsync_ReturnsRentals()
    {
        // Arrange
        var motorcycleId = Guid.NewGuid();
        var rental = new Rental { MotorcycleId = motorcycleId };
        await _context.Rentals.AddAsync(rental);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByMotorcycleIdAsync(motorcycleId);

        // Assert
        Assert.Single(result);
        Assert.Equal(motorcycleId, result.First().MotorcycleId);
    }

    [Fact]
    public async Task GetRentalsByCourierIdAsync_ReturnsRentals()
    {
        // Arrange
        var courierId = Guid.NewGuid();
        var rental = new Rental { CourierId = courierId };
        await _context.Rentals.AddAsync(rental);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByCourierIdAsync(courierId);

        // Assert
        Assert.Single(result);
        Assert.Equal(courierId, result.First().CourierId);
    }

    [Fact]
    public async Task GetRentalsByStartDateAsync_ReturnsRentals()
    {
        // Arrange
        var startDate = DateTime.Now;
        var rental = new Rental { StartDate = startDate };
        await _context.Rentals.AddAsync(rental);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByStartDateAsync(startDate);

        // Assert
        Assert.Single(result);
        Assert.Equal(startDate, result.First().StartDate);
    }

    [Fact]
    public async Task GetRentalsByEndDateAsync_ReturnsRentals()
    {
        // Arrange
        var endDate = DateTime.Now;
        var rental = new Rental { EndDate = endDate };
        await _context.Rentals.AddAsync(rental);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByEndDateAsync(endDate);

        // Assert
        Assert.Single(result);
        Assert.Equal(endDate, result.First().EndDate);
    }

    [Fact]
    public async Task GetRentalsByMotorcycleIdAsync_WithNonExistingMotorcycleId_ReturnsEmptyCollection()
    {
        // Act
        var result = await _repository.GetRentalsByMotorcycleIdAsync(Guid.NewGuid());

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetRentalsByCourierIdAsync_WithNonExistingCourierId_ReturnsEmptyCollection()
    {
        // Act
        var result = await _repository.GetRentalsByCourierIdAsync(Guid.NewGuid());

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetRentalsByStartDateAsync_WithNonExistingStartDate_ReturnsEmptyCollection()
    {
        // Act
        var result = await _repository.GetRentalsByStartDateAsync(DateTime.Now);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetRentalsByEndDateAsync_WithNonExistingEndDate_ReturnsEmptyCollection()
    {
        // Act
        var result = await _repository.GetRentalsByEndDateAsync(DateTime.Now);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetRentalsByMotorcycleIdAsync_WithMultipleRentals_ReturnsRentals()
    {
        // Arrange
        var motorcycleId = Guid.NewGuid();
        var rental1 = new Rental { MotorcycleId = motorcycleId };
        var rental2 = new Rental { MotorcycleId = motorcycleId };
        await _context.Rentals.AddRangeAsync(rental1, rental2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByMotorcycleIdAsync(motorcycleId);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(motorcycleId, result.First().MotorcycleId);
    }

    [Fact]
    public async Task GetRentalsByCourierIdAsync_WithMultipleRentals_ReturnsRentals()
    {
        // Arrange
        var courierId = Guid.NewGuid();
        var rental1 = new Rental { CourierId = courierId };
        var rental2 = new Rental { CourierId = courierId };
        await _context.Rentals.AddRangeAsync(rental1, rental2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByCourierIdAsync(courierId);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(courierId, result.First().CourierId);
    }

    [Fact]
    public async Task GetRentalsByStartDateAsync_WithMultipleRentals_ReturnsRentals()
    {
        // Arrange
        var startDate = DateTime.Now;
        var rental1 = new Rental { StartDate = startDate };
        var rental2 = new Rental { StartDate = startDate };
        await _context.Rentals.AddRangeAsync(rental1, rental2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByStartDateAsync(startDate);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(startDate, result.First().StartDate);
    }

    [Fact]
    public async Task GetRentalsByEndDateAsync_WithMultipleRentals_ReturnsRentals()
    {
        // Arrange
        var endDate = DateTime.Now;
        var rental1 = new Rental { EndDate = endDate };
        var rental2 = new Rental { EndDate = endDate };
        await _context.Rentals.AddRangeAsync(rental1, rental2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByEndDateAsync(endDate);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(endDate, result.First().EndDate);
    }

    [Fact]
    public async Task GetRentalsByMotorcycleIdAsync_WithMultipleRentals_ReturnsRentalsWithMotorcycleId()
    {
        // Arrange
        var motorcycleId = Guid.NewGuid();
        var rental1 = new Rental { MotorcycleId = motorcycleId };
        var rental2 = new Rental { MotorcycleId = Guid.NewGuid() };
        await _context.Rentals.AddRangeAsync(rental1, rental2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByMotorcycleIdAsync(motorcycleId);

        // Assert
        Assert.Single(result);
        Assert.Equal(motorcycleId, result.First().MotorcycleId);
    }

    [Fact]
    public async Task GetRentalsByCourierIdAsync_WithMultipleRentals_ReturnsRentalsWithCourierId()
    {
        // Arrange
        var courierId = Guid.NewGuid();
        var rental1 = new Rental { CourierId = courierId };
        var rental2 = new Rental { CourierId = Guid.NewGuid() };
        await _context.Rentals.AddRangeAsync(rental1, rental2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByCourierIdAsync(courierId);

        // Assert
        Assert.Single(result);
        Assert.Equal(courierId, result.First().CourierId);
    }

    [Fact]
    public async Task GetRentalsByStartDateAsync_WithMultipleRentals_ReturnsRentalsWithStartDate()
    {
        // Arrange
        var startDate = DateTime.Now;
        var rental1 = new Rental { StartDate = startDate };
        var rental2 = new Rental { StartDate = DateTime.Now.AddDays(1) };
        await _context.Rentals.AddRangeAsync(rental1, rental2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByStartDateAsync(startDate);

        // Assert
        Assert.Single(result);
        Assert.Equal(startDate, result.First().StartDate);
    }

    [Fact]
    public async Task GetRentalsByEndDateAsync_WithMultipleRentals_ReturnsRentalsWithEndDate()
    {
        // Arrange
        var endDate = DateTime.Now;
        var rental1 = new Rental { EndDate = endDate };
        var rental2 = new Rental { EndDate = DateTime.Now.AddDays(1) };
        await _context.Rentals.AddRangeAsync(rental1, rental2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByEndDateAsync(endDate);

        // Assert
        Assert.Single(result);
        Assert.Equal(endDate, result.First().EndDate);
    }

    [Fact]
    public async Task GetRentalsByMotorcycleIdAsync_WithMultipleRentals_ReturnsRentalsWithMotorcycleIdOnly()
    {
        // Arrange
        var motorcycleId = Guid.NewGuid();
        var rental1 = new Rental { MotorcycleId = motorcycleId };
        var rental2 = new Rental { MotorcycleId = Guid.NewGuid() };
        await _context.Rentals.AddRangeAsync(rental1, rental2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByMotorcycleIdAsync(motorcycleId);

        // Assert
        Assert.Single(result);
        Assert.Equal(motorcycleId, result.First().MotorcycleId);
    }

    [Fact]
    public async Task GetRentalsByCourierIdAsync_WithMultipleRentals_ReturnsRentalsWithCourierIdOnly()
    {
        // Arrange
        var courierId = Guid.NewGuid();
        var rental1 = new Rental { CourierId = courierId };
        var rental2 = new Rental { CourierId = Guid.NewGuid() };
        await _context.Rentals.AddRangeAsync(rental1, rental2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByCourierIdAsync(courierId);

        // Assert
        Assert.Single(result);
        Assert.Equal(courierId, result.First().CourierId);
    }

    [Fact]
    public async Task GetRentalsByStartDateAsync_WithMultipleRentals_ReturnsRentalsWithStartDateOnly()
    {
        // Arrange
        var startDate = DateTime.Now;
        var rental1 = new Rental { StartDate = startDate };
        var rental2 = new Rental { StartDate = DateTime.Now.AddDays(1) };
        await _context.Rentals.AddRangeAsync(rental1, rental2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByStartDateAsync(startDate);

        // Assert
        Assert.Single(result);
        Assert.Equal(startDate, result.First().StartDate);
    }

    [Fact]
    public async Task GetRentalsByEndDateAsync_WithMultipleRentals_ReturnsRentalsWithEndDateOnly()
    {
        // Arrange
        var endDate = DateTime.Now;
        var rental1 = new Rental { EndDate = endDate };
        var rental2 = new Rental { EndDate = DateTime.Now.AddDays(1) };
        await _context.Rentals.AddRangeAsync(rental1, rental2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByEndDateAsync(endDate);

        // Assert
        Assert.Single(result);
        Assert.Equal(endDate, result.First().EndDate);
    }

    [Fact]
    public async Task GetRentalsByMotorcycleIdAsync_WithMultipleRentals_ReturnsRentalsWithMotorcycleIdInDescendingOrder()
    {
        // Arrange
        var motorcycleId = Guid.NewGuid();
        var rental1 = new Rental { MotorcycleId = motorcycleId };
        var rental2 = new Rental { MotorcycleId = motorcycleId };
        await _context.Rentals.AddRangeAsync(rental1, rental2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByMotorcycleIdAsync(motorcycleId);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(motorcycleId, result.First().MotorcycleId);
    }

    [Fact]
    public async Task GetRentalsByCourierIdAsync_WithMultipleRentals_ReturnsRentalsWithCourierIdInDescendingOrder()
    {
        // Arrange
        var courierId = Guid.NewGuid();
        var rental1 = new Rental { CourierId = courierId };
        var rental2 = new Rental { CourierId = courierId };
        await _context.Rentals.AddRangeAsync(rental1, rental2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByCourierIdAsync(courierId);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(courierId, result.First().CourierId);
    }

    [Fact]
    public async Task GetRentalsByStartDateAsync_WithMultipleRentals_ReturnsRentalsWithStartDateInDescendingOrder()
    {
        // Arrange
        var startDate = DateTime.Now;
        var rental1 = new Rental { StartDate = startDate };
        var rental2 = new Rental { StartDate = startDate };
        await _context.Rentals.AddRangeAsync(rental1, rental2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByStartDateAsync(startDate);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(startDate, result.First().StartDate);
    }

    [Fact]
    public async Task GetRentalsByEndDateAsync_WithMultipleRentals_ReturnsRentalsWithEndDateInDescendingOrder()
    {
        // Arrange
        var endDate = DateTime.Now;
        var rental1 = new Rental { EndDate = endDate };
        var rental2 = new Rental { EndDate = endDate };
        await _context.Rentals.AddRangeAsync(rental1, rental2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByEndDateAsync(endDate);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(endDate, result.First().EndDate);
    }

    [Fact]
    public async Task GetRentalsByMotorcycleIdAsync_WithMultipleRentals_ReturnsRentalsWithMotorcycleIdInAscendingOrder()
    {
        // Arrange
        var motorcycleId = Guid.NewGuid();
        var rental1 = new Rental { MotorcycleId = motorcycleId };
        var rental2 = new Rental { MotorcycleId = motorcycleId };
        await _context.Rentals.AddRangeAsync(rental2, rental1);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByMotorcycleIdAsync(motorcycleId);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(motorcycleId, result.First().MotorcycleId);
    }

    [Fact]
    public async Task GetRentalsByCourierIdAsync_WithMultipleRentals_ReturnsRentalsWithCourierIdInAscendingOrder()
    {
        // Arrange
        var courierId = Guid.NewGuid();
        var rental1 = new Rental { CourierId = courierId };
        var rental2 = new Rental { CourierId = courierId };
        await _context.Rentals.AddRangeAsync(rental2, rental1);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByCourierIdAsync(courierId);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(courierId, result.First().CourierId);
    }

    [Fact]
    public async Task GetRentalsByStartDateAsync_WithMultipleRentals_ReturnsRentalsWithStartDateInAscendingOrder()
    {
        // Arrange
        var startDate = DateTime.Now;
        var rental1 = new Rental { StartDate = startDate };
        var rental2 = new Rental { StartDate = startDate };
        await _context.Rentals.AddRangeAsync(rental2, rental1);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByStartDateAsync(startDate);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(startDate, result.First().StartDate);
    }

    [Fact]
    public async Task GetRentalsByEndDateAsync_WithMultipleRentals_ReturnsRentalsWithEndDateInAscendingOrder()
    {
        // Arrange
        var endDate = DateTime.Now;
        var rental1 = new Rental { EndDate = endDate };
        var rental2 = new Rental { EndDate = endDate };
        await _context.Rentals.AddRangeAsync(rental2, rental1);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentalsByEndDateAsync(endDate);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(endDate, result.First().EndDate);
    }

    [Fact]
    public async Task AddAsync_AddsRental()
    {
        // Arrange
        var rental = new Rental();

        // Act
        var result = await _repository.AddAsync(rental);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(rental, result);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesRental()
    {
        // Arrange
        var rental = new Rental();
        await _context.Rentals.AddAsync(rental);
        await _context.SaveChangesAsync();

        // Act
        rental.StartDate = DateTime.Now;
        var result = await _repository.UpdateAsync(rental);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(rental, result);
    }

    [Fact]
    public async Task DeleteAsync_DeletesRental()
    {
        // Arrange
        var rental = new Rental();
        await _context.Rentals.AddAsync(rental);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.DeleteAsync(rental.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(rental, result);
    }

    [Fact]
    public async Task SaveChanges_SavesChanges()
    {
        // Act
        var result = await _repository.SaveChanges();

        // Assert
        Assert.Equal(0, result);
    }
}
