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

public class RentalControllerTests : BaseControllerTests<RentalController>
{
    private readonly IRentalService _rentalServiceMock;
    private readonly IMapper _mapperMock;
    private readonly IRedisCacheService _redisCacheServiceMock;

    public RentalControllerTests() : base()
    {
        _rentalServiceMock = Substitute.For<IRentalService>();
        _mapperMock = Substitute.For<IMapper>();
        _redisCacheServiceMock = Substitute.For<IRedisCacheService>();

        _userMock.GetUserName().Returns("TestUser");
        _userMock.GetUserEmail().Returns("TestUser");

        controller = new RentalController(_rentalServiceMock, _mapperMock, _redisCacheServiceMock, _notifierMock, _userMock)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext
            }
        };
    }

    private void VerifyCommonErrorResponse(ObjectResult result, string expectedErrorMessage)
    {
        Assert.NotNull(result.Value);

        var responseObject = result.Value;
        var successProperty = responseObject.GetType().GetProperty("success");
        var errorsProperty = responseObject.GetType().GetProperty("errors");

        Assert.NotNull(successProperty);
        Assert.NotNull(errorsProperty);

        Assert.False((bool)successProperty.GetValue(responseObject));
        Assert.Equal(expectedErrorMessage, errorsProperty.GetValue(responseObject) as string);
    }

    [Fact]
    public async Task GetRentalById_ShouldReturnRental_WhenRentalExists()
    {
        var rentalId = Guid.NewGuid();
        var rental = new Rental { Id = rentalId };
        var rentalDto = new RentalDto { Id = rentalId };
        var cacheKey = $"Rental:{rentalId}";

        _redisCacheServiceMock.GetCacheValueAsync<RentalDto>(cacheKey).Returns((RentalDto)null);
        _rentalServiceMock.GetById(rentalId).Returns(Task.FromResult(rental));
        _mapperMock.Map<RentalDto>(rental).Returns(rentalDto);

        var result = await controller.GetRentalById(rentalId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);

        var response = okResult.Value;
        Assert.True((bool)response.GetType().GetProperty("success").GetValue(response));
        Assert.Equal(rentalDto, response.GetType().GetProperty("data").GetValue(response));

        await _redisCacheServiceMock.Received(1).GetCacheValueAsync<RentalDto>(cacheKey);
        await _redisCacheServiceMock.Received(1).SetCacheValueAsync(cacheKey, rentalDto);
    }

    [Fact]
    public async Task GetRentalById_ShouldReturnNotFound_WhenRentalDoesNotExist()
    {
        var rentalId = Guid.NewGuid();
        var cacheKey = $"Rental:{rentalId}";

        _redisCacheServiceMock.GetCacheValueAsync<RentalDto>(cacheKey).Returns((RentalDto)null);
        _rentalServiceMock.GetById(rentalId).Returns(Task.FromResult<Rental>(null));

        var result = await controller.GetRentalById(rentalId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        VerifyCommonErrorResponse(notFoundResult, "Resource not found");

        await _redisCacheServiceMock.Received(1).GetCacheValueAsync<RentalDto>(cacheKey);
    }

    [Fact]
    public async Task GetAllRentals_ShouldReturnPagedRentals_WhenPagingParametersAreProvided()
    {
        var page = 1;
        var pageSize = 10;
        var rentals = new List<Rental> { new Rental() };
        var paginatedResponse = new PaginatedResponse<Rental>(rentals, 1, page, pageSize);
        var paginatedDto = new PaginatedResponse<RentalDto>(new List<RentalDto> { new RentalDto() }, 1, page, pageSize);
        var cacheKey = $"RentalList:Page:{page}:PageSize:{pageSize}";

        _redisCacheServiceMock.GetCacheValueAsync<PaginatedResponse<RentalDto>>(cacheKey).Returns((PaginatedResponse<RentalDto>)null);
        _rentalServiceMock.GetAllPaged(page, pageSize).Returns(Task.FromResult(paginatedResponse));
        _mapperMock.Map<PaginatedResponse<RentalDto>>(paginatedResponse).Returns(paginatedDto);

        var result = await controller.GetAllRentals(page, pageSize);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);

        var responseObject = okResult.Value;
        var successProperty = responseObject.GetType().GetProperty("success");
        var dataProperty = responseObject.GetType().GetProperty("data");

        Assert.NotNull(successProperty);
        Assert.NotNull(dataProperty);

        Assert.True((bool)successProperty.GetValue(responseObject));
        Assert.Equal(paginatedDto, dataProperty.GetValue(responseObject));

        await _redisCacheServiceMock.Received(1).GetCacheValueAsync<PaginatedResponse<RentalDto>>(cacheKey);
        await _redisCacheServiceMock.Received(1).SetCacheValueAsync(cacheKey, paginatedDto);
    }

    [Fact]
    public async Task GetAllRentals_ShouldReturnAllRentals_WhenNoPagingParametersProvided()
    {
        var rentals = new List<Rental> { new Rental() };
        var rentalDtos = new List<RentalDto> { new RentalDto() };
        var cacheKey = "RentalList:All";

        _redisCacheServiceMock.GetCacheValueAsync<IEnumerable<RentalDto>>(cacheKey).Returns((IEnumerable<RentalDto>)null);
        _rentalServiceMock.GetAll().Returns(Task.FromResult((IEnumerable<Rental>)rentals));
        _mapperMock.Map<IEnumerable<RentalDto>>(rentals).Returns(rentalDtos);

        var result = await controller.GetAllRentals(null, null);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);

        var responseObject = okResult.Value;
        var successProperty = responseObject.GetType().GetProperty("success");
        var dataProperty = responseObject.GetType().GetProperty("data");

        Assert.NotNull(successProperty);
        Assert.NotNull(dataProperty);

        Assert.True((bool)successProperty.GetValue(responseObject));
        Assert.Equal(rentalDtos, dataProperty.GetValue(responseObject) as IEnumerable<RentalDto>);

        await _redisCacheServiceMock.Received(1).GetCacheValueAsync<IEnumerable<RentalDto>>(cacheKey);
        await _redisCacheServiceMock.Received(1).SetCacheValueAsync(cacheKey, rentalDtos);
    }

    [Fact]
    public async Task CreateRental_ShouldReturnCreated_WhenRentalIsValid()
    {
        // Arrange
        var rentalRequest = new RentalRequest();
        var rental = new Rental();
        var rentalDto = new RentalDto();

        _mapperMock.Map<Rental>(rentalRequest).Returns(rental);
        _mapperMock.Map<RentalDto>(rental).Returns(rentalDto);
        _rentalServiceMock.Add(rental, "TestUser").Returns(Task.FromResult(true));

        // Act
        var result = await controller.CreateRental(rentalRequest);

        // Assert
        var createdResult = Assert.IsType<ObjectResult>(result);
        Assert.NotNull(createdResult.Value);
        Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);

        var responseObject = createdResult.Value;
        var successProperty = responseObject.GetType().GetProperty("success");
        var dataProperty = responseObject.GetType().GetProperty("data");

        Assert.NotNull(successProperty);
        Assert.NotNull(dataProperty);

        Assert.True((bool)successProperty.GetValue(responseObject));
        Assert.Equal(rentalDto, dataProperty.GetValue(responseObject));
    }

    [Fact]
    public async Task UpdateRental_ShouldReturnNoContent_WhenRentalIsUpdated()
    {
        // Arrange
        var rentalId = Guid.NewGuid();
        var rentalUpdateRequest = new RentalUpdateRequest();
        var rental = new Rental { Id = rentalId };

        _mapperMock.Map<Rental>(rentalUpdateRequest).Returns(rental);
        _rentalServiceMock.Update(rental, "TestUser").Returns(Task.FromResult(true));

        // Act
        var result = await controller.UpdateRental(rentalId, rentalUpdateRequest);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task SoftDeleteRental_ShouldReturnNoContent_WhenRentalIsSoftDeleted()
    {
        // Arrange
        var rentalId = Guid.NewGuid();
        _rentalServiceMock.SoftDelete(rentalId, "TestUser").Returns(Task.FromResult(true));

        // Act
        var result = await controller.SoftDeleteRental(rentalId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task CreateRental_ShouldReturnBadRequest_WhenRentalIsInvalid()
    {
        // Arrange
        var rentalRequest = new RentalRequest();
        var rental = new Rental();

        _mapperMock.Map<Rental>(rentalRequest).Returns(rental);
        _rentalServiceMock.Add(rental, "TestUser").Returns(Task.FromResult(false));

        // Act
        var result = await controller.CreateRental(rentalRequest);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        VerifyCommonErrorResponse(badRequestResult, "Resource conflict");
    }

    [Fact]
    public async Task GetAllRentals_ShouldReturnBadRequest_WhenExceptionOccurs()
    {
        var cacheKey = "RentalList:All";

        // Arrange
        _redisCacheServiceMock.GetCacheValueAsync<IEnumerable<RentalDto>>(cacheKey).Returns(Task.FromResult<IEnumerable<RentalDto>>(null));

        _rentalServiceMock.GetAll().Throws(new Exception("Test Exception"));

        // Act
        var result = await controller.GetAllRentals(null, null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequestResult.Value);

        var responseObject = badRequestResult.Value;
        var successProperty = responseObject.GetType().GetProperty("success");
        var errorsProperty = responseObject.GetType().GetProperty("errors");

        Assert.NotNull(successProperty);
        Assert.NotNull(errorsProperty);

        Assert.False((bool)successProperty.GetValue(responseObject));
        Assert.Equal("Test Exception", errorsProperty.GetValue(responseObject) as string);

        await _redisCacheServiceMock.Received(1).GetCacheValueAsync<IEnumerable<RentalDto>>(cacheKey);
    }

    [Fact]
    public async Task GetRentalById_ShouldReturnBadRequest_WhenExceptionOccurs()
    {
        // Arrange
        var rentalId = Guid.NewGuid();
        _rentalServiceMock.GetById(rentalId).Throws(new Exception("Test Exception"));

        // Act
        var result = await controller.GetRentalById(rentalId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        VerifyCommonErrorResponse(badRequestResult, "Test Exception");
    }

    [Fact]
    public async Task CreateRental_ShouldReturnBadRequest_WhenExceptionOccurs()
    {
        // Arrange
        var rentalRequest = new RentalRequest();
        _rentalServiceMock.When(x => x.Add(Arg.Any<Rental>(), Arg.Any<string>())).Do(x => throw new Exception("Test Exception"));

        // Act
        var result = await controller.CreateRental(rentalRequest);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        VerifyCommonErrorResponse(badRequestResult, "Test Exception");
    }
}
