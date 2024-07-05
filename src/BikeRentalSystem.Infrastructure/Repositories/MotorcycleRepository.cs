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
}
