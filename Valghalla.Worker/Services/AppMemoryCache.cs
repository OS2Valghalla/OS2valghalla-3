using Microsoft.Extensions.Caching.Memory;
using Valghalla.Application.Cache;

namespace Valghalla.Worker.Services
{
    internal class AppMemoryCache : IAppMemoryCache
    {
        private readonly IMemoryCache memoryCache;

        public AppMemoryCache(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public TItem? GetOrCreate<TItem>(string key, Func<TItem> factory)
        {
            return memoryCache.GetOrCreate(key, cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return factory();
            });
        }

        public Task<TItem?> GetOrCreateAsync<TItem>(string key, Func<Task<TItem>> factory)
        {
            return memoryCache.GetOrCreateAsync(key, cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return factory();
            });
        }

        public void Remove(string key)
        {
            memoryCache.Remove(key);
        }
    }
}
