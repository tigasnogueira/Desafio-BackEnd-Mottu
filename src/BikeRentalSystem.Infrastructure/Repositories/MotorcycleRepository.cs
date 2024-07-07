using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Notifications;
using BikeRentalSystem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalSystem.Infrastructure.Repositories;

public class MotorcycleRepository(DataContext _dataContext, INotifier _notifier) : Repository<Motorcycle>(_dataContext, _notifier), IMotorcycleRepository
{
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

    public async Task<IEnumerable<Motorcycle>> GetMotorcyclesByBrandAsync(string brand)
    {
        try
        {
            _notifier.Handle($"Motorcycles with brand {brand} were accessed");
            return _motorcycles.Find(e => e.Brand == brand).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Motorcycle>> GetMotorcyclesByModelAsync(string model)
    {
        try
        {
            _notifier.Handle($"Motorcycles with model {model} were accessed");
            return _motorcycles.Find(e => e.Model == model).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Motorcycle>> GetMotorcyclesByYearAsync(int year)
    {
        try
        {
            _notifier.Handle($"Motorcycles with year {year} were accessed");
            return _motorcycles.Find(e => e.Year == year).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Motorcycle>> GetMotorcyclesByColorAsync(string color)
    {
        try
        {
            _notifier.Handle($"Motorcycles with color {color} were accessed");
            return _motorcycles.Find(e => e.Color == color).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Motorcycle>> GetMotorcyclesByEngineSizeAsync(int engineSize)
    {
        try
        {
            _notifier.Handle($"Motorcycles with engine size {engineSize} were accessed");
            return _motorcycles.Find(e => e.EngineSize == engineSize).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Motorcycle>> GetMotorcyclesByMileageAsync(int mileage)
    {
        try
        {
            _notifier.Handle($"Motorcycles with mileage {mileage} were accessed");
            return _motorcycles.Find(e => e.Mileage == mileage).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<Motorcycle> GetMotorcycleByLicensePlateAsync(string licensePlate)
    {
        try
        {
            _notifier.Handle($"Motorcycle with license plate {licensePlate} was accessed");
            return await _motorcycles.Find(e => e.LicensePlate == licensePlate).FirstOrDefaultAsync();
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
        var existingMotorcycle = await _motorcycles.Find(m => m.LicensePlate == licensePlate).FirstOrDefaultAsync();
        _notifier.Handle($"License plate {licensePlate} is {(existingMotorcycle == null ? "unique" : "not unique")}");
        return existingMotorcycle == null;
    }

    public async Task<bool> MotorcycleHasRentalsAsync(Guid motorcycleId)
    {
        var rentals = await _rentalRepository.GetRentalsByMotorcycleIdAsync(motorcycleId);
        return rentals.Any();
    }
}
