using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.Core.Interfaces.Services;

public interface ICourierService
{
    Task<Courier> GetById(Guid id);
    Task<IEnumerable<Courier>> GetAll();
    Task<Courier> GetByCnpj(string cnpj);
    Task<Courier> GetByCnhNumber(string cnhNumber);
    Task Add(Courier courier);
    Task Update(Courier courier);
    Task SoftDelete(Guid id);
}
