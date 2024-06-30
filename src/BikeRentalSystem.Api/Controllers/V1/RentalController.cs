using Asp.Versioning;
using AutoMapper;
using BikeRentalSystem.Api.Dtos;
using BikeRentalSystem.Api.Extensions;
using BikeRentalSystem.Core.Interfaces;
using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeRentalSystem.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/rentals")]
public class RentalController : MainController
{
    private readonly IRentalService _rentalService;
    private readonly ILogger<RentalController> _logger;
    public IMapper _mapper;

    public RentalController(IRentalService rentalService, 
                            ILogger<RentalController> logger, 
                            IMapper mapper, INotifier notifier, 
                            IUser user) : base(notifier, user)
    {
        _rentalService = rentalService;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RentalDto>>> GetAll(int? pageNumber, int? pageSize)
    {
        try
        {
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                var rentals = await _rentalService.GetAllPagedRentalsAsync(pageNumber.Value, pageSize.Value);
                return CustomResponse(rentals);
            }
            else
            {
                var rentals = await _rentalService.GetAllAsync();
                return CustomResponse(rentals);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the rentals.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RentalDto>> GetById(Guid id)
    {
        try
        {
            var rental = await _rentalService.GetRentalByIdAsync(id);
            return CustomResponse(rental);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the rental.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [ClaimsAuthorize("Rental", "Add")]
    [HttpPost]
    public async Task<ActionResult<RentalDto>> Add(RentalDto rentalDto)
    {
        try
        {
            var rental = _mapper.Map<Rental>(rentalDto);
            var result = await _rentalService.AddRentalAsync(rental);
            return CustomResponse(_mapper.Map<RentalDto>(result), 201);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while adding the rental.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [ClaimsAuthorize("Rental", "Update")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<RentalDto>> Update(Guid id, RentalDto rentalDto)
    {
        try
        {
            var rental = _mapper.Map<Rental>(rentalDto);
            rental.Id = id;
            var result = await _rentalService.UpdateRentalAsync(rental);
            return CustomResponse(_mapper.Map<RentalDto>(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while updating the rental.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [ClaimsAuthorize("Rental", "Delete")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RentalDto>> Delete(Guid id)
    {
        try
        {
            var result = await _rentalService.DeleteRentalAsync(id);
            return CustomResponse(_mapper.Map<RentalDto>(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while deleting the rental.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("motorcycle/{motorcycleId:guid}")]
    public async Task<ActionResult<IEnumerable<RentalDto>>> GetRentalsByMotorcycleId(Guid motorcycleId)
    {
        try
        {
            var rentals = await _rentalService.GetRentalsByMotorcycleIdAsync(motorcycleId);
            return CustomResponse(rentals);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the rentals.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("courier/{courierId:guid}")]
    public async Task<ActionResult<IEnumerable<RentalDto>>> GetRentalsByCourierId(Guid courierId)
    {
        try
        {
            var rentals = await _rentalService.GetRentalsByCourierIdAsync(courierId);
            return CustomResponse(rentals);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the rentals.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("start-date/{startDate}")]
    public async Task<ActionResult<IEnumerable<RentalDto>>> GetRentalsByStartDate(DateTime startDate)
    {
        try
        {
            var rentals = await _rentalService.GetRentalsByStartDateAsync(startDate);
            return CustomResponse(rentals);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the rentals.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("end-date/{endDate}")]
    public async Task<ActionResult<IEnumerable<RentalDto>>> GetRentalsByEndDate(DateTime endDate)
    {
        try
        {
            var rentals = await _rentalService.GetRentalsByEndDateAsync(endDate);
            return CustomResponse(rentals);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the rentals.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("date-range/{startDate}/{endDate}")]
    public async Task<ActionResult<IEnumerable<RentalDto>>> GetRentalsByDateRange(DateTime startDate, DateTime endDate)
    {
        try
        {
            var rentals = await _rentalService.GetRentalsByDateRangeAsync(startDate, endDate);
            return CustomResponse(rentals);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the rentals.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("price/{price}")]
    public async Task<ActionResult<IEnumerable<RentalDto>>> GetRentalsByPrice(decimal price)
    {
        try
        {
            var rentals = await _rentalService.GetRentalsByPriceAsync(price);
            return CustomResponse(rentals);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the rentals.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("price-range/{minPrice}/{maxPrice}")]
    public async Task<ActionResult<IEnumerable<RentalDto>>> GetRentalsByPriceRange(decimal minPrice, decimal maxPrice)
    {
        try
        {
            var rentals = await _rentalService.GetRentalsByPriceRangeAsync(minPrice, maxPrice);
            return CustomResponse(rentals);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the rentals.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("paid/{isPaid}")]
    public async Task<ActionResult<IEnumerable<RentalDto>>> GetRentalsByPaidStatus(bool isPaid)
    {
        try
        {
            var rentals = await _rentalService.GetRentalsByPaidStatusAsync(isPaid);
            return CustomResponse(rentals);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the rentals.", NotificationType.Error);
            return CustomResponse();
        }
    }

    [HttpGet("finished/{isFinished}")]
    public async Task<ActionResult<IEnumerable<RentalDto>>> GetRentalsByFinishedStatus(bool isFinished)
    {
        try
        {
            var rentals = await _rentalService.GetRentalsByFinishedStatusAsync(isFinished);
            return CustomResponse(rentals);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            _notifier.Handle("An error occurred while fetching the rentals.", NotificationType.Error);
            return CustomResponse();
        }
    }
}
