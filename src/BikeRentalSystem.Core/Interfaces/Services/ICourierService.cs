using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.Core.Interfaces.Services;

public interface ICourierService
{
    Task<Courier> GetCourierByIdAsync(Guid id);
    Task<IEnumerable<Courier>> GetAllAsync();
    Task<Courier> AddCourierAsync(Courier entity);
    Task<Courier> UpdateCourierAsync(Courier entity);
    Task<Courier> DeleteCourierAsync(Guid id);
    Task<IEnumerable<Courier>> GetAvailableCouriersAsync();
    Task<IEnumerable<Courier>> GetUnavailableCouriersAsync();
    Task<IEnumerable<Courier>> GetCouriersByFirstNameAsync(string firstName);
    Task<IEnumerable<Courier>> GetCouriersByLastNameAsync(string lastName);
    Task<IEnumerable<Courier>> GetCouriersByCNPJAsync(string cnpj);
    Task<IEnumerable<Courier>> GetCouriersByBirthDateAsync(DateTime birthDate);
    Task<IEnumerable<Courier>> GetCouriersByDriverLicenseNumberAsync(string driverLicenseNumber);
    Task<IEnumerable<Courier>> GetCouriersByDriverLicenseTypeAsync(string driverLicenseType);
    Task<IEnumerable<Courier>> GetCouriersByPhoneNumberAsync(string phoneNumber);
    Task<IEnumerable<Courier>> GetCouriersByEmailAsync(string email);
    Task<IEnumerable<Courier>> GetCouriersByImageUrlAsync(string imageUrl);
}
