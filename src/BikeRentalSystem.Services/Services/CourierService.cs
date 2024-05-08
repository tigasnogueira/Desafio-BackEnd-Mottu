using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using Microsoft.Extensions.Logging;

namespace BikeRentalSystem.Services.Services;

public class CourierService : ICourierService
{
    private readonly ICourierRepository _courierRepository;
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<CourierService> _logger;
    private readonly INotifier _notifier;

    public CourierService(ICourierRepository courierRepository, IMessagePublisher messagePublisher, ILogger<CourierService> logger, INotifier notifier)
    {
        _courierRepository = courierRepository;
        _messagePublisher = messagePublisher;
        _logger = logger;
        _notifier = notifier;
    }

    public async Task<Courier> GetCourierByIdAsync(Guid id)
    {
        try
        {
            _notifier.Handle($"Courier with id {id} was accessed");
            return await _courierRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetAllAsync()
    {
        try
        {
            _notifier.Handle("All couriers were accessed");
            return await _courierRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<Courier> AddCourierAsync(Courier entity)
    {
        try
        {
            _notifier.Handle("Courier was added");
            return await _courierRepository.AddAsync(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<Courier> UpdateCourierAsync(Courier entity)
    {
        try
        {
            _notifier.Handle("Courier was updated");
            return await _courierRepository.UpdateAsync(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<Courier> DeleteCourierAsync(Guid id)
    {
        try
        {
            _notifier.Handle($"Courier with id {id} was deleted");
            return await _courierRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetAvailableCouriers()
    {
        try
        {
            _notifier.Handle("Available couriers were accessed");
            return await _courierRepository.GetAvailableCouriers();
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
            _notifier.Handle("Unavailable couriers were accessed");
            return await _courierRepository.GetUnavailableCouriers();
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
            return await _courierRepository.GetCouriersByFirstName(firstName);
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
            return await _courierRepository.GetCouriersByLastName(lastName);
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
            return await _courierRepository.GetCouriersByCNPJ(cnpj);
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
            return await _courierRepository.GetCouriersByBirthDate(birthDate);
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
            return await _courierRepository.GetCouriersByDriverLicenseNumber(driverLicenseNumber);
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
            return await _courierRepository.GetCouriersByDriverLicenseType(driverLicenseType);
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
            return await _courierRepository.GetCouriersByPhoneNumber(phoneNumber);
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
            return await _courierRepository.GetCouriersByEmail(email);
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
            return await _courierRepository.GetCouriersByImageUrl(imageUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
