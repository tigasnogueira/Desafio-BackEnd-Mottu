using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BikeRentalSystem.Infrastructure.Repositories;

public class CourierRepository : Repository<Courier>, ICourierRepository
{
    private readonly IMongoCollection<Courier> _collection;
    private readonly ILogger<CourierRepository> _logger;
    private readonly INotifier _notifier;

    public CourierRepository(IMongoDatabase database, ILogger<CourierRepository> logger, INotifier notifier)
        : base(database, "couriers", logger, notifier)
    {
        _collection = database.GetCollection<Courier>("couriers");
        _logger = logger;
    }

    public async Task<IEnumerable<Courier>> GetAvailableCouriers()
    {
        try
        {
            _notifier.Handle("All available couriers were accessed");
            return _collection.Find(e => !e.IsAvailable).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetUnavailableCouriers()
    {
        try
        {
            _notifier.Handle("All unavailable couriers were accessed");
            return _collection.Find(e => e.IsAvailable).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByFirstName(string firstName)
    {
        try
        {
            _notifier.Handle($"Couriers with first name {firstName} were accessed");
            return _collection.Find(e => e.FirstName == firstName).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByLastName(string lastName)
    {
        try
        {
            _notifier.Handle($"Couriers with last name {lastName} were accessed");
            return _collection.Find(e => e.LastName == lastName).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByCNPJ(string cnpj)
    {
        try
        {
            _notifier.Handle($"Couriers with CNPJ {cnpj} were accessed");
            return _collection.Find(e => e.CNPJ == cnpj).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByBirthDate(DateTime birthDate)
    {
        try
        {
            _notifier.Handle($"Couriers with birth date {birthDate} were accessed");
            return _collection.Find(e => e.BirthDate == birthDate).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByDriverLicenseNumber(string driverLicenseNumber)
    {
        try
        {
            _notifier.Handle($"Couriers with driver license number {driverLicenseNumber} were accessed");
            return _collection.Find(e => e.DriverLicenseNumber == driverLicenseNumber).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByDriverLicenseType(string driverLicenseType)
    {
        try
        {
            _notifier.Handle($"Couriers with driver license type {driverLicenseType} were accessed");
            return _collection.Find(e => e.DriverLicenseType == driverLicenseType).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByPhoneNumber(string phoneNumber)
    {
        try
        {
            _notifier.Handle($"Couriers with phone number {phoneNumber} were accessed");
            return _collection.Find(e => e.PhoneNumber == phoneNumber).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByEmail(string email)
    {
        try
        {
            _notifier.Handle($"Couriers with email {email} were accessed");
            return _collection.Find(e => e.Email == email).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByImageUrl(string imageUrl)
    {
        try
        {
            _notifier.Handle($"Couriers with image URL {imageUrl} were accessed");
            return _collection.Find(e => e.ImageUrl == imageUrl).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
