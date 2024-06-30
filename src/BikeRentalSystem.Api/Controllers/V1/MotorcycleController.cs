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
[Route("api/v{version:apiVersion}/motorcycles")]
public class MotorcycleController : MainController
{
    private readonly IMotorcycleService _motorcycleService;
    private readonly ILogger<MotorcycleController> _logger;
    public IMapper _mapper;

    public MotorcycleController(IMotorcycleService motorcycleService, 
                                ILogger<MotorcycleController> logger, 
                                IMapper mapper, INotifier notifier, 
                                IUser user) : base(notifier, user)
    {
        _motorcycleService = motorcycleService;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetAll(int? pageNumber, int? pageSize)
    {
        try
        {
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                var motorcyclesPaged = await _motorcycleService.GetAllPagedMotorcyclesAsync(pageNumber.Value, pageSize.Value);
                return CustomResponse(motorcyclesPaged);
            }
            else
            {
                var motorcycles = await _motorcycleService.GetAllAsync();
                return CustomResponse(motorcycles);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the motorcycles.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MotorcycleDto>> GetById(Guid id)
    {
        try
        {
            var motorcycle = await _motorcycleService.GetMotorcycleByIdAsync(id);
            return CustomResponse(motorcycle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the motorcycle.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [ClaimsAuthorize("Motorcycle", "Add")]
    [HttpPost]
    public async Task<ActionResult<MotorcycleDto>> Add(MotorcycleDto motorcycleDto)
    {
        try
        {
            var motorcycle = await _motorcycleService.AddMotorcycleAsync(_mapper.Map<Motorcycle>(motorcycleDto));
            return CustomResponse(motorcycle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while adding the motorcycle.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [ClaimsAuthorize("Motorcycle", "Update")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<MotorcycleDto>> Update(Guid id, MotorcycleDto motorcycleDto)
    {
        try
        {
            if (id != motorcycleDto.Id)
            {
                _notifier.Handle("The id in the request does not match the id in the motorcycle.", NotificationType.Error);
                return CustomResponse();
            }

            var motorcycle = await _motorcycleService.UpdateMotorcycleAsync(_mapper.Map<Motorcycle>(motorcycleDto));
            return CustomResponse(motorcycle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while updating the motorcycle.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [ClaimsAuthorize("Motorcycle", "Delete")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<MotorcycleDto>> Delete(Guid id)
    {
        try
        {
            var motorcycle = await _motorcycleService.GetMotorcycleByIdAsync(id);
            if (motorcycle == null)
            {
                _notifier.Handle("Motorcycle not found.", NotificationType.Error);
                return CustomResponse();
            }

            motorcycle = await _motorcycleService.DeleteMotorcycleAsync(id);
            return CustomResponse(motorcycle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while deleting the motorcycle.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetAvailableMotorcycles()
    {
        try
        {
            var motorcycles = await _motorcycleService.GetAvailableMotorcyclesAsync();
            return CustomResponse(motorcycles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the available motorcycles.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("rented")]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetRentedMotorcycles()
    {
        try
        {
            var motorcycles = await _motorcycleService.GetRentedMotorcyclesAsync();
            return CustomResponse(motorcycles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the rented motorcycles.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("brand/{brand}")]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetMotorcyclesByBrand(string brand)
    {
        try
        {
            var motorcycles = await _motorcycleService.GetMotorcyclesByBrandAsync(brand);
            return CustomResponse(motorcycles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the motorcycles by brand.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("model/{model}")]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetMotorcyclesByModel(string model)
    {
        try
        {
            var motorcycles = await _motorcycleService.GetMotorcyclesByModelAsync(model);
            return CustomResponse(motorcycles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the motorcycles by model.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("year/{year:int}")]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetMotorcyclesByYear(int year)
    {
        try
        {
            var motorcycles = await _motorcycleService.GetMotorcyclesByYearAsync(year);
            return CustomResponse(motorcycles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the motorcycles by year.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("color/{color}")]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetMotorcyclesByColor(string color)
    {
        try
        {
            var motorcycles = await _motorcycleService.GetMotorcyclesByColorAsync(color);
            return CustomResponse(motorcycles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the motorcycles by color.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("engine-size/{engineSize:int}")]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetMotorcyclesByEngineSize(int engineSize)
    {
        try
        {
            var motorcycles = await _motorcycleService.GetMotorcyclesByEngineSizeAsync(engineSize);
            return CustomResponse(motorcycles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the motorcycles by engine size.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("mileage/{mileage:int}")]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetMotorcyclesByMileage(int mileage)
    {
        try
        {
            var motorcycles = await _motorcycleService.GetMotorcyclesByMileageAsync(mileage);
            return CustomResponse(motorcycles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the motorcycles by mileage.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("license-plate/{licensePlate}")]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetMotorcyclesByLicensePlate(string licensePlate)
    {
        try
        {
            var motorcycle = await _motorcycleService.GetMotorcycleByLicensePlateAsync(licensePlate);
            if (motorcycle == null)
            {
                _notifier.Handle("Motorcycle not found.", NotificationType.Error);
                return NotFound();
            }
            return CustomResponse(_mapper.Map<MotorcycleDto>(motorcycle));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching the motorcycle by license plate.");
            _notifier.Handle(ex.Message, NotificationType.Error);
            return CustomResponse();
        }
    }

    [ClaimsAuthorize("Motorcycle", "Update")]
    [HttpPut("license-plate/{id:guid}")]
    public async Task<ActionResult<MotorcycleDto>> UpdateMotorcycleLicensePlate(Guid id, string newLicensePlate)
    {
        try
        {
            var motorcycle = await _motorcycleService.UpdateMotorcycleLicensePlateAsync(id, newLicensePlate);
            return CustomResponse(motorcycle);
        }
        catch (KeyNotFoundException ex)
        {
            _notifier.Handle(ex.Message, NotificationType.Error);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while updating the motorcycle license plate.", NotificationType.Error);
            return CustomResponse();
        }
    }
}
