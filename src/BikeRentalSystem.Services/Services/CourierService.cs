using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Models.Validations;
using Microsoft.Extensions.Logging;

namespace BikeRentalSystem.Services.Services;

public class CourierService : BaseService, ICourierService
{
    private readonly ICourierRepository _courierRepository;
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<CourierService> _logger;
    private readonly INotifier _notifier;

    public CourierService(ICourierRepository courierRepository, IMessagePublisher messagePublisher, ILogger<CourierService> logger, INotifier notifier) : base(notifier)
    {
        _courierRepository = courierRepository;
        _messagePublisher = messagePublisher;
        _logger = logger;
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
            if (!ExecuteValidation(new CourierValidation(_courierRepository), entity)) return null;

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
            if (!ExecuteValidation(new CourierValidation(_courierRepository), entity)) return null;

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

    public async Task<IEnumerable<Courier>> GetAvailableCouriersAsync()
    {
        try
        {
            _notifier.Handle("Available couriers were accessed");
            return await _courierRepository.GetAvailableCouriersAsync();
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
            _notifier.Handle("Unavailable couriers were accessed");
            return await _courierRepository.GetUnavailableCouriersAsync();
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
            return await _courierRepository.GetCouriersByFirstNameAsync(firstName);
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
            return await _courierRepository.GetCouriersByLastNameAsync(lastName);
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
            return await _courierRepository.GetCouriersByCNPJAsync(cnpj);
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
            return await _courierRepository.GetCouriersByBirthDateAsync(birthDate);
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
            return await _courierRepository.GetCouriersByDriverLicenseNumberAsync(driverLicenseNumber);
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
            return await _courierRepository.GetCouriersByDriverLicenseTypeAsync(driverLicenseType);
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
            return await _courierRepository.GetCouriersByPhoneNumberAsync(phoneNumber);
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
            return await _courierRepository.GetCouriersByEmailAsync(email);
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
            return await _courierRepository.GetCouriersByImageUrlAsync(imageUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
