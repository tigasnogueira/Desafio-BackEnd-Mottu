using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BikeRentalSystem.Infrastructure.Repositories;

public class RentalRepository : Repository<Rental>, IRentalRepository
{
    private readonly IMongoCollection<Rental> _collection;
    private readonly ILogger<RentalRepository> _logger;

    public RentalRepository(IMongoDatabase database, ILogger<RentalRepository> logger)
        : base(database, "rentals", logger)
    {
        _collection = database.GetCollection<Rental>("rentals");
        _logger = logger;
    }

    public async Task<IEnumerable<Rental>> GetRentalsByMotorcycleIdAsync(Guid motorcycleId)
    {
        try
        {
            return await _collection.Find(r => r.MotorcycleId == motorcycleId).ToListAsync();
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
            return await _collection.Find(r => r.CourierId == courierId).ToListAsync();
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
            return await _collection.Find(r => r.StartDate == startDate).ToListAsync();
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
            return await _collection.Find(r => r.EndDate == endDate).ToListAsync();
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
            return await _collection.Find(r => r.StartDate >= startDate && r.EndDate <= endDate).ToListAsync();
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
            return await _collection.Find(r => r.Price == price).ToListAsync();
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
            return await _collection.Find(r => r.Price >= minPrice && r.Price <= maxPrice).ToListAsync();
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
            return await _collection.Find(r => r.IsPaid == isPaid).ToListAsync();
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
            return await _collection.Find(r => r.IsFinished == isFinished).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
