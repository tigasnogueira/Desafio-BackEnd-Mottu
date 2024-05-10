using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BikeRentalSystem.Infrastructure.Repositories;

public class MotorcycleRepository : Repository<Motorcycle>, IMotorcycleRepository
{
    private readonly IRentalRepository _rentalRepository;
    private readonly ILogger<MotorcycleRepository> _logger;
    private readonly INotifier _notifier;

    public MotorcycleRepository(BikeRentalDbContext context, IRentalRepository rentalRepository, ILogger<MotorcycleRepository> logger, INotifier notifier)
        : base(context, logger, notifier)
    {
        _rentalRepository = rentalRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Motorcycle>> GetAvailableMotorcyclesAsync()
    {
        try
        {
            _notifier.Handle("All available motorcycles were accessed");
            return await _context.Motorcycles.AsNoTracking().Where(m => !m.IsRented).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Motorcycle>> GetRentedMotorcyclesAsync()
    {
        try
        {
            _notifier.Handle("All rented motorcycles were accessed");
            return await _context.Motorcycles.AsNoTracking().Where(m => m.IsRented).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Motorcycle>> GetMotorcyclesByBrandAsync(string brand)
    {
        try
        {
            _notifier.Handle($"Motorcycles with brand {brand} were accessed");
            return await _context.Motorcycles.AsNoTracking().Where(m => m.Brand == brand).ToListAsync();
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
            return await _context.Motorcycles.AsNoTracking().Where(m => m.Model == model).ToListAsync();
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
            return await _context.Motorcycles.AsNoTracking().Where(m => m.Year == year).ToListAsync();
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
            return await _context.Motorcycles.AsNoTracking().Where(m => m.Color == color).ToListAsync();
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
            return await _context.Motorcycles.AsNoTracking().Where(m => m.EngineSize == engineSize).ToListAsync();
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
            return await _context.Motorcycles.AsNoTracking().Where(m => m.Mileage == mileage).ToListAsync();
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
            return await _context.Motorcycles.AsNoTracking().FirstOrDefaultAsync(m => m.LicensePlate == licensePlate);
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
        var existingMotorcycle = await _context.Motorcycles.AsNoTracking().FirstOrDefaultAsync(m => m.LicensePlate == licensePlate);
        _notifier.Handle($"License plate {licensePlate} is {(existingMotorcycle == null ? "unique" : "not unique")}");
        return existingMotorcycle == null;
    }

    public async Task<bool> MotorcycleHasRentalsAsync(Guid motorcycleId)
    {
        var rentals = await _rentalRepository.GetRentalsByMotorcycleIdAsync(motorcycleId);
        return rentals.Any();
    }
}
