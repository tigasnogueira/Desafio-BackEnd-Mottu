using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Infrastructure.Database;
using BikeRentalSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace BikeRentalSystem.InfrastructureTests.Repositories;

public class CourierRepositoryTests
{
    private readonly BikeRentalDbContext _context;
    private readonly Mock<ILogger<CourierRepository>> _loggerMock;
    private readonly Mock<INotifier> _notifierMock;
    private readonly CourierRepository _repository;

    public CourierRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<BikeRentalDbContext>()
            .UseInMemoryDatabase(databaseName: "BikeRentalTest")
            .Options;
        _context = new BikeRentalDbContext(options);
        _loggerMock = new Mock<ILogger<CourierRepository>>();
        _notifierMock = new Mock<INotifier>();
        _repository = new CourierRepository(_context, _loggerMock.Object, _notifierMock.Object);
    }

    [Fact]
    public async Task GetAvailableCouriersAsync_ReturnsAvailableCouriers()
    {
        // Arrange
        var courier = new Courier { IsAvailable = true };
        await _context.Couriers.AddAsync(courier);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAvailableCouriersAsync();

        // Assert
        Assert.Single(result);
        Assert.True(result.First().IsAvailable);
    }

    [Fact]
    public async Task GetUnavailableCouriersAsync_ReturnsUnavailableCouriers()
    {
        // Arrange
        var courier = new Courier { IsAvailable = false };
        await _context.Couriers.AddAsync(courier);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetUnavailableCouriersAsync();

        // Assert
        Assert.Single(result);
        Assert.False(result.First().IsAvailable);
    }

    [Fact]
    public async Task GetCouriersByFirstNameAsync_ReturnsCouriersByFirstName()
    {
        // Arrange
        var courier = new Courier { FirstName = "John" };
        await _context.Couriers.AddAsync(courier);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetCouriersByFirstNameAsync("John");

        // Assert
        Assert.Single(result);
        Assert.Equal("John", result.First().FirstName);
    }

    [Fact]
    public async Task GetCouriersByLastNameAsync_ReturnsCouriersByLastName()
    {
        // Arrange
        var courier = new Courier { LastName = "Doe" };
        await _context.Couriers.AddAsync(courier);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetCouriersByLastNameAsync("Doe");

        // Assert
        Assert.Single(result);
        Assert.Equal("Doe", result.First().LastName);
    }

    [Fact]
    public async Task GetAvailableCouriersAsync_Notifies()
    {
        // Act
        await _repository.GetAvailableCouriersAsync();

        // Assert
        _notifierMock.Verify(n => n.Handle("All available couriers were accessed"), Times.Once);
    }

    [Fact]
    public async Task GetUnavailableCouriersAsync_Notifies()
    {
        // Act
        await _repository.GetUnavailableCouriersAsync();

        // Assert
        _notifierMock.Verify(n => n.Handle("All unavailable couriers were accessed"), Times.Once);
    }

    [Fact]
    public async Task GetCouriersByFirstNameAsync_Notifies()
    {
        // Act
        await _repository.GetCouriersByFirstNameAsync("John");

        // Assert
        _notifierMock.Verify(n => n.Handle("Couriers with first name John were accessed"), Times.Once);
    }

    [Fact]
    public async Task GetCouriersByLastNameAsync_Notifies()
    {
        // Act
        await _repository.GetCouriersByLastNameAsync("Doe");

        // Assert
        _notifierMock.Verify(n => n.Handle("Couriers with last name Doe were accessed"), Times.Once);
    }

    [Fact]
    public async Task GetAvailableCouriersAsync_ThrowsException()
    {
        // Arrange
        _context.Database.EnsureDeleted();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.GetAvailableCouriersAsync());
    }

    [Fact]
    public async Task GetUnavailableCouriersAsync_ThrowsException()
    {
        // Arrange
        _context.Database.EnsureDeleted();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.GetUnavailableCouriersAsync());
    }

    [Fact]
    public async Task GetCouriersByFirstNameAsync_ThrowsException()
    {
        // Arrange
        _context.Database.EnsureDeleted();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.GetCouriersByFirstNameAsync("John"));
    }

    [Fact]
    public async Task GetCouriersByLastNameAsync_ThrowsException()
    {
        // Arrange
        _context.Database.EnsureDeleted();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.GetCouriersByLastNameAsync("Doe"));
    }

    [Fact]
    public async Task AddAsync_AddsCourier()
    {
        // Arrange
        var courier = new Courier { FirstName = "John" };

        // Act
        await _repository.AddAsync(courier);

        // Assert
        Assert.Contains(courier, _context.Couriers);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesCourier()
    {
        // Arrange
        var courier = new Courier { FirstName = "John" };
        await _context.Couriers.AddAsync(courier);
        await _context.SaveChangesAsync();

        // Act
        courier.FirstName = "Jane";
        await _repository.UpdateAsync(courier);

        // Assert
        Assert.Equal("Jane", _context.Couriers.First().FirstName);
    }

    [Fact]
    public async Task DeleteAsync_DeletesCourier()
    {
        // Arrange
        var courier = new Courier { FirstName = "John" };
        await _context.Couriers.AddAsync(courier);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(courier.Id);

        // Assert
        Assert.DoesNotContain(courier, _context.Couriers);
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
