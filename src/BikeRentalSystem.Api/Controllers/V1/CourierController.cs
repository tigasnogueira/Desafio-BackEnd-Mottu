using Asp.Versioning;
using AutoMapper;
using BikeRentalSystem.Api.Dtos;
using BikeRentalSystem.Api.Extensions;
using BikeRentalSystem.Core.Interfaces;
using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeRentalSystem.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/couriers")]
public class CourierController : MainController
{
    private readonly ICourierService _courierService;
    private readonly ILogger<CourierController> _logger;
    public IMapper _mapper;

    public CourierController(ICourierService courierService, 
                             ILogger<CourierController> logger, 
                             IMapper mapper, INotifier notifier, 
                             IUser user) : base(notifier, user)
    {
        _courierService = courierService;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetAll(int? pageNumber, int? pageSize)
    {
        try
        {
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                var couriersPaged = await _courierService.GetAllPagedCouriersAsync(pageNumber.Value, pageSize.Value);
                return CustomResponse(couriersPaged);
            }
            else
            {
                var couriers = await _courierService.GetAllAsync();
                return CustomResponse(couriers);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the couriers.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CourierDto>> GetById(Guid id)
    {
        try
        {
            var courier = await _courierService.GetCourierByIdAsync(id);
            return CustomResponse(courier);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the courier.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [ClaimsAuthorize("Couriers", "Add")]
    [HttpPost]
    public async Task<ActionResult<CourierDto>> Add(CourierDto courierDto)
    {
        try
        {
            var courier = await _courierService.AddCourierAsync(_mapper.Map<Courier>(courierDto));
            return CustomResponse(courier);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while adding the courier.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [ClaimsAuthorize("Couriers", "Update")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CourierDto>> Update(Guid id, CourierDto courierDto)
    {
        if (id != courierDto.Id)
        {
            _notifier.Handle("The id in the request does not match the id in the body.", NotificationType.Error);
            return CustomResponse();
        }

        try
        {
            var courier = await _courierService.UpdateCourierAsync(_mapper.Map<Courier>(courierDto));
            return CustomResponse(courier);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while updating the courier.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [ClaimsAuthorize("Couriers", "Delete")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<CourierDto>> Delete(Guid id)
    {
        try
        {
            var courier = await _courierService.DeleteCourierAsync(id);
            return CustomResponse(courier);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while deleting the courier.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetAvailableCouriers()
    {
        try
        {
            var couriers = await _courierService.GetAvailableCouriersAsync();
            return CustomResponse(couriers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the available couriers.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("unavailable")]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetUnavailableCouriers()
    {
        try
        {
            var couriers = await _courierService.GetUnavailableCouriersAsync();
            return CustomResponse(couriers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the unavailable couriers.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("first-name/{firstName}")]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetByFirstName(string firstName)
    {
        try
        {
            var couriers = await _courierService.GetCouriersByFirstNameAsync(firstName);
            return CustomResponse(couriers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the couriers by first name.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("last-name/{lastName}")]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetByLastName(string lastName)
    {
        try
        {
            var couriers = await _courierService.GetCouriersByLastNameAsync(lastName);
            return CustomResponse(couriers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the couriers by last name.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("cnpj/{cnpj}")]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetByCNPJ(string cnpj)
    {
        try
        {
            var couriers = await _courierService.GetCouriersByCNPJAsync(cnpj);
            return CustomResponse(couriers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the couriers by CNPJ.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("birth-date/{birthDate}")]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetByBirthDate(DateTime birthDate)
    {
        try
        {
            var couriers = await _courierService.GetCouriersByBirthDateAsync(birthDate);
            return CustomResponse(couriers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the couriers by birth date.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("driver-license-number/{driverLicenseNumber}")]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetByDriverLicenseNumber(string driverLicenseNumber)
    {
        try
        {
            var couriers = await _courierService.GetCouriersByDriverLicenseNumberAsync(driverLicenseNumber);
            return CustomResponse(couriers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the couriers by driver license number.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("driver-license-type/{driverLicenseType}")]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetByDriverLicenseType(string driverLicenseType)
    {
        try
        {
            var couriers = await _courierService.GetCouriersByDriverLicenseTypeAsync(driverLicenseType);
            return CustomResponse(couriers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the couriers by driver license type.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("phone-number/{phoneNumber}")]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetByPhoneNumber(string phoneNumber)
    {
        try
        {
            var couriers = await _courierService.GetCouriersByPhoneNumberAsync(phoneNumber);
            return CustomResponse(couriers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the couriers by phone number.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetByEmail(string email)
    {
        try
        {
            var couriers = await _courierService.GetCouriersByEmailAsync(email);
            return CustomResponse(couriers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the couriers by email.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("image-url/{imageUrl}")]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetByImageUrl(string imageUrl)
    {
        try
        {
            var couriers = await _courierService.GetCouriersByImageUrlAsync(imageUrl);
            return CustomResponse(couriers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the couriers by image URL.", NotificationType.Error);
            return CustomResponse();
        }
    }
}
