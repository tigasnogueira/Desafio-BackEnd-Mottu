using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Notifications;
using BikeRentalSystem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalSystem.Infrastructure.Repositories;

public class RentalRepository : Repository<Rental>, IRentalRepository
{
    public RentalRepository(DataContext dataContext, INotifier notifier) : base(dataContext, notifier)
    {
    }

    public async Task<IEnumerable<Rental>> GetByCourierId(Guid courierId)
    {
        try
        {
            _notifier.Handle($"Getting {nameof(Rental)} by Courier ID {courierId}.");
            return await _dbSet.Where(r => r.CourierId == courierId).ToListAsync();
        }
        catch (Exception ex)
        {
            _notifier.Handle($"Error getting {nameof(Rental)} by Courier ID {courierId}: {ex.Message}", NotificationType.Error);
            throw;
        }
    }

    public async Task<IEnumerable<Rental>> GetByMotorcycleId(Guid motorcycleId)
    {
        try
        {
            _notifier.Handle($"Getting {nameof(Rental)} by Motorcycle ID {motorcycleId}.");
            return await _dbSet.Where(r => r.MotorcycleId == motorcycleId).ToListAsync();
        }
        catch (Exception ex)
        {
            _notifier.Handle($"Error getting {nameof(Rental)} by Motorcycle ID {motorcycleId}: {ex.Message}", NotificationType.Error);
            throw;
        }
    }

    public async Task<IEnumerable<Rental>> GetActiveRentals()
    {
        try
        {
            _notifier.Handle($"Getting active {nameof(Rental)}.");
            return await _dbSet.Where(r => r.EndDate > DateOnly.FromDateTime(DateTime.UtcNow)).ToListAsync();
        }
        catch (Exception ex)
        {
            _notifier.Handle($"Error getting active {nameof(Rental)}: {ex.Message}", NotificationType.Error);
            throw;
        }
    }

    public async Task<decimal> CalculateRentalCost(Rental rental)
    {
        if (rental == null)
        {
            throw new ArgumentNullException(nameof(rental), "Rental cannot be null");
        }

        try
        {
            _notifier.Handle($"Calculating rental cost for rental with Motorcycle ID {rental.MotorcycleId} and Courier ID {rental.CourierId}.");

            var daysRented = (rental.EndDate.ToDateTime(TimeOnly.MinValue) - rental.StartDate.ToDateTime(TimeOnly.MinValue)).Days;
            var cost = daysRented * rental.DailyRate;

            if (rental.EndDate < rental.ExpectedEndDate)
            {
                var penaltyRate = rental.DailyRate * 0.20m;
                cost += penaltyRate * (rental.ExpectedEndDate.ToDateTime(TimeOnly.MinValue) - rental.EndDate.ToDateTime(TimeOnly.MinValue)).Days;
            }
            else if (rental.EndDate > rental.ExpectedEndDate)
            {
                var additionalDays = (rental.EndDate.ToDateTime(TimeOnly.MinValue) - rental.ExpectedEndDate.ToDateTime(TimeOnly.MinValue)).Days;
                cost += additionalDays * 50;
            }

            return cost;
        }
        catch (Exception ex)
        {
            _notifier.Handle($"Error calculating rental cost for rental with Motorcycle ID {rental.MotorcycleId} and Courier ID {rental.CourierId}: {ex.Message}", NotificationType.Error);
            throw;
        }
    }
}
