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
[Route("api/v{version:apiVersion}/couriers")]
public class CourierController : MainController
{
    private readonly ICourierService _courierService;
    private readonly IMapper _mapper;

    public CourierController(ICourierService courierService, IMapper mapper, INotifier notifier, IAspNetUser user) : base(notifier, user)
    {
        _courierService = courierService;
        _mapper = mapper;
    }

    [HttpGet("{id:guid}")]
    [ClaimsAuthorize("Courier", "Get")]
    public async Task<IActionResult> GetCourierById(Guid id)
    {
        return await HandleRequestAsync(
            async () =>
            {
                var courier = await _courierService.GetById(id);
                if (courier == null)
                {
                    NotifyError("Resource not found");
                    return CustomResponse(null, StatusCodes.Status404NotFound);
                }
                var courierDto = _mapper.Map<CourierDto>(courier);
                return CustomResponse(courierDto);
            },
            ex => CustomResponse(ex.Message, StatusCodes.Status400BadRequest)
        );
    }

    [AllowAnonymous]
    [HttpGet("list")]
    public async Task<IActionResult> GetAllCouriers([FromQuery] int? page, [FromQuery] int? pageSize)
    {
        return await HandleRequestAsync(
            async () =>
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
            },
            ex => CustomResponse(ex.Message, StatusCodes.Status400BadRequest)
        );
    }

    [RequestSizeLimit(40000000)]
    [HttpPost]
    [ClaimsAuthorize("Courier", "Add")]
    public async Task<IActionResult> CreateCourier([FromForm] CourierRequest courierDto)
    {
        return await HandleRequestAsync(
            async () =>
            {
                var courier = _mapper.Map<Courier>(courierDto);
                var success = await _courierService.Add(courier);
                if (!success)
                {
                    return CustomResponse("Resource conflict", StatusCodes.Status400BadRequest);
                }

                var createdCourierDto = _mapper.Map<CourierDto>(courier);
                return CustomResponse(createdCourierDto, StatusCodes.Status201Created);
            },
            ex => CustomResponse(ex.Message, StatusCodes.Status400BadRequest)
        );
    }

    [RequestSizeLimit(40000000)]
    [HttpPut("{id:guid}")]
    [ClaimsAuthorize("Courier", "Update")]
    public async Task<IActionResult> UpdateCourier(Guid id, [FromForm] CourierUpdateRequest courierDto)
    {
        return await HandleRequestAsync(
            async () =>
            {
                var courier = _mapper.Map<Courier>(courierDto);
                courier.Id = id;
                await _courierService.Update(courier);

                var updatedCourierDto = _mapper.Map<CourierDto>(courier);
                return CustomResponse(updatedCourierDto, StatusCodes.Status204NoContent);
            },
            ex => CustomResponse(ex.Message)
        );
    }

    [HttpPatch("{id:guid}")]
    [ClaimsAuthorize("Courier", "Delete")]
    public async Task<IActionResult> SoftDeleteCourier(Guid id)
    {
        return await HandleRequestAsync(
            async () =>
            {
                await _courierService.SoftDelete(id);
                return CustomResponse(null, StatusCodes.Status204NoContent);
            },
            ex => CustomResponse(ex.Message)
        );
    }

    [RequestSizeLimit(40000000)]
    [HttpPatch("{cnpj}/cnh")]
    [ClaimsAuthorize("Courier", "Update")]
    public async Task<IActionResult> AddOrUpdateCnhImage(string cnpj, IFormFile cnhImage)
    {
        return await HandleRequestAsync(
            async () =>
            {
                using (var stream = cnhImage.OpenReadStream())
                {
                    var result = await _courierService.AddOrUpdateCnhImage(cnpj, stream);
                    if (!result)
                    {
                        return CustomResponse("Failed to update CNH image", StatusCodes.Status400BadRequest);
                    }
                }
                return CustomResponse("CNH image updated successfully", StatusCodes.Status204NoContent);
            },
            ex => CustomResponse(ex.Message)
        );
    }

    private async Task<IActionResult> HandleRequestAsync(Func<Task<IActionResult>> operation, Func<Exception, IActionResult> handleException)
    {
        try
        {
            return await operation();
        }
        catch (Exception ex)
        {
            return handleException(ex);
        }
    }
}
