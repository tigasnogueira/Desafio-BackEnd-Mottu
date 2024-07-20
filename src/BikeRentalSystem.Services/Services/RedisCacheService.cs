﻿using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Infrastructure.Redis;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace BikeRentalSystem.RentalServices.Services;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase _database;

    public RedisCacheService()
    {
        _database = RedisConnection.Connection.GetDatabase();
    }

    public async Task<T> GetCacheValueAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        return value.HasValue ? JsonConvert.DeserializeObject<T>(value) : default;
    }

    public async Task SetCacheValueAsync<T>(string key, T value)
    {
        await _database.StringSetAsync(key, JsonConvert.SerializeObject(value));
    }
}
