using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.Core.Interfaces.Services;

public interface IMotorcycleService
{
    Task<Motorcycle> GetById(Guid id);
    Task<IEnumerable<Motorcycle>> GetAll();
    Task<Motorcycle> GetByPlate(string plate);
    Task<IEnumerable<Motorcycle>> GetAllByYear(int year);
    Task Add(Motorcycle motorcycle);
    Task Update(Motorcycle motorcycle);
    Task SoftDelete(Guid id);
}
