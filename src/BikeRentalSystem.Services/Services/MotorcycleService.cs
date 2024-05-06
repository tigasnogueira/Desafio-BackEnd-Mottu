using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using Microsoft.Extensions.Logging;

namespace BikeRentalSystem.Services.Services;

public class MotorcycleService : IMotorcycleService
{
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly ILogger<MotorcycleService> _logger;

    public MotorcycleService(IMotorcycleRepository motorcycleRepository, ILogger<MotorcycleService> logger)
    {
        _motorcycleRepository = motorcycleRepository;
        _logger = logger;
    }

    public async Task<Motorcycle> GetMotorcycleByIdAsync(Guid id)
    {
        try
        {
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
            return await _motorcycleRepository.AddAsync(entity);
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
            return await _motorcycleRepository.GetMotorcyclesByMileage(mileage);
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
            return await _motorcycleRepository.GetMotorcyclesByLicensePlate(licensePlate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
