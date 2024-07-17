using AutoMapper;
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

public class CourierControllerTests : BaseControllerTests<CourierController>
{
    private readonly ICourierService _courierServiceMock;
    private readonly IMapper _mapperMock;

    public CourierControllerTests() : base()
    {
        _courierServiceMock = Substitute.For<ICourierService>();
        _mapperMock = Substitute.For<IMapper>();
        controller = new CourierController(_courierServiceMock, _mapperMock, _notifierMock, _userMock)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext
            }
        };
    }

    [Fact]
    public async Task GetCourierById_ShouldReturnCourier_WhenCourierExists()
    {
        var courierId = Guid.NewGuid();
        var courier = new Courier { Id = courierId };
        var courierDto = new CourierDto { Id = courierId };

        _courierServiceMock.GetById(courierId).Returns(Task.FromResult(courier));
        _mapperMock.Map<CourierDto>(courier).Returns(courierDto);

        var result = await controller.GetCourierById(courierId);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        var response = okResult.Value;
        Assert.True((bool)response.GetType().GetProperty("success").GetValue(response));
        Assert.Equal(courierDto, response.GetType().GetProperty("data").GetValue(response));
    }

    [Fact]
    public async Task GetCourierById_ShouldReturnNotFound_WhenCourierDoesNotExist()
    {
        // Arrange
        var courierId = Guid.NewGuid();
        _courierServiceMock.GetById(courierId).Returns(Task.FromResult<Courier>(null));

        // Act
        var result = await controller.GetCourierById(courierId);

        // Assert
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
    public async Task GetAllCouriers_ShouldReturnPagedCouriers_WhenPagingParametersAreProvided()
    {
        // Arrange
        var page = 1;
        var pageSize = 10;
        var couriers = new List<Courier> { new Courier() };
        var paginatedResponse = new PaginatedResponse<Courier>(couriers, 1, page, pageSize);
        var paginatedDto = new PaginatedResponse<CourierDto>(new List<CourierDto> { new CourierDto() }, 1, page, pageSize);

        _courierServiceMock.GetAllPaged(page, pageSize).Returns(Task.FromResult(paginatedResponse));
        _mapperMock.Map<PaginatedResponse<CourierDto>>(paginatedResponse).Returns(paginatedDto);

        // Act
        var result = await controller.GetAllCouriers(page, pageSize);

        // Assert
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
    public async Task GetAllCouriers_ShouldReturnAllCouriers_WhenNoPagingParametersProvided()
    {
        // Arrange
        var couriers = new List<Courier> { new Courier() };
        var courierDtos = new List<CourierDto> { new CourierDto() };

        _courierServiceMock.GetAll().Returns(Task.FromResult((IEnumerable<Courier>)couriers));
        _mapperMock.Map<IEnumerable<CourierDto>>(couriers).Returns(courierDtos);

        // Act
        var result = await controller.GetAllCouriers(null, null);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);

        // Assert
        var responseObject = okResult.Value;
        var successProperty = responseObject.GetType().GetProperty("success");
        var dataProperty = responseObject.GetType().GetProperty("data");

        Assert.NotNull(successProperty);
        Assert.NotNull(dataProperty);

        Assert.True((bool)successProperty.GetValue(responseObject));
        Assert.Equal(courierDtos, dataProperty.GetValue(responseObject) as IEnumerable<CourierDto>);
    }

    [Fact]
    public async Task CreateCourier_ShouldReturnCreated_WhenCourierIsValid()
    {
        // Arrange
        var courierRequest = new CourierRequest
        {
            Name = "John Doe",
            Cnpj = "12345678901234",
            BirthDate = new DateTime(1980, 1, 1),
            CnhNumber = "1234567890",
            CnhType = "A"
        };

        var courier = new Courier
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Cnpj = "12345678901234",
            BirthDate = new DateTime(1980, 1, 1),
            CnhNumber = "1234567890",
            CnhType = "A"
        };

        var courierDto = new CourierDto
        {
            Id = courier.Id,
            Name = "John Doe",
            Cnpj = "12345678901234",
            BirthDate = new DateTime(1980, 1, 1),
            CnhNumber = "1234567890",
            CnhType = "A"
        };

        var cnhImage = Substitute.For<IFormFile>();
        var stream = new MemoryStream();
        cnhImage.OpenReadStream().Returns(stream);

        _mapperMock.Map<Courier>(courierRequest).Returns(courier);
        _mapperMock.Map<CourierDto>(courier).Returns(courierDto);
        _courierServiceMock.Add(courier).Returns(Task.FromResult(true));

        // Act
        var result = await controller.CreateCourier(courierRequest);

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
        Assert.Equal(courierDto, dataProperty.GetValue(responseObject));
    }

    [Fact]
    public async Task UpdateCourier_ShouldReturnNoContent_WhenCourierIsUpdated()
    {
        var courierId = Guid.NewGuid();
        var courierUpdateRequest = new CourierUpdateRequest();
        var courier = new Courier { Id = courierId };
        var cnhImage = Substitute.For<IFormFile>();

        _mapperMock.Map<Courier>(courierUpdateRequest).Returns(courier);
        _courierServiceMock.Update(courier);

        var result = await controller.UpdateCourier(courierId, courierUpdateRequest);
        var noContentResult = Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task SoftDeleteCourier_ShouldReturnNoContent_WhenCourierIsSoftDeleted()
    {
        var courierId = Guid.NewGuid();
        _courierServiceMock.SoftDelete(courierId).Returns(Task.FromResult(true));

        var result = await controller.SoftDeleteCourier(courierId);
        var noContentResult = Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task AddOrUpdateCnhImage_ShouldReturnNoContent_WhenImageIsUpdated()
    {
        var cnpj = "12345678000100";
        var cnhImage = Substitute.For<IFormFile>();
        var stream = new MemoryStream();

        cnhImage.OpenReadStream().Returns(stream);
        _courierServiceMock.AddOrUpdateCnhImage(cnpj, Arg.Any<Stream>()).Returns(Task.FromResult(true));

        var result = await controller.AddOrUpdateCnhImage(cnpj, cnhImage);
        var noContentResult = Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task AddOrUpdateCnhImage_ShouldReturnBadRequest_WhenImageUpdateFails()
    {
        var cnpj = "12345678000100";
        var cnhImage = Substitute.For<IFormFile>();
        var stream = new MemoryStream();

        cnhImage.OpenReadStream().Returns(stream);
        _courierServiceMock.AddOrUpdateCnhImage(cnpj, Arg.Any<Stream>()).Returns(Task.FromResult(false));

        var result = await controller.AddOrUpdateCnhImage(cnpj, cnhImage);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequestResult.Value);
        var response = badRequestResult.Value;
        Assert.False((bool)response.GetType().GetProperty("success").GetValue(response));
        var errors = (string)response.GetType().GetProperty("errors").GetValue(response);
        Assert.Equal("Failed to update CNH image", errors);
    }

    [Fact]
    public async Task CreateCourier_ShouldReturnBadRequest_WhenCnpjOrCnhIsDuplicate()
    {
        // Arrange
        var courierRequest = new CourierRequest();
        var courier = new Courier();
        var cnhImage = Substitute.For<IFormFile>();

        _mapperMock.Map<Courier>(courierRequest).Returns(courier);
        _courierServiceMock.Add(courier);

        // Act
        var result = await controller.CreateCourier(courierRequest);

        // Assert
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
    public async Task GetAllCouriers_ShouldReturnBadRequest_WhenExceptionOccurs()
    {
        // Arrange
        _courierServiceMock.GetAll().Throws(new Exception("Test Exception"));

        // Act
        var result = await controller.GetAllCouriers(null, null);

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
    }

    [Fact]
    public async Task GetCourierById_ShouldReturnBadRequest_WhenExceptionOccurs()
    {
        // Arrange
        var courierId = Guid.NewGuid();
        _courierServiceMock.GetById(courierId).Throws(new Exception("Test Exception"));

        // Act
        var result = await controller.GetCourierById(courierId);

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
    }

    [Fact]
    public async Task CreateCourier_ShouldReturnBadRequest_WhenExceptionOccurs()
    {
        // Arrange
        var courierRequest = new CourierRequest();
        var cnhImage = Substitute.For<IFormFile>();
        _courierServiceMock.When(x => x.Add(Arg.Any<Courier>())).Do(x => throw new Exception("Test Exception"));

        // Act
        var result = await controller.CreateCourier(courierRequest);

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
    }
}
