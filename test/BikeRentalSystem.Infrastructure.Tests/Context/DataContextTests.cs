using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Infrastructure.Context;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalSystem.Infrastructure.Tests.Context;

public class DataContextTests
{
    private DbContextOptions<DataContext> CreateDbContextOptions()
    {
        return new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }

    [Fact]
    public void DataContext_Should_Create_Database()
    {
        // Arrange
        var options = CreateDbContextOptions();

        // Act & Assert
        using (var context = new DataContext(options))
        {
            var motorcycle = new Motorcycle
            {
                Id = Guid.NewGuid(),
                Model = "Test Model",
                CreatedByUser = "TestUser"
            };
            context.Motorcycles.Add(motorcycle);
            context.SaveChanges();
        }

        using (var context = new DataContext(options))
        {
            var motorcycleCount = context.Motorcycles.Count();
            motorcycleCount.Should().Be(1);
        }
    }

    [Fact]
    public void DataContext_Should_Have_DbSets()
    {
        // Arrange
        var options = CreateDbContextOptions();

        // Act
        using (var context = new DataContext(options))
        {
            // Assert
            context.Motorcycles.Should().NotBeNull();
            context.Couriers.Should().NotBeNull();
            context.Rentals.Should().NotBeNull();
        }
    }

    [Fact]
    public void DataContext_Should_Add_And_Retrieve_Motorcycle()
    {
        // Arrange
        var options = CreateDbContextOptions();

        var motorcycle = new Motorcycle
        {
            Id = Guid.NewGuid(),
            Model = "Test Model",
            CreatedByUser = "TestUser"
        };

        // Act
        using (var context = new DataContext(options))
        {
            context.Motorcycles.Add(motorcycle);
            context.SaveChanges();
        }

        // Assert
        using (var context = new DataContext(options))
        {
            var retrievedMotorcycle = context.Motorcycles.SingleOrDefault(m => m.Id == motorcycle.Id);
            retrievedMotorcycle.Should().NotBeNull();
            retrievedMotorcycle.Model.Should().Be("Test Model");
        }
    }

    [Fact]
    public void DataContext_Should_Not_Add_Invalid_Motorcycle()
    {
        // Arrange
        var options = CreateDbContextOptions();

        var motorcycle = new Motorcycle
        {
            Id = Guid.NewGuid(),
            Model = null,
            CreatedByUser = "TestUser"
        };

        // Act
        using (var context = new DataContext(options))
        {
            context.Motorcycles.Add(motorcycle);
            // Assert
            FluentActions.Invoking(() => context.SaveChanges())
                .Should().Throw<DbUpdateException>();
        }
    }

    [Fact]
    public void DataContext_Should_Add_And_Retrieve_Courier()
    {
        // Arrange
        var options = CreateDbContextOptions();

        var courier = new Courier
        {
            Id = Guid.NewGuid(),
            Name = "Test Courier",
            CreatedByUser = "TestUser",
            Cnpj = "12345678901234",
            BirthDate = new DateOnly(1990, 1, 1),
            CnhNumber = "1234567890",
            CnhType = "A",
            CnhImage = null
        };

        // Act
        using (var context = new DataContext(options))
        {
            context.Couriers.Add(courier);
            context.SaveChanges();
        }

        // Assert
        using (var context = new DataContext(options))
        {
            var retrievedCourier = context.Couriers.SingleOrDefault(c => c.Id == courier.Id);
            retrievedCourier.Should().NotBeNull();
            retrievedCourier.Name.Should().Be("Test Courier");
        }
    }

    [Fact]
    public void DataContext_Should_Not_Add_Invalid_Courier()
    {
        // Arrange
        var options = CreateDbContextOptions();

        var courier = new Courier
        {
            Id = Guid.NewGuid(),
            Name = null,
            CreatedByUser = "TestUser",
            Cnpj = "12345678901234",
            BirthDate = new DateOnly(1990, 1, 1),
            CnhNumber = "1234567890",
            CnhType = "A",
            CnhImage = null
        };

        // Act
        using (var context = new DataContext(options))
        {
            context.Couriers.Add(courier);
            // Assert
            FluentActions.Invoking(() => context.SaveChanges())
                .Should().Throw<DbUpdateException>();
        }
    }

    [Fact]
    public void DataContext_Should_Add_And_Retrieve_Rental()
    {
        // Arrange
        var options = CreateDbContextOptions();

        var rental = new Rental
        {
            Id = Guid.NewGuid(),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            CreatedByUser = "TestUser"
        };

        // Act
        using (var context = new DataContext(options))
        {
            context.Rentals.Add(rental);
            context.SaveChanges();
        }

        // Assert
        using (var context = new DataContext(options))
        {
            var retrievedRental = context.Rentals.SingleOrDefault(r => r.Id == rental.Id);
            retrievedRental.Should().NotBeNull();
            retrievedRental.StartDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            retrievedRental.EndDate.Should().BeCloseTo(DateTime.Now.AddDays(1), TimeSpan.FromSeconds(1));
        }
    }

    [Fact]
    public void DataContext_Should_Not_Add_Invalid_Rental()
    {
        // Arrange
        var options = CreateDbContextOptions();

        var rental = new Rental
        {
            Id = Guid.NewGuid(),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            CreatedByUser = null
        };

        // Act
        using (var context = new DataContext(options))
        {
            context.Rentals.Add(rental);
            // Assert
            FluentActions.Invoking(() => context.SaveChanges())
                .Should().Throw<DbUpdateException>();
        }
    }

}
