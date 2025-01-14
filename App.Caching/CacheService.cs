using App.Application.Contracts.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace App.Caching
{
    public class CacheService(IMemoryCache memoryCache) : ICacheService
    {
        public Task<T?> GetAsync<T>(string key) => Task.FromResult(memoryCache.TryGetValue(key, out T cacheItem) ? cacheItem : default(T));

        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(5)
            };

            memoryCache.Set(key, value, expiration ?? TimeSpan.FromMinutes(5));

            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            memoryCache.Remove(key);

            return Task.CompletedTask;
        }
    }
}
