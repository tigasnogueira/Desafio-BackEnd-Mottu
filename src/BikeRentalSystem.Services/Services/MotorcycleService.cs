using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Models.Validations;
using Microsoft.Extensions.Logging;

namespace BikeRentalSystem.Services.Services;

public class MotorcycleService : BaseService, IMotorcycleService
{
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<MotorcycleService> _logger;
    private readonly INotifier _notifier;

    public MotorcycleService(IMotorcycleRepository motorcycleRepository, IMessagePublisher messagePublisher, ILogger<MotorcycleService> logger, INotifier notifier) : base(notifier)
    {
        _motorcycleRepository = motorcycleRepository;
        _messagePublisher = messagePublisher;
        _logger = logger;
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
            if (!ExecuteValidation(new MotorcycleValidation(_motorcycleRepository), entity)) return null;

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
            if (!ExecuteValidation(new MotorcycleValidation(_motorcycleRepository), entity)) return null;

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
            if (!ExecuteValidation(new MotorcycleValidation(_motorcycleRepository), new Motorcycle { Id = id })) return null;

            _notifier.Handle($"Motorcycle deleted: {id}");
            return await _motorcycleRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Motorcycle>> GetAvailableMotorcyclesAsync()
    {
        try
        {
            _notifier.Handle("Available motorcycles were accessed");
            return await _motorcycleRepository.GetAvailableMotorcyclesAsync();
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
            _notifier.Handle("Rented motorcycles were accessed");
            return await _motorcycleRepository.GetRentedMotorcyclesAsync();
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
            _notifier.Handle($"Motorcycles by brand {brand} were accessed");
            return await _motorcycleRepository.GetMotorcyclesByBrandAsync(brand);
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
            _notifier.Handle($"Motorcycles by model {model} were accessed");
            return await _motorcycleRepository.GetMotorcyclesByModelAsync(model);
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
            _notifier.Handle($"Motorcycles by year {year} were accessed");
            return await _motorcycleRepository.GetMotorcyclesByYearAsync(year);
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
            _notifier.Handle($"Motorcycles by color {color} were accessed");
            return await _motorcycleRepository.GetMotorcyclesByColorAsync(color);
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
            _notifier.Handle($"Motorcycles by engine size {engineSize} were accessed");
            return await _motorcycleRepository.GetMotorcyclesByEngineSizeAsync(engineSize);
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
            _notifier.Handle($"Motorcycles by mileage {mileage} were accessed");
            return await _motorcycleRepository.GetMotorcyclesByMileageAsync(mileage);
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
            _notifier.Handle($"Motorcycle by license plate {licensePlate} was accessed");
            return await _motorcycleRepository.GetMotorcycleByLicensePlateAsync(licensePlate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<Motorcycle> UpdateMotorcycleLicensePlateAsync(Guid id, string newLicensePlate)
    {
        var motorcycle = await _motorcycleRepository.GetByIdAsync(id);
        if (motorcycle == null)
            throw new KeyNotFoundException("Motorcycle not found.");

        if (!ExecuteValidation(new MotorcycleValidation(_motorcycleRepository), new Motorcycle { LicensePlate = newLicensePlate }))
            return null;

        motorcycle.LicensePlate = newLicensePlate;
        await _motorcycleRepository.UpdateAsync(motorcycle);
        return motorcycle;
    }
}
