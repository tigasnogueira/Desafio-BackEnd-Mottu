using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.Core.Interfaces.Repositories;

public interface IMotorcycleRepository : IRepository<Motorcycle>
{
    Task<IEnumerable<Motorcycle>> GetAvailableMotorcyclesAsync();
    Task<IEnumerable<Motorcycle>> GetRentedMotorcyclesAsync();
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByBrandAsync(string brand);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByModelAsync(string model);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByYearAsync(int year);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByColorAsync(string color);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByEngineSizeAsync(int engineSize);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByMileageAsync(int mileage);
    Task<Motorcycle> GetMotorcycleByLicensePlateAsync(string licensePlate);
    Task<bool> IsLicensePlateUniqueAsync(string licensePlate);
    Task<bool> MotorcycleHasRentalsAsync(Guid motorcycleId);
}
