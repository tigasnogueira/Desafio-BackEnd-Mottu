using BikeRentalSystem.Core.Common;
using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Repositories;
using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BikeRentalSystem.Infrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityModel , new()
{
    protected readonly BikeRentalDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;
    private readonly ILogger<Repository<TEntity>> _logger;
    private readonly INotifier _notifier;

    public Repository(BikeRentalDbContext context, ILogger<Repository<TEntity>> logger, INotifier notifier)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
        _logger = logger;
        _notifier = notifier;
    }

    public async Task<TEntity> GetByIdAsync(Guid id)
    {
        try
        {
            _notifier.Handle($"Entity with id {id} was accessed");
            return await _dbSet.FindAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        try
        {
            _notifier.Handle("All entities were accessed");
            return await _dbSet.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<PaginatedResponse<TEntity>> GetAllPagedAsync(int page, int pageSize)
    {
        try
        {
            _notifier.Handle("All entities were accessed");
            var totalRecords = await _dbSet.CountAsync();
            var entities = await _dbSet.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedResponse<TEntity>(entities, totalRecords, page, pageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        try
        {
            _notifier.Handle("Entity was added");
            _dbSet.AddAsync(entity);
            await SaveChanges();
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        try
        {
            _notifier.Handle($"Entity with id {entity.Id} was updated");
            _dbSet.Update(entity);
            await SaveChanges();
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<TEntity> DeleteAsync(Guid id)
    {
        try
        {
            _notifier.Handle($"Entity with id {id} was deleted");
            var entity = await GetByIdAsync(id);
            _dbSet.Remove(entity);
            await SaveChanges();
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<int> SaveChanges()
    {
        try
        {
            _notifier.Handle("Changes were saved");
            return await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
