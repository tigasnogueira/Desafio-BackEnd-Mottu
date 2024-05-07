using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using Microsoft.Extensions.Logging;

namespace BikeRentalSystem.Services.Services;

public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;
    private readonly ILogger<RentalService> _logger;

    public RentalService(IRentalRepository rentalRepository, ILogger<RentalService> logger)
    {
        _rentalRepository = rentalRepository;
        _logger = logger;
    }

    public async Task<Rental> GetRentalByIdAsync(Guid id)
    {
        try
        {
            return await _rentalRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Rental>> GetAllAsync()
    {
        try
        {
            return await _rentalRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<Rental> AddRentalAsync(Rental entity)
    {
        try
        {
            return await _rentalRepository.AddAsync(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<Rental> UpdateRentalAsync(Rental entity)
    {
        try
        {
            return await _rentalRepository.UpdateAsync(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<Rental> DeleteRentalAsync(Guid id)
    {
        try
        {
            return await _rentalRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Rental>> GetRentalsByMotorcycleIdAsync(Guid motorcycleId)
    {
        try
        {
            return await _rentalRepository.GetRentalsByMotorcycleIdAsync(motorcycleId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Rental>> GetRentalsByCourierIdAsync(Guid courierId)
    {
        try
        {
            return await _rentalRepository.GetRentalsByCourierIdAsync(courierId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Rental>> GetRentalsByStartDateAsync(DateTime startDate)
    {
        try
        {
            return await _rentalRepository.GetRentalsByStartDateAsync(startDate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Rental>> GetRentalsByEndDateAsync(DateTime endDate)
    {
        try
        {
            return await _rentalRepository.GetRentalsByEndDateAsync(endDate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Rental>> GetRentalsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            return await _rentalRepository.GetRentalsByDateRangeAsync(startDate, endDate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Rental>> GetRentalsByPriceAsync(decimal price)
    {
        try
        {
            return await _rentalRepository.GetRentalsByPriceAsync(price);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Rental>> GetRentalsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        try
        {
            return await _rentalRepository.GetRentalsByPriceRangeAsync(minPrice, maxPrice);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Rental>> GetRentalsByPaidStatusAsync(bool isPaid)
    {
        try
        {
            return await _rentalRepository.GetRentalsByPaidStatusAsync(isPaid);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Rental>> GetRentalsByFinishedStatusAsync(bool isFinished)
    {
        try
        {
            return await _rentalRepository.GetRentalsByFinishedStatusAsync(isFinished);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
