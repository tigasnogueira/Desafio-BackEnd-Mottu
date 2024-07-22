namespace BikeRentalSystem.Core.Interfaces.Services;

public interface IRedisCacheService
{
    Task<T> GetCacheValueAsync<T>(string key);
    Task SetCacheValueAsync<T>(string key, T value);
}
