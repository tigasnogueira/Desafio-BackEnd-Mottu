using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.Core.Interfaces.Repositories;

public interface IMotorcycleRepository : IRepository<Motorcycle>
{
    Task<IEnumerable<Motorcycle>> GetAvailableMotorcycles();
    Task<IEnumerable<Motorcycle>> GetRentedMotorcycles();
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByBrand(string brand);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByModel(string model);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByYear(int year);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByColor(string color);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByEngineSize(int engineSize);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByMileage(int mileage);
    Task<Motorcycle> GetMotorcycleByLicensePlate(string licensePlate);
    Task<bool> IsLicensePlateUniqueAsync(string licensePlate);
}
