using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Notifications;
using BikeRentalSystem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalSystem.Infrastructure.Repositories;

public class MotorcycleNotificationRepository : Repository<MotorcycleNotification>, IMotorcycleNotificationRepository
{
    public MotorcycleNotificationRepository(DataContext dataContext, INotifier notifier) : base(dataContext, notifier)
    {
    }

    public async Task<MotorcycleNotification> GetByMotorcycleId(Guid motorcycleId)
    {
        try
        {
            _notifier.Handle($"Getting {nameof(MotorcycleNotification)} by Motorcycle ID {motorcycleId}.");
            return await _dbSet.FirstOrDefaultAsync(mn => mn.MotorcycleId == motorcycleId);
        }
        catch (Exception ex)
        {
            _notifier.Handle($"Error getting {nameof(MotorcycleNotification)} by Motorcycle ID {motorcycleId}: {ex.Message}", NotificationType.Error);
            throw;
        }
    }
}
