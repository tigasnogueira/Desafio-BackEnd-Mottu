using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Notifications;
using BikeRentalSystem.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BikeRentalSystem.Infrastructure.Repositories;

public class CourierRepository : Repository<Courier>, ICourierRepository
{
    public CourierRepository(BikeRentalDbContext context, ILogger<CourierRepository> logger, INotifier notifier)
        : base(context, logger, notifier)
    { }

    public async Task<IEnumerable<Courier>> GetAvailableCouriersAsync()
    {
        try
        {
            _notifier.Handle("All available couriers were accessed");
            return await _context.Couriers.AsNoTracking().Where(c => c.IsAvailable).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("Available couriers were not found", NotificationType.Error);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetUnavailableCouriersAsync()
    {
        try
        {
            _notifier.Handle("All unavailable couriers were accessed");
            return await _context.Couriers.AsNoTracking().Where(c => !c.IsAvailable).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("Unavailable couriers were not found", NotificationType.Error);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByFirstNameAsync(string firstName)
    {
        try
        {
            _notifier.Handle($"Couriers with first name {firstName} were accessed");
            return await _context.Couriers.AsNoTracking().Where(c => c.FirstName == firstName).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle($"Couriers with first name {firstName} were not found", NotificationType.Error);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByLastNameAsync(string lastName)
    {
        try
        {
            _notifier.Handle($"Couriers with last name {lastName} were accessed");
            return await _context.Couriers.AsNoTracking().Where(c => c.LastName == lastName).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle($"Couriers with last name {lastName} were not found", NotificationType.Error);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByCNPJAsync(string cnpj)
    {
        try
        {
            _notifier.Handle($"Couriers with CNPJ {cnpj} were accessed");
            return await _context.Couriers.AsNoTracking().Where(c => c.CNPJ == cnpj).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle($"Couriers with CNPJ {cnpj} were not found", NotificationType.Error);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByBirthDateAsync(DateTime birthDate)
    {
        try
        {
            _notifier.Handle($"Couriers with birth date {birthDate} were accessed");
            return await _context.Couriers.AsNoTracking().Where(c => c.BirthDate == birthDate).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle($"Couriers with birth date {birthDate} were not found", NotificationType.Error);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByDriverLicenseNumberAsync(string driverLicenseNumber)
    {
        try
        {
            _notifier.Handle($"Couriers with driver license number {driverLicenseNumber} were accessed");
            return await _context.Couriers.AsNoTracking().Where(c => c.DriverLicenseNumber == driverLicenseNumber).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle($"Couriers with driver license number {driverLicenseNumber} were not found", NotificationType.Error);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByDriverLicenseTypeAsync(string driverLicenseType)
    {
        try
        {
            _notifier.Handle($"Couriers with driver license type {driverLicenseType} were accessed");
            return await _context.Couriers.AsNoTracking().Where(c => c.DriverLicenseType == driverLicenseType).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle($"Couriers with driver license type {driverLicenseType} were not found", NotificationType.Error);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByPhoneNumberAsync(string phoneNumber)
    {
        try
        {
            _notifier.Handle($"Couriers with phone number {phoneNumber} were accessed");
            return await _context.Couriers.AsNoTracking().Where(c => c.PhoneNumber == phoneNumber).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle($"Couriers with phone number {phoneNumber} were not found", NotificationType.Error);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByEmailAsync(string email)
    {
        try
        {
            _notifier.Handle($"Couriers with email {email} were accessed");
            return await _context.Couriers.AsNoTracking().Where(c => c.Email == email).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle($"Couriers with email {email} were not found", NotificationType.Error);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByImageUrlAsync(string imageUrl)
    {
        try
        {
            _notifier.Handle($"Couriers with image URL {imageUrl} were accessed");
            return await _context.Couriers.AsNoTracking().Where(c => c.ImageUrl == imageUrl).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle($"Couriers with image URL {imageUrl} were not found", NotificationType.Error);
            throw;
        }
    }

    public async Task<bool> IsCNPJUniqueAsync(string cnpj)
    {
        try
        {
            _notifier.Handle($"Checking if courier with CNPJ {cnpj} is unique");
            return await _context.Couriers.AsNoTracking().Where(c => c.CNPJ == cnpj).CountAsync() == 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle($"Courier with CNPJ {cnpj} was not found", NotificationType.Error);
            throw;
        }
    }

    public async Task<bool> IsDriverLicenseNumberUniqueAsync(string driverLicenseNumber)
    {
        try
        {
            _notifier.Handle($"Checking if courier with driver license number {driverLicenseNumber} is unique");
            return await _context.Couriers.AsNoTracking().Where(c => c.DriverLicenseNumber == driverLicenseNumber).CountAsync() == 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle($"Courier with driver license number {driverLicenseNumber} was not found", NotificationType.Error);
            throw;
        }
    }
}
