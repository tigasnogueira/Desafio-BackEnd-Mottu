using Microsoft.EntityFrameworkCore.Storage;

namespace BikeRentalSystem.Core.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IMotorcycleRepository Motorcycles { get; }
    ICourierRepository Couriers { get; }
    IRentalRepository Rentals { get; }
    Task<int> SaveAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
