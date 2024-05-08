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

    public async Task<IEnumerable<Courier>> GetAvailableCouriersAsync()
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

    public async Task<IEnumerable<Courier>> GetUnavailableCouriersAsync()
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

    public async Task<IEnumerable<Courier>> GetCouriersByFirstNameAsync(string firstName)
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

    public async Task<IEnumerable<Courier>> GetCouriersByLastNameAsync(string lastName)
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

    public async Task<IEnumerable<Courier>> GetCouriersByCNPJAsync(string cnpj)
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

    public async Task<IEnumerable<Courier>> GetCouriersByBirthDateAsync(DateTime birthDate)
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

    public async Task<IEnumerable<Courier>> GetCouriersByDriverLicenseNumberAsync(string driverLicenseNumber)
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

    public async Task<IEnumerable<Courier>> GetCouriersByDriverLicenseTypeAsync(string driverLicenseType)
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

    public async Task<IEnumerable<Courier>> GetCouriersByPhoneNumberAsync(string phoneNumber)
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

    public async Task<IEnumerable<Courier>> GetCouriersByEmailAsync(string email)
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

    public async Task<IEnumerable<Courier>> GetCouriersByImageUrlAsync(string imageUrl)
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

    public async Task<bool> IsCNPJUniqueAsync(string cnpj)
    {
        try
        {
            _notifier.Handle($"Checking if courier with CNPJ {cnpj} is unique");
            return await _collection.Find(e => e.CNPJ == cnpj).CountDocumentsAsync() == 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<bool> IsDriverLicenseNumberUniqueAsync(string driverLicenseNumber)
    {
        try
        {
            _notifier.Handle($"Checking if courier with driver license number {driverLicenseNumber} is unique");
            return await _collection.Find(e => e.DriverLicenseNumber == driverLicenseNumber).CountDocumentsAsync() == 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
