using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Tests.Helpers;
using BikeRentalSystem.Infrastructure.Context;
using BikeRentalSystem.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalSystem.Infrastructure.Tests.Repositories;

public class RentalRepositoryTests : IDisposable
{
    private readonly DataContext _dataContext;
    private readonly INotifier _notifier;
    private readonly RentalRepository _repository;

    public RentalRepositoryTests()
    {
        _dataContext = DataContextMock.Create();
        _notifier = NotifierMock.Create();
        _repository = new RentalRepository(_dataContext, _notifier);
    }

    public void Dispose()
    {
        _dataContext.Database.EnsureDeleted();
        _dataContext.Dispose();
    }

    [Fact]
    public async Task GetByCourierId_ShouldReturnRentals_WhenCourierIdExists()
    {
        // Arrange
        var courier = new Courier
        {
            Name = "Test Courier",
            Cnpj = "12345678901234",
            BirthDate = new DateOnly(1990, 1, 1),
            CnhNumber = "CNH12345",
            CnhType = "A"
        };
        await _dataContext.Couriers.AddAsync(courier);
        await _dataContext.SaveChangesAsync();

        var courierId = courier.Id;

        var rental1 = new Rental
        {
            CourierId = courierId,
            MotorcycleId = Guid.NewGuid(),
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(7),
            ExpectedEndDate = DateTime.UtcNow.AddDays(7),
            DailyRate = 30
        };
        var rental2 = new Rental
        {
            CourierId = courierId,
            MotorcycleId = Guid.NewGuid(),
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(7),
            ExpectedEndDate = DateTime.UtcNow.AddDays(7),
            DailyRate = 30
        };
        await _dataContext.Rentals.AddRangeAsync(rental1, rental2);
        await _dataContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetByCourierId(courierId);

        // Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(2, because: "we have just added two rentals for this courier");
    }

    [Fact]
    public async Task GetByCourierId_ShouldReturnEmptyList_WhenCourierIdDoesNotExist()
    {
        // Act
        var result = await _repository.GetByCourierId(Guid.NewGuid());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByMotorcycleId_ShouldReturnRentals_WhenMotorcycleIdExists()
    {
        // Arrange
        var motorcycleId = Guid.NewGuid();
        var rental1 = new Rental { CourierId = Guid.NewGuid(), MotorcycleId = motorcycleId, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(7), DailyRate = 30 };
        var rental2 = new Rental { CourierId = Guid.NewGuid(), MotorcycleId = motorcycleId, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(7), DailyRate = 30 };
        await _dataContext.Rentals.AddRangeAsync(rental1, rental2);
        await _dataContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetByMotorcycleId(motorcycleId);

        // Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(2);
    }

    [Fact]
    public async Task GetByMotorcycleId_ShouldReturnEmptyList_WhenMotorcycleIdDoesNotExist()
    {
        // Act
        var result = await _repository.GetByMotorcycleId(Guid.NewGuid());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetActiveRentals_ShouldReturnActiveRentals()
    {
        // Arrange
        var rental1 = new Rental { CourierId = Guid.NewGuid(), MotorcycleId = Guid.NewGuid(), StartDate = DateTime.UtcNow.AddDays(-2), EndDate = DateTime.UtcNow.AddDays(5), ExpectedEndDate = DateTime.UtcNow.AddDays(5), DailyRate = 30 };
        var rental2 = new Rental { CourierId = Guid.NewGuid(), MotorcycleId = Guid.NewGuid(), StartDate = DateTime.UtcNow.AddDays(-1), EndDate = DateTime.UtcNow.AddDays(6), ExpectedEndDate = DateTime.UtcNow.AddDays(6), DailyRate = 30 };
        await _dataContext.Rentals.AddRangeAsync(rental1, rental2);
        await _dataContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetActiveRentals();

        // Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(2);
    }

    [Fact]
    public async Task GetActiveRentals_ShouldReturnEmptyList_WhenNoActiveRentals()
    {
        // Act
        var result = await _repository.GetActiveRentals();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task CalculateRentalCost_ShouldReturnCorrectCost_ForOnTimeReturn()
    {
        // Arrange
        var rental = new Rental
        {
            CourierId = Guid.NewGuid(),
            MotorcycleId = Guid.NewGuid(),
            StartDate = DateTime.UtcNow.AddDays(-7),
            EndDate = DateTime.UtcNow,
            ExpectedEndDate = DateTime.UtcNow,
            DailyRate = 30
        };
        await _dataContext.Rentals.AddAsync(rental);
        await _dataContext.SaveChangesAsync();

        // Act
        var result = await _repository.CalculateRentalCost(rental);

        // Assert
        result.Should().Be(210); // 7 days * 30 = 210
    }

    [Fact]
    public async Task CalculateRentalCost_ShouldReturnCorrectCost_ForEarlyReturn()
    {
        // Arrange
        var startDate = DateTime.UtcNow.AddDays(-7);
        var endDate = DateTime.UtcNow.AddDays(-1);
        var expectedEndDate = DateTime.UtcNow;
        var dailyRate = 30m;

        var rental = new Rental
        {
            CourierId = Guid.NewGuid(),
            MotorcycleId = Guid.NewGuid(),
            StartDate = startDate,
            EndDate = endDate,
            ExpectedEndDate = expectedEndDate,
            DailyRate = dailyRate
        };

        await _dataContext.Rentals.AddAsync(rental);
        await _dataContext.SaveChangesAsync();

        // Act
        var result = await _repository.CalculateRentalCost(rental);

        // Assert
        var expectedCost = (endDate - startDate).Days * dailyRate;
        var penalty = (expectedEndDate - endDate).Days * dailyRate * 0.20m;
        var totalCost = expectedCost + penalty;

        result.Should().Be(totalCost);
    }

    [Fact]
    public async Task CalculateRentalCost_ShouldReturnCorrectCost_ForLateReturn()
    {
        // Arrange
        var startDate = DateTime.UtcNow.AddDays(-8);
        var endDate = DateTime.UtcNow;
        var expectedEndDate = DateTime.UtcNow.AddDays(-1);
        var dailyRate = 30m;

        var rental = new Rental
        {
            CourierId = Guid.NewGuid(),
            MotorcycleId = Guid.NewGuid(),
            StartDate = startDate,
            EndDate = endDate,
            ExpectedEndDate = expectedEndDate,
            DailyRate = dailyRate
        };

        await _dataContext.Rentals.AddAsync(rental);
        await _dataContext.SaveChangesAsync();

        // Act
        var result = await _repository.CalculateRentalCost(rental);

        // Assert
        var expectedCost = (endDate - startDate).Days * dailyRate;
        var additionalCost = (endDate - expectedEndDate).Days * 50;
        var totalCost = expectedCost + additionalCost;

        result.Should().Be(totalCost);
    }

    [Fact]
    public async Task CalculateRentalCost_ShouldThrowException_WhenRentalIsNull()
    {
        // Arrange
        Rental rental = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _repository.CalculateRentalCost(rental));
    }
}
