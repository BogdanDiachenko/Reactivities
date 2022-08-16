using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Reactivities.Core.Helpers;

public static class DistributedCacheExtensions
{
    public static async Task SetRecordAsync<T>(this IDistributedCache cache,
        string recordId, T data, TimeSpan? absoluteExpiredTime = null,
        TimeSpan? slidingExpiredTime = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpiredTime ?? TimeSpan.FromSeconds(60),
            SlidingExpiration = slidingExpiredTime ?? TimeSpan.FromSeconds(30)
        };

        var serializedData = JsonConvert.SerializeObject(data);

        await cache.SetStringAsync(recordId, serializedData, options);
    }

    public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
    {
        var serializedData = await cache.GetStringAsync(recordId);

        return serializedData == null
            ? default
            : JsonConvert.DeserializeObject<T>(serializedData);
    }
}