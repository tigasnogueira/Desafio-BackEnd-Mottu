using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Notifications;
using BikeRentalSystem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalSystem.Infrastructure.Repositories;

public class CourierRepository : Repository<Courier>, ICourierRepository
{
    public CourierRepository(DataContext dataContext, INotifier notifier) : base(dataContext, notifier)
    {
    }

    public async Task<Courier> GetByCnpj(string cnpj)
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
