using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.Core.Interfaces.Services;

public interface IMotorcycleService
{
    Task<Motorcycle> GetMotorcycleByIdAsync(Guid id);
    Task<IEnumerable<Motorcycle>> GetAllAsync();
    Task<Motorcycle> AddMotorcycleAsync(Motorcycle entity);
    Task<Motorcycle> UpdateMotorcycleAsync(Motorcycle entity);
    Task<Motorcycle> DeleteMotorcycleAsync(Guid id);
    Task<IEnumerable<Motorcycle>> GetAvailableMotorcyclesAsync();
    Task<IEnumerable<Motorcycle>> GetRentedMotorcyclesAsync();
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByBrandAsync(string brand);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByModelAsync(string model);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByYearAsync(int year);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByColorAsync(string color);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByEngineSizeAsync(int engineSize);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByMileageAsync(int mileage);
    Task<Motorcycle> GetMotorcycleByLicensePlateAsync(string licensePlate);
    Task<Motorcycle> UpdateMotorcycleLicensePlateAsync(Guid id, string newLicensePlate);
}
