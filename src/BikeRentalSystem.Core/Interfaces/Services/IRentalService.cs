using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.Core.Interfaces.Services;

public interface IRentalService
{
    Task<Rental> GetById(Guid id);
    Task<IEnumerable<Rental>> GetAll();
    Task<IEnumerable<Rental>> GetByCourierId(Guid courierId);
    Task<IEnumerable<Rental>> GetByMotorcycleId(Guid motorcycleId);
    Task<IEnumerable<Rental>> GetActiveRentals();
    Task<decimal> CalculateRentalCost(Guid rentalId);
    Task<bool> Add(Rental rental);
    Task<bool> Update(Rental rental);
    Task<bool> SoftDelete(Guid id);
}
