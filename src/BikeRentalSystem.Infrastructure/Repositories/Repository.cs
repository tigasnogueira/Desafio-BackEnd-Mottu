using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BikeRentalSystem.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : EntityModel
{
    private readonly IMongoCollection<T> _collection;
    private readonly ILogger<Repository<T>> _logger;

    public Repository(IMongoDatabase database, string collectionName, ILogger<Repository<T>> logger)
    {
        _collection = database.GetCollection<T>(collectionName);
        _logger = logger;
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        try
        {
            return await _collection.Find(e => e.Id == id).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            return await _collection.Find(e => true).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<T> AddAsync(T entity)
    {
        try
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<T> UpdateAsync(T entity)
    {
        try
        {
            await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<T> DeleteAsync(Guid id)
    {
        try
        {
            return await _collection.FindOneAndDeleteAsync(e => e.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
