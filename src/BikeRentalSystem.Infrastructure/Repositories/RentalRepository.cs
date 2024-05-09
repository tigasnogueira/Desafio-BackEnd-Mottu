using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Infrastructure.Database;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BikeRentalSystem.Infrastructure.Repositories;

public class RentalRepository : Repository<Rental>, IRentalRepository
{
    private readonly IMongoCollection<Rental> _rentals;
    private readonly ILogger<RentalRepository> _logger;
    private readonly INotifier _notifier;

    public RentalRepository(MongoDBContext database, ILogger<RentalRepository> logger, INotifier notifier)
        : base(database, "rentals", logger, notifier)
    {
        _rentals = database.GetCollection<Rental>("rentals");
        _logger = logger;
    }

    public async Task<IEnumerable<Rental>> GetRentalsByMotorcycleIdAsync(Guid motorcycleId)
    {
        try
        {
            _notifier.Handle($"Rentals with motorcycle id {motorcycleId} were accessed");
            return await _rentals.Find(r => r.MotorcycleId == motorcycleId).ToListAsync();
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
            _notifier.Handle($"Rentals with courier id {courierId} were accessed");
            return await _rentals.Find(r => r.CourierId == courierId).ToListAsync();
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
            _notifier.Handle($"Rentals with start date {startDate} were accessed");
            return await _rentals.Find(r => r.StartDate == startDate).ToListAsync();
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
            _notifier.Handle($"Rentals with end date {endDate} were accessed");
            return await _rentals.Find(r => r.EndDate == endDate).ToListAsync();
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
            return await _rentals.Find(r => r.StartDate >= startDate && r.EndDate <= endDate).ToListAsync();
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
            return await _rentals.Find(r => r.Price == price).ToListAsync();
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
            return await _rentals.Find(r => r.Price >= minPrice && r.Price <= maxPrice).ToListAsync();
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
            return await _rentals.Find(r => r.IsPaid == isPaid).ToListAsync();
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
            return await _rentals.Find(r => r.IsFinished == isFinished).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
