using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.Core.Interfaces.Services;

public interface IRentalService
{
    Task<Rental> GetRentalByIdAsync(Guid id);
    Task<IEnumerable<Rental>> GetAllAsync();
    Task<Rental> AddRentalAsync(Rental entity);
    Task<Rental> UpdateRentalAsync(Rental entity);
    Task<Rental> DeleteRentalAsync(Guid id);
    Task<IEnumerable<Rental>> GetRentalsByMotorcycleIdAsync(Guid motorcycleId);
    Task<IEnumerable<Rental>> GetRentalsByCourierIdAsync(Guid courierId);
    Task<IEnumerable<Rental>> GetRentalsByStartDateAsync(DateTime startDate);
    Task<IEnumerable<Rental>> GetRentalsByEndDateAsync(DateTime endDate);
    Task<IEnumerable<Rental>> GetRentalsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Rental>> GetRentalsByPriceAsync(decimal price);
    Task<IEnumerable<Rental>> GetRentalsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    Task<IEnumerable<Rental>> GetRentalsByPaidStatusAsync(bool isPaid);
    Task<IEnumerable<Rental>> GetRentalsByFinishedStatusAsync(bool isFinished);
}
