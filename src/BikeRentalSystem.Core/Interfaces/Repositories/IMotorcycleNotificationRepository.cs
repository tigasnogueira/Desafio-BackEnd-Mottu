using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.Core.Interfaces.Repositories;

public interface IMotorcycleNotificationRepository : IRepository<MotorcycleNotification>
{
    Task<MotorcycleNotification?> GetByMotorcycleId(Guid motorcycleId);
}
