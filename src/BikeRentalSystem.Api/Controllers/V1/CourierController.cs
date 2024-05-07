using Asp.Versioning;
using AutoMapper;
using BikeRentalSystem.Api.Dtos;
using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BikeRentalSystem.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CourierController : MainController
{
    private readonly ICourierService _courierService;
    private readonly ILogger<CourierController> _logger;
    public IMapper _mapper;

    public CourierController(ICourierService courierService, ILogger<CourierController> logger, IMapper mapper, INotifier notifier) : base(notifier)
    {
        _courierService = courierService;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetAll()
    {
        try
        {
            var couriers = await _courierService.GetAllAsync();
            return CustomResponse(couriers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while fetching the couriers.");
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
            NotifyError("An error occurred while fetching the courier.");
            return CustomResponse();
        }
    }

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
            NotifyError("An error occurred while adding the courier.");
            return CustomResponse();
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CourierDto>> Update(Guid id, CourierDto courierDto)
    {
        if (id != courierDto.Id)
        {
            NotifyError("The id in the request does not match the id in the body.");
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
            NotifyError("An error occurred while updating the courier.");
            return CustomResponse();
        }
    }

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
            NotifyError("An error occurred while deleting the courier.");
            return CustomResponse();
        }
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetAvailableCouriers()
    {
        try
        {
            var couriers = await _courierService.GetAvailableCouriers();
            return CustomResponse(couriers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while fetching the available couriers.");
            return CustomResponse();
        }
    }

    [HttpGet("unavailable")]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetUnavailableCouriers()
    {
        try
        {
            var couriers = await _courierService.GetUnavailableCouriers();
            return CustomResponse(couriers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while fetching the unavailable couriers.");
            return CustomResponse();
        }
    }

    [HttpGet("first-name/{firstName}")]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetByFirstName(string firstName)
    {
        try
        {
            var couriers = await _courierService.GetCouriersByFirstName(firstName);
            return CustomResponse(couriers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while fetching the couriers by first name.");
            return CustomResponse();
        }
    }

    [HttpGet("last-name/{lastName}")]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetByLastName(string lastName)
    {
        try
        {
            var couriers = await _courierService.GetCouriersByLastName(lastName);
            return CustomResponse(couriers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while fetching the couriers by last name.");
            return CustomResponse();
        }
    }

    [HttpGet("cnpj/{cnpj}")]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetByCNPJ(string cnpj)
    {
        try
        {
            var couriers = await _courierService.GetCouriersByCNPJ(cnpj);
            return CustomResponse(couriers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while fetching the couriers by CNPJ.");
            return CustomResponse();
        }
    }

    [HttpGet("birth-date/{birthDate}")]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetByBirthDate(DateTime birthDate)
    {
        try
        {
            var couriers = await _courierService.GetCouriersByBirthDate(birthDate);
            return CustomResponse(couriers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while fetching the couriers by birth date.");
            return CustomResponse();
        }
    }

    [HttpGet("driver-license-number/{driverLicenseNumber}")]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetByDriverLicenseNumber(string driverLicenseNumber)
    {
        try
        {
            var couriers = await _courierService.GetCouriersByDriverLicenseNumber(driverLicenseNumber);
            return CustomResponse(couriers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while fetching the couriers by driver license number.");
            return CustomResponse();
        }
    }

    [HttpGet("driver-license-type/{driverLicenseType}")]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetByDriverLicenseType(string driverLicenseType)
    {
        try
        {
            var couriers = await _courierService.GetCouriersByDriverLicenseType(driverLicenseType);
            return CustomResponse(couriers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while fetching the couriers by driver license type.");
            return CustomResponse();
        }
    }

    [HttpGet("phone-number/{phoneNumber}")]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetByPhoneNumber(string phoneNumber)
    {
        try
        {
            var couriers = await _courierService.GetCouriersByPhoneNumber(phoneNumber);
            return CustomResponse(couriers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while fetching the couriers by phone number.");
            return CustomResponse();
        }
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetByEmail(string email)
    {
        try
        {
            var couriers = await _courierService.GetCouriersByEmail(email);
            return CustomResponse(couriers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while fetching the couriers by email.");
            return CustomResponse();
        }
    }

    [HttpGet("image-url/{imageUrl}")]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetByImageUrl(string imageUrl)
    {
        try
        {
            var couriers = await _courierService.GetCouriersByImageUrl(imageUrl);
            return CustomResponse(couriers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while fetching the couriers by image URL.");
            return CustomResponse();
        }
    }
}
