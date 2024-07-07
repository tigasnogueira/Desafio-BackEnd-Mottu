using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.Core.Interfaces.Services;

public interface IMotorcycleService
{
    Task<Motorcycle> GetById(Guid id);
    Task<IEnumerable<Motorcycle>> GetAll();
    Task<Motorcycle> GetByPlate(string plate);
    Task<IEnumerable<Motorcycle>> GetAllByYear(int year);
    Task<bool> Add(Motorcycle motorcycle);
    Task<bool> Update(Motorcycle motorcycle);
    Task<bool> SoftDelete(Guid id);
}
