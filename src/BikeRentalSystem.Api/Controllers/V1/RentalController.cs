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
[Route("api/v{version:apiVersion}/rentals")]
public class RentalController : MainController
{
    private readonly IRentalService _rentalService;
    private readonly IMapper _mapper;

    public RentalController(IRentalService rentalService, IMapper mapper, INotifier notifier) : base(notifier)
    {
        _rentalService = rentalService;
        _mapper = mapper;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RentalDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RentalDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllRentals()
    {
        try
        {
            var rentals = await _rentalService.GetAll();
            var rentalDtos = _mapper.Map<IEnumerable<RentalDto>>(rentals);
            return CustomResponse(rentalDtos);
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
    public async Task<IActionResult> CreateRental(RentalDto rentalDto)
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateRental(Guid id, RentalDto rentalDto)
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

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
