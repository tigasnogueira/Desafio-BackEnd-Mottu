using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using Microsoft.Extensions.Logging;

namespace BikeRentalSystem.Services.Services;

public class CourierService : ICourierService
{
    private readonly ICourierRepository _courierRepository;
    private readonly ILogger<CourierService> _logger;

    public CourierService(ICourierRepository courierRepository, ILogger<CourierService> logger)
    {
        _courierRepository = courierRepository;
        _logger = logger;
    }

    public async Task<Courier> GetCourierByIdAsync(Guid id)
    {
        try
        {
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
            return await _courierRepository.GetCouriersByImageUrl(imageUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
