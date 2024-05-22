using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Infrastructure.Database;
using BikeRentalSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace BikeRentalSystem.InfrastructureTests.Repositories;

public class MotorcycleRepositoryTests
{
    private readonly BikeRentalDbContext _context;
    private readonly Mock<IRentalRepository> _rentalRepositoryMock;
    private readonly Mock<ILogger<MotorcycleRepository>> _loggerMock;
    private readonly Mock<INotifier> _notifierMock;
    private readonly MotorcycleRepository _repository;

    public MotorcycleRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<BikeRentalDbContext>()
            .UseInMemoryDatabase(databaseName: "BikeRentalTest")
            .Options;
        _context = new BikeRentalDbContext(options);
        _rentalRepositoryMock = new Mock<IRentalRepository>();
        _loggerMock = new Mock<ILogger<MotorcycleRepository>>();
        _notifierMock = new Mock<INotifier>();
        _repository = new MotorcycleRepository(_context, _rentalRepositoryMock.Object, _loggerMock.Object, _notifierMock.Object);
    }

    [Fact]
    public async Task GetAvailableMotorcyclesAsync_ReturnsAvailableMotorcycles()
    {
        // Arrange
        var motorcycle = new Motorcycle { IsRented = false };
        await _context.Motorcycles.AddAsync(motorcycle);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAvailableMotorcyclesAsync();

        // Assert
        Assert.Single(result);
        Assert.False(result.First().IsRented);
    }

    [Fact]
    public async Task GetRentedMotorcyclesAsync_ReturnsRentedMotorcycles()
    {
        // Arrange
        var motorcycle = new Motorcycle { IsRented = true };
        await _context.Motorcycles.AddAsync(motorcycle);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRentedMotorcyclesAsync();

        // Assert
        Assert.Single(result);
        Assert.True(result.First().IsRented);
    }

    [Fact]
    public async Task GetMotorcyclesByBrandAsync_ReturnsMotorcyclesByBrand()
    {
        // Arrange
        var motorcycle = new Motorcycle { Brand = "Yamaha" };
        await _context.Motorcycles.AddAsync(motorcycle);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetMotorcyclesByBrandAsync("Yamaha");

        // Assert
        Assert.Single(result);
        Assert.Equal("Yamaha", result.First().Brand);
    }

    [Fact]
    public async Task GetMotorcyclesByModelAsync_ReturnsMotorcyclesByModel()
    {
        // Arrange
        var motorcycle = new Motorcycle { Model = "MT-07" };
        await _context.Motorcycles.AddAsync(motorcycle);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetMotorcyclesByModelAsync("MT-07");

        // Assert
        Assert.Single(result);
        Assert.Equal("MT-07", result.First().Model);
    }

    [Fact]
    public async Task GetMotorcyclesByModelAsync_ReturnsEmptyCollection()
    {
        // Arrange
        var motorcycle = new Motorcycle { Model = "MT-07" };
        await _context.Motorcycles.AddAsync(motorcycle);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetMotorcyclesByModelAsync("MT-09");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetMotorcyclesByBrandAsync_ReturnsEmptyCollection()
    {
        // Arrange
        var motorcycle = new Motorcycle { Brand = "Yamaha" };
        await _context.Motorcycles.AddAsync(motorcycle);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetMotorcyclesByBrandAsync("Kawasaki");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetMotorcyclesByYearAsync_ReturnsMotorcyclesByYear()
    {
        // Arrange
        var motorcycle = new Motorcycle { Year = 2021 };
        await _context.Motorcycles.AddAsync(motorcycle);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetMotorcyclesByYearAsync(2021);

        // Assert
        Assert.Single(result);
        Assert.Equal(2021, result.First().Year);
    }

    [Fact]
    public async Task GetMotorcyclesByYearAsync_ReturnsEmptyCollection()
    {
        // Arrange
        var motorcycle = new Motorcycle { Year = 2021 };
        await _context.Motorcycles.AddAsync(motorcycle);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetMotorcyclesByYearAsync(2020);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetMotorcyclesByColorAsync_ReturnsMotorcyclesByColor()
    {
        // Arrange
        var motorcycle = new Motorcycle { Color = "Red" };
        await _context.Motorcycles.AddAsync(motorcycle);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetMotorcyclesByColorAsync("Red");

        // Assert
        Assert.Single(result);
        Assert.Equal("Red", result.First().Color);
    }

    [Fact]
    public async Task GetMotorcyclesByColorAsync_ReturnsEmptyCollection()
    {
        // Arrange
        var motorcycle = new Motorcycle { Color = "Red" };
        await _context.Motorcycles.AddAsync(motorcycle);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetMotorcyclesByColorAsync("Blue");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task AddAsync_AddsMotorcycle()
    {
        // Arrange
        var motorcycle = new Motorcycle { Brand = "Yamaha" };

        // Act
        var result = await _repository.AddAsync(motorcycle);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Yamaha", result.Brand);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesMotorcycle()
    {
        // Arrange
        var motorcycle = new Motorcycle { Brand = "Yamaha" };
        await _context.Motorcycles.AddAsync(motorcycle);
        await _context.SaveChangesAsync();

        // Act
        motorcycle.Brand = "Kawasaki";
        var result = await _repository.UpdateAsync(motorcycle);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Kawasaki", result.Brand);
    }

    [Fact]
    public async Task DeleteAsync_DeletesMotorcycle()
    {
        // Arrange
        var motorcycle = new Motorcycle { Brand = "Yamaha" };
        await _context.Motorcycles.AddAsync(motorcycle);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.DeleteAsync(motorcycle.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Yamaha", result.Brand);
    }

    [Fact]
    public async Task SaveChanges_SavesChanges()
    {
        // Act
        var result = await _repository.SaveChanges();

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task SaveChanges_ThrowsException()
    {
        // Arrange
        _context.Database.EnsureDeleted();

        // Act
        async Task Act() => await _repository.SaveChanges();

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Act);
    }

    [Fact]
    public async Task GetMotorcyclesByBrandAsync_ThrowsException()
    {
        // Arrange
        _context.Database.EnsureDeleted();

        // Act
        async Task Act() => await _repository.GetMotorcyclesByBrandAsync("Yamaha");

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Act);
    }

    [Fact]
    public async Task GetMotorcyclesByModelAsync_ThrowsException()
    {
        // Arrange
        _context.Database.EnsureDeleted();

        // Act
        async Task Act() => await _repository.GetMotorcyclesByModelAsync("MT-07");

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Act);
    }

    [Fact]
    public async Task GetMotorcyclesByYearAsync_ThrowsException()
    {
        // Arrange
        _context.Database.EnsureDeleted();

        // Act
        async Task Act() => await _repository.GetMotorcyclesByYearAsync(2021);

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Act);
    }

    [Fact]
    public async Task GetMotorcyclesByColorAsync_ThrowsException()
    {
        // Arrange
        _context.Database.EnsureDeleted();

        // Act
        async Task Act() => await _repository.GetMotorcyclesByColorAsync("Red");

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Act);
    }

    [Fact]
    public async Task GetAvailableMotorcyclesAsync_ThrowsException()
    {
        // Arrange
        _context.Database.EnsureDeleted();

        // Act
        async Task Act() => await _repository.GetAvailableMotorcyclesAsync();

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Act);
    }

    [Fact]
    public async Task GetRentedMotorcyclesAsync_ThrowsException()
    {
        // Arrange
        _context.Database.EnsureDeleted();

        // Act
        async Task Act() => await _repository.GetRentedMotorcyclesAsync();

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Act);
    }

    [Fact]
    public async Task AddAsync_ThrowsException()
    {
        // Arrange
        _context.Database.EnsureDeleted();

        // Act
        async Task Act() => await _repository.AddAsync(new Motorcycle());

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Act);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsException()
    {
        // Arrange
        _context.Database.EnsureDeleted();

        // Act
        async Task Act() => await _repository.UpdateAsync(new Motorcycle());

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Act);
    }

    [Fact]
    public async Task DeleteAsync_ThrowsException()
    {
        // Arrange
        _context.Database.EnsureDeleted();

        // Act
        async Task Act() => await _repository.DeleteAsync(Guid.NewGuid());

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Act);
    }
}

