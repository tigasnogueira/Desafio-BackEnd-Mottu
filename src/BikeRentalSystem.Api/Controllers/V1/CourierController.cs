using Asp.Versioning;
using AutoMapper;
using BikeRentalSystem.Api.Models.Dtos;
using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BikeRentalSystem.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/couriers")]
public class CourierController : MainController
{
    private readonly ICourierService _courierService;
    private readonly IMapper _mapper;

    public CourierController(ICourierService courierService, IMapper mapper, INotifier notifier) : base(notifier)
    {
        _courierService = courierService;
        _mapper = mapper;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourierDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCourierById(Guid id)
    {
        try
        {
            var courier = await _courierService.GetById(id);
            var courierDto = _mapper.Map<CourierDto>(courier);
            return CustomResponse(courierDto);
        }
        catch (Exception ex)
        {
            return CustomResponse(ex.Message);
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CourierDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllCouriers()
    {
        try
        {
            var couriers = await _courierService.GetAll();
            var courierDtos = _mapper.Map<IEnumerable<CourierDto>>(couriers);
            return CustomResponse(courierDtos);
        }
        catch (Exception ex)
        {
            return CustomResponse(ex.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCourier(CourierDto courierDto)
    {
        try
        {
            var courier = _mapper.Map<Courier>(courierDto);
            await _courierService.Add(courier);
            var createdCourierDto = _mapper.Map<CourierDto>(courier);
            return CustomResponse(createdCourierDto, StatusCodes.Status201Created);
        }
        catch (Exception ex)
        {
            return CustomResponse(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCourier(Guid id, CourierDto courierDto)
    {
        try
        {
            var courier = _mapper.Map<Courier>(courierDto);
            courier.Id = id;
            await _courierService.Update(courier);
            return CustomResponse(StatusCodes.Status204NoContent);
        }
        catch (Exception ex)
        {
            return CustomResponse(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SoftDeleteCourier(Guid id)
    {
        try
        {
            await _courierService.SoftDelete(id);
            return CustomResponse(StatusCodes.Status204NoContent);
        }
        catch (Exception ex)
        {
            return CustomResponse(ex.Message);
        }
    }
}
