using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Infrastructure.Database;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BikeRentalSystem.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : EntityModel
{
    private readonly IMongoCollection<T> _collection;
    private readonly ILogger<Repository<T>> _logger;
    private readonly INotifier _notifier;

    public Repository(MongoDBContext database, string collectionName, ILogger<Repository<T>> logger, INotifier notifier)
    {
        _collection = database.GetCollection<T>(collectionName);
        _logger = logger;
        _notifier = notifier;
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        try
        {
            _notifier.Handle($"Entity with id {id} was accessed");
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
            _notifier.Handle("All entities were accessed");
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
            _notifier.Handle("Entity was added");
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
            _notifier.Handle($"Entity with id {entity.Id} was updated");
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
            _notifier.Handle($"Entity with id {id} was deleted");
            return await _collection.FindOneAndDeleteAsync(e => e.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
