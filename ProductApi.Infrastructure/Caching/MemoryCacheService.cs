using Microsoft.Extensions.Caching.Memory;
using ProductApi.Application.Interfaces;

namespace ProductApi.Infrastructure.Caching;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    
    public MemoryCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public T? Get<T>(string key)
    {
        return _memoryCache.TryGetValue(key, out T? value)
            ? value
            : default;
    }

    public void Set<T>(string key, T value, TimeSpan duration)
    {
        _memoryCache.Set(key, value, duration);
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);
    }
}