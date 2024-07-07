using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.Core.Interfaces.Repositories;

public interface ICourierRepository : IRepository<Courier>
{
    Task<IEnumerable<Courier>> GetAvailableCouriers();
    Task<IEnumerable<Courier>> GetUnavailableCouriers();
    Task<IEnumerable<Courier>> GetCouriersByFirstName(string firstName);
    Task<IEnumerable<Courier>> GetCouriersByLastName(string lastName);
    Task<IEnumerable<Courier>> GetCouriersByCNPJ(string cnpj);
    Task<IEnumerable<Courier>> GetCouriersByBirthDate(DateTime birthDate);
    Task<IEnumerable<Courier>> GetCouriersByDriverLicenseNumber(string driverLicenseNumber);
    Task<IEnumerable<Courier>> GetCouriersByDriverLicenseType(string driverLicenseType);
    Task<IEnumerable<Courier>> GetCouriersByPhoneNumber(string phoneNumber);
    Task<IEnumerable<Courier>> GetCouriersByEmail(string email);
    Task<IEnumerable<Courier>> GetCouriersByImageUrl(string imageUrl);
}
