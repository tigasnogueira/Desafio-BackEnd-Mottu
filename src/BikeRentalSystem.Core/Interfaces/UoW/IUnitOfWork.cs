using BikeRentalSystem.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace BikeRentalSystem.Core.Interfaces.UoW;

public interface IUnitOfWork : IDisposable
{
    IMotorcycleRepository Motorcycles { get; }
    ICourierRepository Couriers { get; }
    IRentalRepository Rentals { get; }
    IMotorcycleNotificationRepository MotorcycleNotifications { get; }
    Task<int> SaveAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
