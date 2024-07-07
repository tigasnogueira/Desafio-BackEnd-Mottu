using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.Core.Interfaces.Services;

public interface ICourierService
{
    Task<Courier> GetCourierByIdAsync(Guid id);
    Task<IEnumerable<Courier>> GetAllAsync();
    Task<Courier> AddCourierAsync(Courier entity);
    Task<Courier> UpdateCourierAsync(Courier entity);
    Task<Courier> DeleteCourierAsync(Guid id);
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
