using Application.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services;

public class MemeCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;

    public MemeCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public void Remove(string cacheKey)
    {
        _memoryCache.Remove(cacheKey);
    }
    public void Set<T>(string cacheKey, T value, TimeSpan timespan)
    {
        _memoryCache.Set(cacheKey, value, timespan);
    }
    public bool TryGet<T>(string cacheKey, out T? value)
    {
        if (_memoryCache.TryGetValue(cacheKey, out T? cachedObject))
        {
            value = cachedObject;
            return true;
        }
        value = default;
        return false;
    }
}
