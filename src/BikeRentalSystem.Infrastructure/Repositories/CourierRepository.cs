﻿using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BikeRentalSystem.Infrastructure.Repositories;

public class CourierRepository : Repository<Courier>, ICourierRepository
{
    private readonly IMongoCollection<Courier> _collection;
    private readonly ILogger<CourierRepository> _logger;

    public CourierRepository(IMongoDatabase database, ILogger<CourierRepository> logger)
        : base(database, "couriers", logger)
    {
        _collection = database.GetCollection<Courier>("couriers");
        _logger = logger;
    }

    public async Task<IEnumerable<Courier>> GetAvailableCouriers()
    {
        try
        {
            return _collection.Find(e => !e.IsAvailable).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetUnavailableCouriers()
    {
        try
        {
            return _collection.Find(e => e.IsAvailable).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByFirstName(string firstName)
    {
        try
        {
            return _collection.Find(e => e.FirstName == firstName).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByLastName(string lastName)
    {
        try
        {
            return _collection.Find(e => e.LastName == lastName).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByCNPJ(string cnpj)
    {
        try
        {
            return _collection.Find(e => e.CNPJ == cnpj).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByBirthDate(DateTime birthDate)
    {
        try
        {
            return _collection.Find(e => e.BirthDate == birthDate).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByDriverLicenseNumber(string driverLicenseNumber)
    {
        try
        {
            return _collection.Find(e => e.DriverLicenseNumber == driverLicenseNumber).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByDriverLicenseType(string driverLicenseType)
    {
        try
        {
            return _collection.Find(e => e.DriverLicenseType == driverLicenseType).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByPhoneNumber(string phoneNumber)
    {
        try
        {
            return _collection.Find(e => e.PhoneNumber == phoneNumber).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByEmail(string email)
    {
        try
        {
            return _collection.Find(e => e.Email == email).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Courier>> GetCouriersByImageUrl(string imageUrl)
    {
        try
        {
            return _collection.Find(e => e.ImageUrl == imageUrl).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
