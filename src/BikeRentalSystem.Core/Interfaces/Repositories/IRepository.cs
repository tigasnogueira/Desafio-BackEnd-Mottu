using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.Core.Interfaces.Repositories;

public interface IRepository<T> where T : EntityModel
{
    Task<T> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(Guid id);
}
