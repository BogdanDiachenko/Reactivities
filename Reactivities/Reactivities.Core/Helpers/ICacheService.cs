namespace Reactivities.Core.Helpers;

public interface ICacheService
{
    Task<T> GetRecordAsync<T>(string key);

    Task SetRecordAsync<T>(string key, T value,
        TimeSpan? absoluteExpiredTime = null);
}