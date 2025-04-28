using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using bibliotecaPDF.Services.Interfaces;
using StackExchange.Redis;

namespace bibliotecaPDF.Services;

public class RedisCacheService : IRedisCacheService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IDatabase _database;
    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _database = _connectionMultiplexer.GetDatabase();
    }
    public async Task<bool> SetValueAsync(string key, object value)
    {
        if (value is string)
        {
            return await _database.StringSetAsync(key, value.ToString());
        }
        else
        {
            var jsonValue = JsonSerializer.Serialize(value);
            return await _database.StringSetAsync(key, jsonValue);
        }
    }
    public async Task<string?> GetValueAsync(string key)
    {
        return await _database.StringGetAsync(key);
    }
}
