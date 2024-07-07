﻿using Asp.Versioning;
using AutoMapper;
using BikeRentalSystem.Api.Models.Dtos;
using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BikeRentalSystem.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/motorcycles")]
public class MotorcycleController : MainController
{
    private readonly IMotorcycleService _motorcycleService;
    private readonly IMapper _mapper;

    public MotorcycleController(IMotorcycleService motorcycleService, IMapper mapper, INotifier notifier) : base(notifier)
    {
        _motorcycleService = motorcycleService;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MotorcycleDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MotorcycleDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllMotorcycles()
    {
        try
        {
            var motorcycles = await _motorcycleService.GetAll();
            var motorcycleDtos = _mapper.Map<IEnumerable<MotorcycleDto>>(motorcycles);
            return CustomResponse(motorcycleDtos);
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
    public async Task<IActionResult> CreateMotorcycle(MotorcycleDto motorcycleDto)
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateMotorcycle(Guid id, MotorcycleDto motorcycleDto)
    {
        try
        {
            var motorcycle = _mapper.Map<Motorcycle>(motorcycleDto);
            motorcycle.Id = id;
            await _motorcycleService.Update(motorcycle);
            return CustomResponse(StatusCodes.Status204NoContent);
            }

            var motorcycle = await _motorcycleService.UpdateMotorcycleAsync(_mapper.Map<Motorcycle>(motorcycleDto));
            return CustomResponse(motorcycle);
        }
        catch (Exception ex)
        {
            return CustomResponse(ex.Message);
        }
    }

    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SoftDeleteMotorcycle(Guid id)
    {
        try
        {
            await _motorcycleService.SoftDelete(id);
            return CustomResponse(StatusCodes.Status204NoContent);
            }

            motorcycle = await _motorcycleService.DeleteMotorcycleAsync(id);
            return CustomResponse(motorcycle);
        }
        catch (Exception ex)
        {
            return CustomResponse(ex.Message);
        }
    }
}
