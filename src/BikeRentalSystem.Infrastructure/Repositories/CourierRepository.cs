using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using MongoDB.Driver;

namespace BikeRentalSystem.Infrastructure.Repositories;

public class CourierRepository : Repository<Courier>, ICourierRepository
{
    private readonly IMongoCollection<Courier> _collection;

    public CourierRepository(IMongoDatabase database)
        : base(database, "couriers")
    {
        _collection = database.GetCollection<Courier>("couriers");
    }

    public IEnumerable<Courier> GetAvailableCouriers()
    {
        return _collection.Find(e => !e.IsAvailable).ToList();
    }

    public IEnumerable<Courier> GetUnavailableCouriers()
    {
        return _collection.Find(e => e.IsAvailable).ToList();
    }

    public IEnumerable<Courier> GetCouriersByFirstName(string firstName)
    {
        return _collection.Find(e => e.FirstName == firstName).ToList();
    }

    public IEnumerable<Courier> GetCouriersByLastName(string lastName)
    {
        return _collection.Find(e => e.LastName == lastName).ToList();
    }

    public IEnumerable<Courier> GetCouriersByCNPJ(string cnpj)
    {
        return _collection.Find(e => e.CNPJ == cnpj).ToList();
    }

    public IEnumerable<Courier> GetCouriersByBirthDate(DateTime birthDate)
    {
        return _collection.Find(e => e.BirthDate == birthDate).ToList();
    }

    public IEnumerable<Courier> GetCouriersByDriverLicenseNumber(string driverLicenseNumber)
    {
        return _collection.Find(e => e.DriverLicenseNumber == driverLicenseNumber).ToList();
    }

    public IEnumerable<Courier> GetCouriersByDriverLicenseType(string driverLicenseType)
    {
        return _collection.Find(e => e.DriverLicenseType == driverLicenseType).ToList();
    }

    public IEnumerable<Courier> GetCouriersByPhoneNumber(string phoneNumber)
    {
        return _collection.Find(e => e.PhoneNumber == phoneNumber).ToList();
    }

    public IEnumerable<Courier> GetCouriersByEmail(string email)
    {
        return _collection.Find(e => e.Email == email).ToList();
    }

    public IEnumerable<Courier> GetCouriersByImageUrl(string imageUrl)
    {
        return _collection.Find(e => e.ImageUrl == imageUrl).ToList();
    }
}
