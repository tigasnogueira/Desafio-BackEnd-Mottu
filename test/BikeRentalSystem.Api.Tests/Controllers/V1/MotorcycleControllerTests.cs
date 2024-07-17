﻿using AutoMapper;
using BikeRentalSystem.Api.Contracts.Request;
using BikeRentalSystem.Api.Controllers.V1;
using BikeRentalSystem.Core.Common;
using BikeRentalSystem.Core.Dtos;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BikeRentalSystem.Api.Tests.Controllers.V1;

public class MotorcycleControllerTests : BaseControllerTests<MotorcycleController>
{
    private readonly IMotorcycleService _motorcycleServiceMock;
    private readonly IMapper _mapperMock;

    public MotorcycleControllerTests() : base()
    {
        _motorcycleServiceMock = Substitute.For<IMotorcycleService>();
        _mapperMock = Substitute.For<IMapper>();
        controller = new MotorcycleController(_motorcycleServiceMock, _mapperMock, _notifierMock, _userMock)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext
            }
        };
    }

    [Fact]
    public async Task GetMotorcycleById_ShouldReturnMotorcycle_WhenMotorcycleExists()
    {
        var motorcycleId = Guid.NewGuid();
        var motorcycle = new Motorcycle { Id = motorcycleId };
        var motorcycleDto = new MotorcycleDto { Id = motorcycleId };

        _motorcycleServiceMock.GetById(motorcycleId).Returns(Task.FromResult(motorcycle));
        _mapperMock.Map<MotorcycleDto>(motorcycle).Returns(motorcycleDto);

        var result = await controller.GetMotorcycleById(motorcycleId);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        var response = okResult.Value;
        Assert.True((bool)response.GetType().GetProperty("success").GetValue(response));
        Assert.Equal(motorcycleDto, response.GetType().GetProperty("data").GetValue(response));
    }

    [Fact]
    public async Task GetMotorcycleById_ShouldReturnNotFound_WhenMotorcycleDoesNotExist()
    {
        var motorcycleId = Guid.NewGuid();
        _motorcycleServiceMock.GetById(motorcycleId).Returns(Task.FromResult<Motorcycle>(null));

        var result = await controller.GetMotorcycleById(motorcycleId);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.NotNull(notFoundResult.Value);

        var responseObject = notFoundResult.Value;
        var successProperty = responseObject.GetType().GetProperty("success");
        var errorsProperty = responseObject.GetType().GetProperty("errors");

        Assert.NotNull(successProperty);
        Assert.NotNull(errorsProperty);

        Assert.False((bool)successProperty.GetValue(responseObject));
        Assert.Equal("Resource not found", errorsProperty.GetValue(responseObject) as string);
    }

    [Fact]
    public async Task GetAllMotorcycles_ShouldReturnPagedMotorcycles_WhenPagingParametersAreProvided()
    {
        var page = 1;
        var pageSize = 10;
        var motorcycles = new List<Motorcycle> { new Motorcycle() };
        var paginatedResponse = new PaginatedResponse<Motorcycle>(motorcycles, 1, page, pageSize);
        var paginatedDto = new PaginatedResponse<MotorcycleDto>(new List<MotorcycleDto> { new MotorcycleDto() }, 1, page, pageSize);

        _motorcycleServiceMock.GetAllPaged(page, pageSize).Returns(Task.FromResult(paginatedResponse));
        _mapperMock.Map<PaginatedResponse<MotorcycleDto>>(paginatedResponse).Returns(paginatedDto);

        var result = await controller.GetAllMotorcycles(page, pageSize);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);

        var responseObject = okResult.Value;
        var successProperty = responseObject.GetType().GetProperty("success");
        var dataProperty = responseObject.GetType().GetProperty("data");

        Assert.NotNull(successProperty);
        Assert.NotNull(dataProperty);

        Assert.True((bool)successProperty.GetValue(responseObject));
        Assert.Equal(paginatedDto, dataProperty.GetValue(responseObject));
    }

    [Fact]
    public async Task GetAllMotorcycles_ShouldReturnAllMotorcycles_WhenNoPagingParametersProvided()
    {
        var motorcycles = new List<Motorcycle> { new Motorcycle() };
        var motorcycleDtos = new List<MotorcycleDto> { new MotorcycleDto() };

        _motorcycleServiceMock.GetAll().Returns(Task.FromResult((IEnumerable<Motorcycle>)motorcycles));
        _mapperMock.Map<IEnumerable<MotorcycleDto>>(motorcycles).Returns(motorcycleDtos);

        var result = await controller.GetAllMotorcycles(null, null);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);

        var responseObject = okResult.Value;
        var successProperty = responseObject.GetType().GetProperty("success");
        var dataProperty = responseObject.GetType().GetProperty("data");

        Assert.NotNull(successProperty);
        Assert.NotNull(dataProperty);

        Assert.True((bool)successProperty.GetValue(responseObject));
        Assert.Equal(motorcycleDtos, dataProperty.GetValue(responseObject) as IEnumerable<MotorcycleDto>);
    }

    [Fact]
    public async Task CreateMotorcycle_ShouldReturnCreated_WhenMotorcycleIsValid()
    {
        var motorcycleRequest = new MotorcycleRequest();
        var motorcycle = new Motorcycle();
        var motorcycleDto = new MotorcycleDto();

        _mapperMock.Map<Motorcycle>(motorcycleRequest).Returns(motorcycle);
        _mapperMock.Map<MotorcycleDto>(motorcycle).Returns(motorcycleDto);
        _motorcycleServiceMock.Add(motorcycle).Returns(Task.FromResult(true));

        var result = await controller.CreateMotorcycle(motorcycleRequest);

        var createdResult = Assert.IsType<ObjectResult>(result);
        Assert.NotNull(createdResult.Value);
        Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);

        var responseObject = createdResult.Value;
        var successProperty = responseObject.GetType().GetProperty("success");
        var dataProperty = responseObject.GetType().GetProperty("data");

        Assert.NotNull(successProperty);
        Assert.NotNull(dataProperty);

        Assert.True((bool)successProperty.GetValue(responseObject));
        Assert.Equal(motorcycleDto, dataProperty.GetValue(responseObject));
    }

    [Fact]
    public async Task UpdateMotorcycle_ShouldReturnNoContent_WhenMotorcycleIsUpdated()
    {
        var motorcycleId = Guid.NewGuid();
        var motorcycleUpdateRequest = new MotorcycleUpdateRequest();
        var motorcycle = new Motorcycle { Id = motorcycleId };

        _mapperMock.Map<Motorcycle>(motorcycleUpdateRequest).Returns(motorcycle);
        _motorcycleServiceMock.Update(motorcycle).Returns(Task.FromResult(true));

        var result = await controller.UpdateMotorcycle(motorcycleId, motorcycleUpdateRequest);
        var noContentResult = Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task SoftDeleteMotorcycle_ShouldReturnNoContent_WhenMotorcycleIsSoftDeleted()
    {
        var motorcycleId = Guid.NewGuid();
        _motorcycleServiceMock.SoftDelete(motorcycleId).Returns(Task.FromResult(true));

        var result = await controller.SoftDeleteMotorcycle(motorcycleId);
        var noContentResult = Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task CreateMotorcycle_ShouldReturnBadRequest_WhenLicensePlateIsDuplicate()
    {
        var motorcycleRequest = new MotorcycleRequest();
        var motorcycle = new Motorcycle();

        _mapperMock.Map<Motorcycle>(motorcycleRequest).Returns(motorcycle);
        _motorcycleServiceMock.Add(motorcycle).Returns(Task.FromResult(false));

        var result = await controller.CreateMotorcycle(motorcycleRequest);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequestResult.Value);

        var responseObject = badRequestResult.Value;
        var successProperty = responseObject.GetType().GetProperty("success");
        var errorsProperty = responseObject.GetType().GetProperty("errors");

        Assert.NotNull(successProperty);
        Assert.NotNull(errorsProperty);

        Assert.False((bool)successProperty.GetValue(responseObject));
        Assert.Equal("Resource conflict", errorsProperty.GetValue(responseObject) as string);
    }

    [Fact]
    public async Task GetAllMotorcycles_ShouldReturnBadRequest_WhenExceptionOccurs()
    {
        _motorcycleServiceMock.GetAll().Throws(new Exception("Test Exception"));

        var result = await controller.GetAllMotorcycles(null, null);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequestResult.Value);

        var responseObject = badRequestResult.Value;
        var successProperty = responseObject.GetType().GetProperty("success");
        var errorsProperty = responseObject.GetType().GetProperty("errors");

        Assert.NotNull(successProperty);
        Assert.NotNull(errorsProperty);

        Assert.False((bool)successProperty.GetValue(responseObject));
        Assert.Equal("Test Exception", errorsProperty.GetValue(responseObject) as string);
    }

    [Fact]
    public async Task GetMotorcycleById_ShouldReturnBadRequest_WhenExceptionOccurs()
    {
        var motorcycleId = Guid.NewGuid();
        _motorcycleServiceMock.GetById(motorcycleId).Throws(new Exception("Test Exception"));

        var result = await controller.GetMotorcycleById(motorcycleId);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequestResult.Value);

        var responseObject = badRequestResult.Value;
        var successProperty = responseObject.GetType().GetProperty("success");
        var errorsProperty = responseObject.GetType().GetProperty("errors");

        Assert.NotNull(successProperty);
        Assert.NotNull(errorsProperty);

        Assert.False((bool)successProperty.GetValue(responseObject));
        Assert.Equal("Test Exception", errorsProperty.GetValue(responseObject) as string);
    }

    [Fact]
    public async Task CreateMotorcycle_ShouldReturnBadRequest_WhenExceptionOccurs()
    {
        var motorcycleRequest = new MotorcycleRequest();
        _motorcycleServiceMock.When(x => x.Add(Arg.Any<Motorcycle>())).Do(x => throw new Exception("Test Exception"));

        var result = await controller.CreateMotorcycle(motorcycleRequest);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequestResult.Value);

        var responseObject = badRequestResult.Value;
        var successProperty = responseObject.GetType().GetProperty("success");
        var errorsProperty = responseObject.GetType().GetProperty("errors");

        Assert.NotNull(successProperty);
        Assert.NotNull(errorsProperty);

        Assert.False((bool)successProperty.GetValue(responseObject));
        Assert.Equal("Test Exception", errorsProperty.GetValue(responseObject) as string);
    }
}