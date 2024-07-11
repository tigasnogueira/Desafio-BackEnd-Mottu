using Asp.Versioning;
using AutoMapper;
using BikeRentalSystem.Api.Extensions;
using BikeRentalSystem.Api.Models.Dtos;
using BikeRentalSystem.Api.Models.Request;
using BikeRentalSystem.Core.Common;
using BikeRentalSystem.Core.Interfaces;
using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeRentalSystem.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/couriers")]
public class CourierController : MainController
{
    private readonly ICourierService _courierService;
    private readonly IMapper _mapper;

    public CourierController(ICourierService courierService, IMapper mapper, INotifier notifier, IUser user) : base(notifier, user)
    {
        _courierService = courierService;
        _mapper = mapper;
    }

    [HttpGet("{id:guid}")]
    [ClaimsAuthorize("Courier", "Get")]
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

    [AllowAnonymous]
    [HttpGet("list")]
    public async Task<IActionResult> GetAllCouriers([FromQuery] int? page, [FromQuery] int? pageSize)
    {
        try
        {
            if (page.HasValue && pageSize.HasValue)
            {
                var couriers = await _courierService.GetAllPaged(page.Value, pageSize.Value);
                var courierDtos = _mapper.Map<PaginatedResponse<CourierDto>>(couriers);
                return CustomResponse(courierDtos);
            }
            else
            {
                var couriers = await _courierService.GetAll();
                var courierDtos = _mapper.Map<IEnumerable<CourierDto>>(couriers);
                return CustomResponse(courierDtos);
            }
        }
        catch (Exception ex)
        {
            return CustomResponse(ex.Message);
        }
    }

    [HttpPost]
    [ClaimsAuthorize("Courier", "Add")]
    public async Task<IActionResult> CreateCourier(CourierRequest courierDto)
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
    [ClaimsAuthorize("Courier", "Update")]
    public async Task<IActionResult> UpdateCourier(Guid id, CourierUpdateRequest courierDto)
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

    [HttpPatch("{id:guid}")]
    [ClaimsAuthorize("Courier", "Delete")]
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
