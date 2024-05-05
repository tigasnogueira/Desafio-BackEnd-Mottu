using BikeRentalSystem.Core.Models;

namespace BikeRentalSystem.Core.Interfaces.Repositories;

public interface ICourierRepository : IRepository<Courier>
{
    IEnumerable<Courier> GetAvailableCouriers();
    IEnumerable<Courier> GetUnavailableCouriers();
    IEnumerable<Courier> GetCouriersByFirstName(string firstName);
    IEnumerable<Courier> GetCouriersByLastName(string lastName);
    IEnumerable<Courier> GetCouriersByCNPJ(string cnpj);
    IEnumerable<Courier> GetCouriersByBirthDate(DateTime birthDate);
    IEnumerable<Courier> GetCouriersByDriverLicenseNumber(string driverLicenseNumber);
    IEnumerable<Courier> GetCouriersByDriverLicenseType(string driverLicenseType);
    IEnumerable<Courier> GetCouriersByPhoneNumber(string phoneNumber);
    IEnumerable<Courier> GetCouriersByEmail(string email);
    IEnumerable<Courier> GetCouriersByImageUrl(string imageUrl);
}
