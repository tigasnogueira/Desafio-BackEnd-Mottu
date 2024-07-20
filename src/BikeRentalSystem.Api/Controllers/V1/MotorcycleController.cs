﻿using Asp.Versioning;
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
    private readonly IRedisCacheService _redisCacheService;

    public MotorcycleController(IMotorcycleService motorcycleService,
                                IMapper mapper,
                                IRedisCacheService redisCacheService,
                                INotifier notifier,
                                IAspNetUser user) : base(notifier, user)
    {
        _motorcycleService = motorcycleService;
        _mapper = mapper;
        _redisCacheService = redisCacheService;
    }

    [HttpGet("{id:guid}")]
    [ClaimsAuthorize("Motorcycle", "Get")]
    public async Task<IActionResult> GetMotorcycleById(Guid id)
    {
        var cacheKey = $"Motorcycle:{id}";
        var cachedMotorcycle = await _redisCacheService.GetCacheValueAsync<MotorcycleDto>(cacheKey);
        if (cachedMotorcycle != null)
        {
            return CustomResponse(cachedMotorcycle);
        }

        return await HandleRequestAsync(
            async () =>
            {
                var motorcycle = await _motorcycleService.GetById(id);
                if (motorcycle == null)
                {
                    return CustomResponse("Resource not found", StatusCodes.Status404NotFound);
                }
                var motorcycleDto = _mapper.Map<MotorcycleDto>(motorcycle);
                await _redisCacheService.SetCacheValueAsync(cacheKey, motorcycleDto);
                return CustomResponse(motorcycleDto);
            },
            ex => CustomResponse(ex.Message, StatusCodes.Status400BadRequest)
        );
    }

    [AllowAnonymous]
    [HttpGet("list")]
    public async Task<IActionResult> GetAllMotorcycles([FromQuery] int? page, [FromQuery] int? pageSize)
    {
        return await HandleRequestAsync(
            async () =>
            {
                string cacheKey = page.HasValue && pageSize.HasValue
                    ? $"MotorcycleList:Page:{page.Value}:PageSize:{pageSize.Value}"
                    : "MotorcycleList:All";

                if (page.HasValue && pageSize.HasValue)
                {
                    var cachedMotorcycles = await _redisCacheService.GetCacheValueAsync<PaginatedResponse<MotorcycleDto>>(cacheKey);
                    if (cachedMotorcycles != null)
                    {
                        return CustomResponse(cachedMotorcycles);
                    }

                    var motorcycles = await _motorcycleService.GetAllPaged(page.Value, pageSize.Value);
                    var motorcycleDtos = _mapper.Map<PaginatedResponse<MotorcycleDto>>(motorcycles);
                    await _redisCacheService.SetCacheValueAsync(cacheKey, motorcycleDtos);
                    return CustomResponse(motorcycleDtos);
                }
                else
                {
                    var cachedMotorcycles = await _redisCacheService.GetCacheValueAsync<IEnumerable<MotorcycleDto>>(cacheKey);
                    if (cachedMotorcycles != null)
                    {
                        return CustomResponse(cachedMotorcycles);
                    }

                    var motorcycles = await _motorcycleService.GetAll();
                    var motorcycleDtos = _mapper.Map<IEnumerable<MotorcycleDto>>(motorcycles);
                    await _redisCacheService.SetCacheValueAsync(cacheKey, motorcycleDtos);
                    return CustomResponse(motorcycleDtos);
                }
            },
            ex => CustomResponse(ex.Message, StatusCodes.Status400BadRequest)
        );
    }

    [HttpPost]
    [ClaimsAuthorize("Motorcycle", "Add")]
    public async Task<IActionResult> CreateMotorcycle(MotorcycleRequest motorcycleDto)
    {
        return await HandleRequestAsync(
            async () =>
            {
                var motorcycle = _mapper.Map<Motorcycle>(motorcycleDto);
                var result = await _motorcycleService.Add(motorcycle, UserEmail);
                if (!result)
                {
                    return CustomResponse("Resource conflict", StatusCodes.Status400BadRequest);
                }
                var createdMotorcycleDto = _mapper.Map<MotorcycleDto>(motorcycle);
                return CustomResponse(createdMotorcycleDto, StatusCodes.Status201Created);
            },
            ex => CustomResponse(ex.Message, StatusCodes.Status400BadRequest)
        );
    }

    [HttpPut("{id:guid}")]
    [ClaimsAuthorize("Motorcycle", "Update")]
    public async Task<IActionResult> UpdateMotorcycle(Guid id, MotorcycleUpdateRequest motorcycleDto)
    {
        return await HandleRequestAsync(
            async () =>
            {
                var motorcycle = _mapper.Map<Motorcycle>(motorcycleDto);
                motorcycle.Id = id;
                await _motorcycleService.Update(motorcycle, UserEmail);
                var updatedMotorcycleDto = _mapper.Map<MotorcycleDto>(motorcycle);
                return CustomResponse(updatedMotorcycleDto, StatusCodes.Status204NoContent);
            },
            ex => CustomResponse(ex.Message, StatusCodes.Status400BadRequest)
        );
    }

    [HttpPatch("{id:guid}/status")]
    [ClaimsAuthorize("Motorcycle", "Delete")]
    public async Task<IActionResult> SoftDeleteMotorcycle(Guid id)
    {
        return await HandleRequestAsync(
            async () =>
            {
                await _motorcycleService.SoftDelete(id, UserEmail);
                return CustomResponse(null, StatusCodes.Status204NoContent);
            },
            ex => CustomResponse(ex.Message, StatusCodes.Status400BadRequest)
        );
    }
}
