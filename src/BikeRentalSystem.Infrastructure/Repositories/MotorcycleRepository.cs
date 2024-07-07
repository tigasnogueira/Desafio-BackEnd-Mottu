using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BikeRentalSystem.Infrastructure.Repositories;

public class MotorcycleRepository : Repository<Motorcycle>, IMotorcycleRepository
{
    private readonly IMongoCollection<Motorcycle> _collection;
    private readonly ILogger<MotorcycleRepository> _logger;
    private readonly INotifier _notifier;

    public MotorcycleRepository(IMongoDatabase database, ILogger<MotorcycleRepository> logger, INotifier notifier)
        : base(database, "motorcycles", logger, notifier)
    {
        _collection = database.GetCollection<Motorcycle>("motorcycles");
        _logger = logger;
    }

    public async Task<IEnumerable<Motorcycle>> GetAvailableMotorcycles()
    {
        try
        {
            _notifier.Handle("All available motorcycles were accessed");
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
            _notifier.Handle("All rented motorcycles were accessed");
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
            _notifier.Handle($"Motorcycles with brand {brand} were accessed");
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
            _notifier.Handle($"Motorcycles with model {model} were accessed");
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
            _notifier.Handle($"Motorcycles with year {year} were accessed");
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
            _notifier.Handle($"Motorcycles with color {color} were accessed");
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
            _notifier.Handle($"Motorcycles with engine size {engineSize} were accessed");
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
            _notifier.Handle($"Motorcycles with mileage {mileage} were accessed");
            return _collection.Find(e => e.Mileage == mileage).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<Motorcycle> GetMotorcycleByLicensePlate(string licensePlate)
    {
        try
        {
            _notifier.Handle($"Motorcycle with license plate {licensePlate} was accessed");
            return await _collection.Find(e => e.LicensePlate == licensePlate).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<bool> IsLicensePlateUniqueAsync(string licensePlate)
    {
        _notifier.Handle($"Checking if license plate {licensePlate} is unique");
        var existingMotorcycle = await _collection.Find(m => m.LicensePlate == licensePlate).FirstOrDefaultAsync();
        _notifier.Handle($"License plate {licensePlate} is {(existingMotorcycle == null ? "unique" : "not unique")}");
        return existingMotorcycle == null;
    }
}
