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
[Route("api/v{version:apiVersion}/rentals")]
public class RentalController : MainController
{
    private readonly IRentalService _rentalService;
    private readonly IMapper _mapper;

    public RentalController(IRentalService rentalService, IMapper mapper, INotifier notifier, IUser user) : base(notifier, user)
    {
        _rentalService = rentalService;
        _mapper = mapper;
    }

    [HttpGet("{id:guid}")]
    [ClaimsAuthorize("Rental", "Get")]
    public async Task<IActionResult> GetRentalById(Guid id)
    {
        try
        {
            var rental = await _rentalService.GetById(id);
            var rentalDto = _mapper.Map<RentalDto>(rental);
            return CustomResponse(rentalDto);
        }
        catch (Exception ex)
        {
            return CustomResponse(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpGet("list")]
    public async Task<IActionResult> GetAllRentals([FromQuery] int? page, [FromQuery] int? pageSize)
    {
        try
        {
            if (page.HasValue && pageSize.HasValue)
            {
                var rentals = await _rentalService.GetAllPaged(page.Value, pageSize.Value);
                var rentalDtos = _mapper.Map<PaginatedResponse<RentalDto>>(rentals);
                return CustomResponse(rentalDtos);
            }
            else
            {
                var rentals = await _rentalService.GetAll();
                var rentalDtos = _mapper.Map<IEnumerable<RentalDto>>(rentals);
                return CustomResponse(rentalDtos);
            }
        }
        catch (Exception ex)
        {
            return CustomResponse(ex.Message);
        }
    }

    [HttpPost]
    [ClaimsAuthorize("Rental", "Add")]
    public async Task<IActionResult> CreateRental(RentalRequest rentalDto)
    {
        try
        {
            var rental = _mapper.Map<Rental>(rentalDto);
            await _rentalService.Add(rental);
            var createdRentalDto = _mapper.Map<RentalDto>(rental);
            return CustomResponse(createdRentalDto, StatusCodes.Status201Created);
        }
        catch (Exception ex)
        {
            return CustomResponse(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    [ClaimsAuthorize("Rental", "Update")]
    public async Task<IActionResult> UpdateRental(Guid id, RentalUpdateRequest rentalDto)
    {
        try
        {
            var rental = _mapper.Map<Rental>(rentalDto);
            rental.Id = id;
            await _rentalService.Update(rental);
            return CustomResponse(StatusCodes.Status204NoContent);
        }
        catch (Exception ex)
        {
            return CustomResponse(ex.Message);
        }
    }

    [HttpPatch("{id:guid}")]
    [ClaimsAuthorize("Rental", "Delete")]
    public async Task<IActionResult> SoftDeleteRental(Guid id)
    {
        try
        {
            await _rentalService.SoftDelete(id);
            return CustomResponse(StatusCodes.Status204NoContent);
        }
        catch (Exception ex)
        {
            return CustomResponse(ex.Message);
        }
    }
}
