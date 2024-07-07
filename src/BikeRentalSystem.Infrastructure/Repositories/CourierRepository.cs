using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Notifications;
using BikeRentalSystem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalSystem.Infrastructure.Repositories;

public class CourierRepository(DataContext _dataContext, INotifier _notifier) : Repository<Courier>(_dataContext, _notifier), ICourierRepository
{
    public async Task<Courier> GetByCnpj(string cnpj)
    {
        _couriers = database.GetCollection<Courier>("couriers");
        _logger = logger;
    }

    public async Task<IEnumerable<Courier>> GetAvailableCouriersAsync()
    {
        try
        {
            _notifier.Handle($"Getting {nameof(Courier)} by CNPJ {cnpj}.");
            return await _dbSet.FirstOrDefaultAsync(c => c.Cnpj == cnpj);
        }
        catch (Exception ex)
        {
            _notifier.Handle($"Error getting {nameof(Courier)} by CNPJ {cnpj}: {ex.Message}", NotificationType.Error);
            throw;
        }
    }

    public async Task<Courier> GetByCnhNumber(string cnhNumber)
    {
        try
        {
            _notifier.Handle($"Getting {nameof(Courier)} by CNH Number {cnhNumber}.");
            return await _dbSet.FirstOrDefaultAsync(c => c.CnhNumber == cnhNumber);
        }
        catch (Exception ex)
        {
            _notifier.Handle($"Error getting {nameof(Courier)} by CNH Number {cnhNumber}: {ex.Message}", NotificationType.Error);
            throw;
        }
    }
}
