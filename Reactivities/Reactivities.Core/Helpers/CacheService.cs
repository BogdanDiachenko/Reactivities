using System.Net.Http.Json;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Reactivities.Core.Helpers;

public class CacheService : ICacheService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public CacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task SetRecordAsync<T>(string key, T value, 
        TimeSpan? absoluteExpiredTime = null)
    {
        var serializedData = JsonConvert.SerializeObject(value);
        var db = _connectionMultiplexer.GetDatabase();
        
        await db.StringSetAsync(key, serializedData, absoluteExpiredTime ?? TimeSpan.FromSeconds(60));
    }

    public async Task<T> GetRecordAsync<T>(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var serializedData = await db.StringGetAsync(key);

        return serializedData == RedisValue.Null
            ? default
            : JsonConvert.DeserializeObject<T>(serializedData);
    }
}