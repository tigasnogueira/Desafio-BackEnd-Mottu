using BikeRentalSystem.Core.Common;
using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.Core.Interfaces.Services;

public interface IRentalService
{
    Task<Rental> GetById(Guid id);
    Task<IEnumerable<Rental>> GetAll();
    Task<PaginatedResponse<Rental>> GetAllPaged(int page, int pageSize);
    Task<IEnumerable<Rental>> GetByCourierId(Guid courierId);
    Task<IEnumerable<Rental>> GetByMotorcycleId(Guid motorcycleId);
    Task<IEnumerable<Rental>> GetActiveRentals();
    Task<decimal> CalculateRentalCost(Rental rental);
    Task<bool> Add(Rental rental);
    Task<bool> Update(Rental rental);
    Task<bool> SoftDelete(Guid id);
}
