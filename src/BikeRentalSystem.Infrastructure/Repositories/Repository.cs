using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using MongoDB.Driver;

namespace BikeRentalSystem.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : EntityModel
{
    private readonly IMongoCollection<T> _collection;

    public Repository(IMongoDatabase database, string collectionName)
    {
        _collection = database.GetCollection<T>(collectionName);
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _collection.Find(e => e.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _collection.Find(e => true).ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
        return entity;
    }

    public async Task<T> DeleteAsync(Guid id)
    {
        return await _collection.FindOneAndDeleteAsync(e => e.Id == id);
    }
}
