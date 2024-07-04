using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.Core.Interfaces.Repositories;

public interface IRentalRepository : IRepository<Rental>
{
    Task<IEnumerable<Rental>> GetByCourierId(Guid courierId);
    Task<IEnumerable<Rental>> GetByMotorcycleId(Guid motorcycleId);
    Task<IEnumerable<Rental>> GetActiveRentals();
    Task<decimal> CalculateRentalCost(Guid rentalId);
}
