using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.Core.Interfaces.Repositories;

public interface IMotorcycleRepository : IRepository<Motorcycle>
{
    Task<Motorcycle> GetByPlate(string plate);
    Task<IEnumerable<Motorcycle>> GetAllByYear(int year);
}
