using BikeRentalSystem.Core.Common;
using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.Core.Interfaces.Repositories;

public interface IRepository<TEntity> where TEntity : EntityModel
{
    Task<TEntity> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<PaginatedResponse<TEntity>> GetAllPagedAsync(int page, int pageSize);
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<TEntity> DeleteAsync(Guid id);
    Task<int> SaveChanges();
}
