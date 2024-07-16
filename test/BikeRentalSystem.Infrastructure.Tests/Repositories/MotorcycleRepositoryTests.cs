using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Notifications;
using BikeRentalSystem.Core.Tests.Helpers;
using BikeRentalSystem.Infrastructure.Context;
using BikeRentalSystem.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

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
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task GetByPlate_ShouldReturnMotorcycle_WhenPlateExists()
    {
        // Arrange
        var motorcycle = new Motorcycle(2020, "ModelX", "ABC1234");
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
        var motorcycle1 = new Motorcycle(2020, "ModelX", "ABC1234");
        var motorcycle2 = new Motorcycle(2020, "ModelY", "XYZ5678");
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
        var motorcycle = new Motorcycle(2020, "ModelX", "ABC1234");
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
        var motorcycle1 = new Motorcycle(2020, "ModelX", "ABC1234");
        var motorcycle2 = new Motorcycle(2021, "ModelY", "XYZ5678");
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
        var motorcycle = new Motorcycle(2020, "ModelX", "ABC1234");

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
        var motorcycle = new Motorcycle(2020, "ModelX", "ABC1234");
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
        var motorcycle = new Motorcycle(2020, "ModelX", "ABC1234");
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

public class MotorcycleRepository : Repository<Motorcycle>, IMotorcycleRepository
{
    public MotorcycleRepository(DataContext dataContext, INotifier notifier) : base(dataContext, notifier)
    {
    }

    public async Task<Motorcycle> GetByPlate(string plate)
    {
        try
        {
            _notifier.Handle($"Getting {nameof(Motorcycle)} by Plate {plate}.");
            return await _dbSet.FirstOrDefaultAsync(m => m.Plate == plate);
        }
        catch (Exception ex)
        {
            _notifier.Handle($"Error getting {nameof(Motorcycle)} by Plate {plate}: {ex.Message}", NotificationType.Error);
            throw;
        }
    }

    public async Task<IEnumerable<Motorcycle>> GetAllByYear(int year)
    {
        try
        {
            _notifier.Handle($"Getting all {nameof(Motorcycle)} by Year {year}.");
            return await _dbSet.Where(m => m.Year == year).ToListAsync();
        }
        catch (Exception ex)
        {
            _notifier.Handle($"Error getting all {nameof(Motorcycle)} by Year {year}: {ex.Message}", NotificationType.Error);
            throw;
        }
    }
}
