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
public class MotorcycleController : MainController
{
    private readonly IMotorcycleService _motorcycleService;
    private readonly ILogger<MotorcycleController> _logger;
    public IMapper _mapper;

    public MotorcycleController(IMotorcycleService motorcycleService, ILogger<MotorcycleController> logger, IMapper mapper, INotifier notifier) : base(notifier)
    {
        _motorcycleService = motorcycleService;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetAll()
    {
        try
        {
            var motorcycles = await _motorcycleService.GetAllAsync();
            return CustomResponse(motorcycles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while fetching the motorcycles.");
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
            NotifyError("An error occurred while fetching the motorcycle.");
            return CustomResponse();
        }
    }

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
            NotifyError("An error occurred while adding the motorcycle.");
            return CustomResponse();
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<MotorcycleDto>> Update(Guid id, MotorcycleDto motorcycleDto)
    {
        try
        {
            if (id != motorcycleDto.Id)
            {
                NotifyError("The id in the request does not match the id in the motorcycle.");
                return CustomResponse();
            }

            var motorcycle = await _motorcycleService.UpdateMotorcycleAsync(_mapper.Map<Motorcycle>(motorcycleDto));
            return CustomResponse(motorcycle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while updating the motorcycle.");
            return CustomResponse();
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<MotorcycleDto>> Delete(Guid id)
    {
        try
        {
            var motorcycle = await _motorcycleService.GetMotorcycleByIdAsync(id);
            if (motorcycle == null)
            {
                NotifyError("Motorcycle not found.");
                return CustomResponse();
            }

            motorcycle = await _motorcycleService.DeleteMotorcycleAsync(id);
            return CustomResponse(motorcycle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while deleting the motorcycle.");
            return CustomResponse();
        }
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetAvailableMotorcycles()
    {
        try
        {
            var motorcycles = await _motorcycleService.GetAvailableMotorcycles();
            return CustomResponse(motorcycles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while fetching the available motorcycles.");
            return CustomResponse();
        }
    }

    [HttpGet("rented")]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetRentedMotorcycles()
    {
        try
        {
            var motorcycles = await _motorcycleService.GetRentedMotorcycles();
            return CustomResponse(motorcycles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while fetching the rented motorcycles.");
            return CustomResponse();
        }
    }

    [HttpGet("brand/{brand}")]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetMotorcyclesByBrand(string brand)
    {
        try
        {
            var motorcycles = await _motorcycleService.GetMotorcyclesByBrand(brand);
            return CustomResponse(motorcycles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while fetching the motorcycles by brand.");
            return CustomResponse();
        }
    }

    [HttpGet("model/{model}")]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetMotorcyclesByModel(string model)
    {
        try
        {
            var motorcycles = await _motorcycleService.GetMotorcyclesByModel(model);
            return CustomResponse(motorcycles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while fetching the motorcycles by model.");
            return CustomResponse();
        }
    }

    [HttpGet("year/{year:int}")]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetMotorcyclesByYear(int year)
    {
        try
        {
            var motorcycles = await _motorcycleService.GetMotorcyclesByYear(year);
            return CustomResponse(motorcycles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while fetching the motorcycles by year.");
            return CustomResponse();
        }
    }

    [HttpGet("color/{color}")]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetMotorcyclesByColor(string color)
    {
        try
        {
            var motorcycles = await _motorcycleService.GetMotorcyclesByColor(color);
            return CustomResponse(motorcycles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while fetching the motorcycles by color.");
            return CustomResponse();
        }
    }

    [HttpGet("engine-size/{engineSize:int}")]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetMotorcyclesByEngineSize(int engineSize)
    {
        try
        {
            var motorcycles = await _motorcycleService.GetMotorcyclesByEngineSize(engineSize);
            return CustomResponse(motorcycles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while fetching the motorcycles by engine size.");
            return CustomResponse();
        }
    }

    [HttpGet("mileage/{mileage:int}")]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetMotorcyclesByMileage(int mileage)
    {
        try
        {
            var motorcycles = await _motorcycleService.GetMotorcyclesByMileage(mileage);
            return CustomResponse(motorcycles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while fetching the motorcycles by mileage.");
            return CustomResponse();
        }
    }

    [HttpGet("license-plate/{licensePlate}")]
    public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetMotorcyclesByLicensePlate(string licensePlate)
    {
        try
        {
            var motorcycles = await _motorcycleService.GetMotorcycleByLicensePlate(licensePlate);
            return CustomResponse(motorcycles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            NotifyError("An error occurred while fetching the motorcycles by license plate.");
            return CustomResponse();
        }
    }
}
