using BikeRentalSystem.Core.Common;
using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.Core.Interfaces.Services;

public interface ICourierService
{
    Task<Courier> GetById(Guid id);
    Task<IEnumerable<Courier>> GetAll();
    Task<PaginatedResponse<Courier>> GetAllPaged(int page, int pageSize);
    Task<Courier> GetByCnpj(string cnpj);
    Task<Courier> GetByCnhNumber(string cnhNumber);
    Task<bool> Add(Courier courier, Stream cnhImageStream = null);
    Task<bool> Update(Courier courier, Stream cnhImageStream = null);
    Task<bool> SoftDelete(Guid id);
    Task<bool> AddOrUpdateCnhImage(string cnpj, Stream cnhImageStream);
}
