using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using Microsoft.Extensions.Logging;

namespace BikeRentalSystem.Services.Services;

public class MotorcycleService : IMotorcycleService
{
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<MotorcycleService> _logger;
    private readonly INotifier _notifier;

    public MotorcycleService(IMotorcycleRepository motorcycleRepository, IMessagePublisher messagePublisher, ILogger<MotorcycleService> logger, INotifier notifier)
    {
        _motorcycleRepository = motorcycleRepository;
        _messagePublisher = messagePublisher;
        _logger = logger;
        _notifier = notifier;
    }

    public async Task<Motorcycle> GetMotorcycleByIdAsync(Guid id)
    {
        try
        {
            _notifier.Handle($"Motorcycle with id {id} was accessed");
            return await _motorcycleRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Motorcycle>> GetAllAsync()
    {
        try
        {
            _notifier.Handle("All motorcycles were accessed");
            return await _motorcycleRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<Motorcycle> AddMotorcycleAsync(Motorcycle entity)
    {
        try
        {
            if (!await _motorcycleRepository.IsLicensePlateUniqueAsync(entity.LicensePlate))
            {
                throw new ArgumentException("License plate must be unique");
            }

            _notifier.Handle($"New motorcycle added: {entity.LicensePlate}");
            var addedMotorcycle = await _motorcycleRepository.AddAsync(entity);

            if (addedMotorcycle.Year == 2024)
            {
                _notifier.Handle($"New 2024 motorcycle registered: {addedMotorcycle.LicensePlate}");
                await _messagePublisher.PublishAsync("motorcycle_registered", $"New 2024 motorcycle registered: {addedMotorcycle.LicensePlate}");
            }

            return addedMotorcycle;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<Motorcycle> UpdateMotorcycleAsync(Motorcycle entity)
    {
        try
        {
            _notifier.Handle($"Motorcycle updated: {entity.LicensePlate}");
            return await _motorcycleRepository.UpdateAsync(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<Motorcycle> DeleteMotorcycleAsync(Guid id)
    {
        try
        {
            _notifier.Handle($"Motorcycle deleted: {id}");
            return await _motorcycleRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Motorcycle>> GetAvailableMotorcycles()
    {
        try
        {
            _notifier.Handle("Available motorcycles were accessed");
            return await _motorcycleRepository.GetAvailableMotorcycles();
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
            _notifier.Handle("Rented motorcycles were accessed");
            return await _motorcycleRepository.GetRentedMotorcycles();
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
            _notifier.Handle($"Motorcycles by brand {brand} were accessed");
            return await _motorcycleRepository.GetMotorcyclesByBrand(brand);
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
            _notifier.Handle($"Motorcycles by model {model} were accessed");
            return await _motorcycleRepository.GetMotorcyclesByModel(model);
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
            _notifier.Handle($"Motorcycles by year {year} were accessed");
            return await _motorcycleRepository.GetMotorcyclesByYear(year);
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
            _notifier.Handle($"Motorcycles by color {color} were accessed");
            return await _motorcycleRepository.GetMotorcyclesByColor(color);
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
            _notifier.Handle($"Motorcycles by engine size {engineSize} were accessed");
            return await _motorcycleRepository.GetMotorcyclesByEngineSize(engineSize);
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
            _notifier.Handle($"Motorcycles by mileage {mileage} were accessed");
            return await _motorcycleRepository.GetMotorcyclesByMileage(mileage);
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
            _notifier.Handle($"Motorcycle by license plate {licensePlate} was accessed");
            return await _motorcycleRepository.GetMotorcycleByLicensePlate(licensePlate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
