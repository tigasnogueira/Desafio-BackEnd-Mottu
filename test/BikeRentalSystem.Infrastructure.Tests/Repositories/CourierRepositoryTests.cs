using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Core.Tests.Helpers;
using BikeRentalSystem.Infrastructure.Context;
using BikeRentalSystem.Infrastructure.Repositories;
using FluentAssertions;
using NSubstitute;

namespace BikeRentalSystem.Infrastructure.Tests.Repositories;

public class CourierRepositoryTests : IDisposable
{
    private readonly DataContext _dataContext;
    private readonly INotifier _notifier;
    private readonly IBlobStorageService _blobStorageService;
    private readonly CourierRepository _repository;

    public CourierRepositoryTests()
    {
        _dataContext = DataContextMock.Create();
        _notifier = NotifierMock.Create();
        _blobStorageService = BlobStorageServiceMock.Create();
        _repository = new CourierRepository(_dataContext, _notifier, _blobStorageService);
    }

    public void Dispose()
    {
        _dataContext.Database.EnsureDeleted();
        _dataContext.Dispose();
    }

    [Fact]
    public async Task AddOrUpdateCnhImage_ShouldUpdateCnhImageUrl_WhenCourierExists()
    {
        // Arrange
        var cnpj = "12345678901234";
        var courier = new Courier { Cnpj = cnpj, CnhNumber = "AB123456", Name = "John Doe", CnhType = "A" };
        await _dataContext.Couriers.AddAsync(courier);
        await _dataContext.SaveChangesAsync();

        var cnhImageStream = new MemoryStream();
        _blobStorageService.UploadFileAsync(cnhImageStream, "AB123456.png").Returns("http://example.com/AB123456.png");

        // Act
        var result = await _repository.AddOrUpdateCnhImage(cnpj, cnhImageStream);

        // Assert
        result.Should().Be("http://example.com/AB123456.png");

        var updatedCourier = await _dataContext.Couriers.FindAsync(courier.Id);
        updatedCourier.CnhImage.Should().Be("http://example.com/AB123456.png");
    }

    [Fact]
    public async Task AddOrUpdateCnhImage_ShouldReturnNull_WhenCourierDoesNotExist()
    {
        // Act
        var result = await _repository.AddOrUpdateCnhImage("non_existing_cnpj", new MemoryStream());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByCnpj_ShouldReturnCourier_WhenCourierExists()
    {
        // Arrange
        var cnpj = "12345678901234";
        var courier = new Courier { Cnpj = cnpj, Name = "John Doe", CnhType = "A", CnhNumber = "AB123456" };
        await _dataContext.Couriers.AddAsync(courier);
        await _dataContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetByCnpj(cnpj);

        // Assert
        result.Should().NotBeNull();
        result.Cnpj.Should().Be(cnpj);
    }

    [Fact]
    public async Task GetByCnpj_ShouldReturnNull_WhenCourierDoesNotExist()
    {
        // Act
        var result = await _repository.GetByCnpj("non_existing_cnpj");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByCnhNumber_ShouldReturnCourier_WhenCourierExists()
    {
        // Arrange
        var cnhNumber = "AB123456";
        var courier = new Courier { CnhNumber = cnhNumber, Name = "John Doe", CnhType = "A", Cnpj = "12345678901234" };
        await _dataContext.Couriers.AddAsync(courier);
        await _dataContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetByCnhNumber(cnhNumber);

        // Assert
        result.Should().NotBeNull();
        result.CnhNumber.Should().Be(cnhNumber);
    }

    [Fact]
    public async Task GetByCnhNumber_ShouldReturnNull_WhenCourierDoesNotExist()
    {
        // Act
        var result = await _repository.GetByCnhNumber("non_existing_cnh_number");

        // Assert
        result.Should().BeNull();
    }
}
