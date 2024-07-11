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
[Route("api/v{version:apiVersion}/motorcycles")]
public class MotorcycleController : MainController
{
    private readonly IMotorcycleService _motorcycleService;
    private readonly IMapper _mapper;

    public MotorcycleController(IMotorcycleService motorcycleService, IMapper mapper, INotifier notifier, IUser user) : base(notifier, user)
    {
        _motorcycleService = motorcycleService;
        _mapper = mapper;
    }

    [HttpGet("{id:guid}")]
    //[ClaimsAuthorize("Motorcycle", "Get")]
    public async Task<IActionResult> GetMotorcycleById(Guid id)
    {
        try
        {
            var motorcycle = await _motorcycleService.GetById(id);
            var motorcycleDto = _mapper.Map<MotorcycleDto>(motorcycle);
            return CustomResponse(motorcycleDto);
        }
        catch (Exception ex)
        {
            return CustomResponse(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpGet("list")]
    public async Task<IActionResult> GetAllMotorcycles([FromQuery] int? page, [FromQuery] int? pageSize)
    {
        try
        {
            if (page.HasValue && pageSize.HasValue)
            {
                var motorcycles = await _motorcycleService.GetAllPaged(page.Value, pageSize.Value);
                var motorcycleDtos = _mapper.Map<PaginatedResponse<MotorcycleDto>>(motorcycles);
                return CustomResponse(motorcycleDtos);
            }
            else
            {
                var motorcycles = await _motorcycleService.GetAll();
                var motorcycleDtos = _mapper.Map<IEnumerable<MotorcycleDto>>(motorcycles);
                return CustomResponse(motorcycleDtos);
            }
        }
        catch (Exception ex)
        {
            return CustomResponse(ex.Message);
        }
    }

    [HttpPost]
    [ClaimsAuthorize("Motorcycle", "Add")]
    public async Task<IActionResult> CreateMotorcycle(MotorcycleRequest motorcycleDto)
    {
        try
        {
            var motorcycle = _mapper.Map<Motorcycle>(motorcycleDto);
            await _motorcycleService.Add(motorcycle);
            var createdMotorcycleDto = _mapper.Map<MotorcycleDto>(motorcycle);
            return CustomResponse(createdMotorcycleDto, StatusCodes.Status201Created);
        }
        catch (Exception ex)
        {
            return CustomResponse(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    [ClaimsAuthorize("Motorcycle", "Update")]
    public async Task<IActionResult> UpdateMotorcycle(Guid id, MotorcycleUpdateRequest motorcycleDto)
    {
        try
        {
            var motorcycle = _mapper.Map<Motorcycle>(motorcycleDto);
            motorcycle.Id = id;
            await _motorcycleService.Update(motorcycle);
            return CustomResponse(StatusCodes.Status204NoContent);
        }
        catch (Exception ex)
        {
            return CustomResponse(ex.Message);
        }
    }

    [HttpPatch("{id:guid}/status")]
    [ClaimsAuthorize("Motorcycle", "Delete")]
    public async Task<IActionResult> SoftDeleteMotorcycle(Guid id)
    {
        try
        {
            await _motorcycleService.SoftDelete(id);
            return CustomResponse(StatusCodes.Status204NoContent);
        }
        catch (Exception ex)
        {
            return CustomResponse(ex.Message);
        }
    }
}
