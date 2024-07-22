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
[Route("api/v{version:apiVersion}/rentals")]
public class RentalController : MainController
{
    private readonly IRentalService _rentalService;
    private readonly IMapper _mapper;
    private readonly IRedisCacheService _redisCacheService;

    public RentalController(IRentalService rentalService,
                            IMapper mapper,
                            IRedisCacheService redisCacheService,
                            INotifier notifier,
                            IAspNetUser user) : base(notifier, user)
    {
        _rentalService = rentalService;
        _mapper = mapper;
        _redisCacheService = redisCacheService;
    }

    [HttpGet("{id:guid}")]
    [ClaimsAuthorize("Rental", "Get")]
    public async Task<IActionResult> GetRentalById(Guid id)
    {
        var cacheKey = $"Rental:{id}";
        var cachedRental = await _redisCacheService.GetCacheValueAsync<RentalDto>(cacheKey);
        if (cachedRental != null)
        {
            return CustomResponse(cachedRental);
        }

        return await HandleRequestAsync(
            async () =>
            {
                var rental = await _rentalService.GetById(id);
                if (rental == null)
                {
                    return CustomResponse("Resource not found", StatusCodes.Status404NotFound);
                }
                var rentalDto = _mapper.Map<RentalDto>(rental);
                await _redisCacheService.SetCacheValueAsync(cacheKey, rentalDto);
                return CustomResponse(rentalDto);
            },
            ex => CustomResponse(ex.Message)
        );
    }

    [AllowAnonymous]
    [HttpGet("list")]
    public async Task<IActionResult> GetAllRentals([FromQuery] int? page, [FromQuery] int? pageSize)
    {
        return await HandleRequestAsync(
            async () =>
            {
                string cacheKey = page.HasValue && pageSize.HasValue
                    ? $"RentalList:Page:{page.Value}:PageSize:{pageSize.Value}"
                    : "RentalList:All";

                if (page.HasValue && pageSize.HasValue)
                {
                    var cachedRentals = await _redisCacheService.GetCacheValueAsync<PaginatedResponse<RentalDto>>(cacheKey);
                    if (cachedRentals != null)
                    {
                        return CustomResponse(cachedRentals);
                    }

                    var rentals = await _rentalService.GetAllPaged(page.Value, pageSize.Value);
                    var rentalDtos = _mapper.Map<PaginatedResponse<RentalDto>>(rentals);
                    await _redisCacheService.SetCacheValueAsync(cacheKey, rentalDtos);
                    return CustomResponse(rentalDtos);
                }
                else
                {
                    var cachedRentals = await _redisCacheService.GetCacheValueAsync<IEnumerable<RentalDto>>(cacheKey);
                    if (cachedRentals != null)
                    {
                        return CustomResponse(cachedRentals);
                    }

                    var rentals = await _rentalService.GetAll();
                    var rentalDtos = _mapper.Map<IEnumerable<RentalDto>>(rentals);
                    await _redisCacheService.SetCacheValueAsync(cacheKey, rentalDtos);
                    return CustomResponse(rentalDtos);
                }
            },
            ex => CustomResponse(ex.Message)
        );
    }

    [HttpPost]
    [ClaimsAuthorize("Rental", "Add")]
    public async Task<IActionResult> CreateRental(RentalRequest rentalDto)
    {
        return await HandleRequestAsync(
            async () =>
            {
                var rental = _mapper.Map<Rental>(rentalDto);
                var result = await _rentalService.Add(rental, UserEmail);

                if (!result)
                {
                    return CustomResponse("Resource conflict", StatusCodes.Status400BadRequest);
                }

                var createdRentalDto = _mapper.Map<RentalDto>(rental);
                return CustomResponse(createdRentalDto, StatusCodes.Status201Created);
            },
            ex => CustomResponse(ex.Message)
        );
    }

    [HttpPut("{id:guid}")]
    [ClaimsAuthorize("Rental", "Update")]
    public async Task<IActionResult> UpdateRental(Guid id, RentalUpdateRequest rentalDto)
    {
        return await HandleRequestAsync(
            async () =>
            {
                var rental = _mapper.Map<Rental>(rentalDto);
                rental.Id = id;
                await _rentalService.Update(rental, UserEmail);
                var updatedRentalDto = _mapper.Map<RentalDto>(rental);
                return CustomResponse(updatedRentalDto, StatusCodes.Status204NoContent);
            },
            ex => CustomResponse(ex.Message)
        );
    }

    [HttpPatch("{id:guid}")]
    [ClaimsAuthorize("Rental", "Delete")]
    public async Task<IActionResult> SoftDeleteRental(Guid id)
    {
        return await HandleRequestAsync(
            async () =>
            {
                await _rentalService.SoftDelete(id, UserEmail);
                return CustomResponse(null, StatusCodes.Status204NoContent);
            },
            ex => CustomResponse(ex.Message)
        );
    }
}
