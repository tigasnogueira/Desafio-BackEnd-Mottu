using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using Microsoft.Extensions.Logging;

namespace BikeRentalSystem.Services.Services;

public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<RentalService> _logger;
    private readonly INotifier _notifier;

    public RentalService(IRentalRepository rentalRepository, IMessagePublisher messagePublisher, ILogger<RentalService> logger, INotifier notifier)
    {
        _rentalRepository = rentalRepository;
        _messagePublisher = messagePublisher;
        _logger = logger;
        _notifier = notifier;
    }

    public async Task<Rental> GetRentalByIdAsync(Guid id)
    {
        try
        {
            _notifier.Handle($"Rental with id {id} was accessed");
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
            _notifier.Handle("All rentals were accessed");
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
            _notifier.Handle("Rental was added");
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
            _notifier.Handle($"Rental with id {entity.Id} was updated");
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
            _notifier.Handle($"Rental with id {id} was deleted");
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
            _notifier.Handle($"Rentals for motorcycle with id {motorcycleId} were accessed");
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
            _notifier.Handle($"Rentals for courier with id {courierId} were accessed");
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
            _notifier.Handle($"Rentals starting from {startDate} were accessed");
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
            _notifier.Handle($"Rentals ending on {endDate} were accessed");
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
            _notifier.Handle($"Rentals in the date range from {startDate} to {endDate} were accessed");
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
            _notifier.Handle($"Rentals with price {price} were accessed");
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
            _notifier.Handle($"Rentals with price between {minPrice} and {maxPrice} were accessed");
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
            _notifier.Handle($"Rentals with paid status {isPaid} were accessed");
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
            _notifier.Handle($"Rentals with finished status {isFinished} were accessed");
            return await _rentalRepository.GetRentalsByFinishedStatusAsync(isFinished);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
