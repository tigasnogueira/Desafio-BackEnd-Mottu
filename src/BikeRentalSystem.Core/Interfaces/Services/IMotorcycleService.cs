using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.Core.Interfaces.Services;

public interface IMotorcycleService
{
    Task<Motorcycle> GetMotorcycleByIdAsync(Guid id);
    Task<IEnumerable<Motorcycle>> GetAllAsync();
    Task<Motorcycle> AddMotorcycleAsync(Motorcycle entity);
    Task<Motorcycle> UpdateMotorcycleAsync(Motorcycle entity);
    Task<Motorcycle> DeleteMotorcycleAsync(Guid id);
    Task<IEnumerable<Motorcycle>> GetAvailableMotorcycles();
    Task<IEnumerable<Motorcycle>> GetRentedMotorcycles();
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByBrand(string brand);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByModel(string model);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByYear(int year);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByColor(string color);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByEngineSize(int engineSize);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByMileage(int mileage);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByLicensePlate(string licensePlate);
}
