using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.Core.Interfaces.Repositories;

public interface ICourierRepository : IRepository<Courier>
{
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
    Task<bool> IsCNPJUniqueAsync(string cnpj);
    Task<bool> IsDriverLicenseNumberUniqueAsync(string driverLicenseNumber);
}
