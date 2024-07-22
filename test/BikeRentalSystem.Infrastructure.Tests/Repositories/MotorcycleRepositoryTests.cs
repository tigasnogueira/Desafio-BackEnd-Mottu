using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Tests.Helpers;
using BikeRentalSystem.Infrastructure.Context;
using BikeRentalSystem.Infrastructure.Repositories;
using FluentAssertions;

namespace BikeRentalSystem.Infrastructure.Tests.Repositories;

public class MotorcycleRepositoryTests : IDisposable
{
    private readonly DataContext _context;
    private readonly INotifier _notifier;
    private readonly MotorcycleRepository _repository;

    public MotorcycleRepositoryTests()
    {
        _context = DataContextMock.Create();
        _notifier = NotifierMock.Create();
        _repository = new MotorcycleRepository(_context, _notifier);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task GetByPlate_ShouldReturnMotorcycle_WhenPlateExists()
    {
        // Arrange
        var motorcycle = new Motorcycle{ Year = 2020, Model = "ModelX", Plate = "ABC1234", CreatedByUser = "TestUser" };
        await _context.Motorcycles.AddAsync(motorcycle);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByPlate("ABC1234");

        // Assert
        result.Should().NotBeNull();
        result.Plate.Should().Be("ABC1234");
    }

    [Fact]
    public async Task GetByPlate_ShouldReturnNull_WhenPlateDoesNotExist()
    {
        // Act
        var result = await _repository.GetByPlate("NonExistentPlate");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllByYear_ShouldReturnMotorcycles_WhenYearExists()
    {
        // Arrange
        var motorcycle1 = new Motorcycle{ Year = 2020, Model = "ModelX", Plate = "ABC1234", CreatedByUser = "TestUser" };
        var motorcycle2 = new Motorcycle{ Year = 2020, Model = "ModelY", Plate = "XYZ5678", CreatedByUser = "TestUser" };
        await _context.Motorcycles.AddRangeAsync(motorcycle1, motorcycle2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllByYear(2020);

        // Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(2);
    }

    [Fact]
    public async Task GetAllByYear_ShouldReturnEmptyList_WhenYearDoesNotExist()
    {
        // Act
        var result = await _repository.GetAllByYear(1990);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_ShouldReturnEntity_WhenIdExists()
    {
        // Arrange
        var motorcycle = new Motorcycle{ Year = 2020, Model = "ModelX", Plate = "ABC1234", CreatedByUser = "TestUser" };
        await _context.Motorcycles.AddAsync(motorcycle);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetById(motorcycle.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(motorcycle.Id);
    }

    [Fact]
    public async Task GetById_ShouldReturnNull_WhenIdDoesNotExist()
    {
        // Act
        var result = await _repository.GetById(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllEntities()
    {
        // Arrange
        var motorcycle1 = new Motorcycle{ Year = 2020, Model = "ModelX", Plate = "ABC1234", CreatedByUser = "TestUser" };
        var motorcycle2 = new Motorcycle{ Year = 2021, Model = "ModelY", Plate = "XYZ5678", CreatedByUser = "TestUser" };
        await _context.Motorcycles.AddRangeAsync(motorcycle1, motorcycle2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAll();

        // Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(2);
    }

    [Fact]
    public async Task Add_ShouldAddEntity()
    {
        // Arrange
        var motorcycle = new Motorcycle { Year = 2020, Model = "ModelX", Plate = "ABC1234", CreatedByUser = "TestUser" };

        // Act
        await _repository.Add(motorcycle);
        await _context.SaveChangesAsync();

        // Assert
        var result = await _context.Motorcycles.FindAsync(motorcycle.Id);
        result.Should().NotBeNull();
        result.Model.Should().Be("ModelX");
    }

    [Fact]
    public async Task Update_ShouldUpdateEntity()
    {
        // Arrange
        var motorcycle = new Motorcycle { Year = 2020, Model = "ModelX", Plate = "ABC1234", CreatedByUser = "TestUser" };
        await _context.Motorcycles.AddAsync(motorcycle);
        await _context.SaveChangesAsync();

        // Act
        motorcycle.Model = "UpdatedModel";
        await _repository.Update(motorcycle);
        await _context.SaveChangesAsync();

        // Assert
        var result = await _context.Motorcycles.FindAsync(motorcycle.Id);
        result.Should().NotBeNull();
        result.Model.Should().Be("UpdatedModel");
    }

    [Fact]
    public async Task Delete_ShouldMarkEntityAsDeleted()
    {
        // Arrange
        var motorcycle = new Motorcycle { Year = 2020, Model = "ModelX", Plate = "ABC1234", CreatedByUser = "TestUser" };
        await _context.Motorcycles.AddAsync(motorcycle);
        await _context.SaveChangesAsync();

        // Act
        _repository.Delete(motorcycle);
        await _context.SaveChangesAsync();

        // Assert
        var result = await _context.Motorcycles.FindAsync(motorcycle.Id);
        result.Should().NotBeNull();
        result.IsDeleted.Should().BeTrue();
    }
}
