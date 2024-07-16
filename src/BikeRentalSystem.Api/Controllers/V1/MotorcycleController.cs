using Asp.Versioning;
using AutoMapper;
using BikeRentalSystem.Api.Contracts.Request;
using BikeRentalSystem.Core.Common;
using BikeRentalSystem.Core.Dtos;
using BikeRentalSystem.Core.Interfaces;
using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Identity.Extensions;
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
    [ClaimsAuthorize("Motorcycle", "Get")]
    public async Task<IActionResult> GetMotorcycleById(Guid id)
    {
        try
        {
            var motorcycle = await _motorcycleService.GetById(id);
            if (motorcycle == null)
            {
                return CustomResponse("Resource not found", StatusCodes.Status404NotFound);
            }
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
            var result = await _motorcycleService.Add(motorcycle);
            if (!result)
            {
                return CustomResponse("Resource conflict", StatusCodes.Status400BadRequest);
            }
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
            var updatedMotorcycleDto = _mapper.Map<MotorcycleDto>(motorcycle);
            return CustomResponse(updatedMotorcycleDto, StatusCodes.Status204NoContent);
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
            return CustomResponse(null, StatusCodes.Status204NoContent);
        }
        catch (Exception ex)
        {
            return CustomResponse(ex.Message);
        }
    }
}
