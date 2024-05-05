using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.Core.Interfaces.Repositories;

public interface IMotorcycleRepository : IRepository<Motorcycle>
{
    IEnumerable<Motorcycle> GetAvailableMotorcycles();
    IEnumerable<Motorcycle> GetRentedMotorcycles();
    IEnumerable<Motorcycle> GetMotorcyclesByBrand(string brand);
    IEnumerable<Motorcycle> GetMotorcyclesByModel(string model);
    IEnumerable<Motorcycle> GetMotorcyclesByYear(int year);
    IEnumerable<Motorcycle> GetMotorcyclesByColor(string color);
    IEnumerable<Motorcycle> GetMotorcyclesByEngineSize(int engineSize);
    IEnumerable<Motorcycle> GetMotorcyclesByMileage(int mileage);
    IEnumerable<Motorcycle> GetMotorcyclesByLicensePlate(string licensePlate);
}
