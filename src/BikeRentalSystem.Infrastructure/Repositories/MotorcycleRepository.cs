using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BikeRentalSystem.Infrastructure.Repositories;

public class MotorcycleRepository : Repository<Motorcycle>, IMotorcycleRepository
{
    private readonly IMongoCollection<Motorcycle> _collection;
    private readonly ILogger<MotorcycleRepository> _logger;

    public MotorcycleRepository(IMongoDatabase database, ILogger<MotorcycleRepository> logger)
        : base(database, "motorcycles", logger)
    {
        _collection = database.GetCollection<Motorcycle>("motorcycles");
        _logger = logger;
    }

    public async Task<IEnumerable<Motorcycle>> GetAvailableMotorcycles()
    {
        try
        {
            return _collection.Find(e => !e.IsRented).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Motorcycle>> GetRentedMotorcycles()
    {
        try
        {
            return _collection.Find(e => e.IsRented).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Motorcycle>> GetMotorcyclesByBrand(string brand)
    {
        try
        {
            return _collection.Find(e => e.Brand == brand).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Motorcycle>> GetMotorcyclesByModel(string model)
    {
        try
        {
            return _collection.Find(e => e.Model == model).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Motorcycle>> GetMotorcyclesByYear(int year)
    {
        try
        {
            return _collection.Find(e => e.Year == year).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Motorcycle>> GetMotorcyclesByColor(string color)
    {
        try
        {
            return _collection.Find(e => e.Color == color).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Motorcycle>> GetMotorcyclesByEngineSize(int engineSize)
    {
        try
        {
            return _collection.Find(e => e.EngineSize == engineSize).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Motorcycle>> GetMotorcyclesByMileage(int mileage)
    {
        try
        {
            return _collection.Find(e => e.Mileage == mileage).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Motorcycle>> GetMotorcyclesByLicensePlate(string licensePlate)
    {
        try
        {
            return _collection.Find(e => e.LicensePlate == licensePlate).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
