using Microsoft.Extensions.Caching.Memory;
using Valghalla.Application.Cache;

namespace Valghalla.Internal.Application.Tests
{
    internal class MockTenantMemoryCache : ITenantMemoryCache
    {
        public TItem? GetOrCreate<TItem>(string key, Func<TItem> factory)
        {
            return factory();
        }

        public TItem? GetOrCreate<TItem>(string key, Func<ICacheEntry, TItem> factory)
        {
            return factory(null);
        }

        public async Task<TItem?> GetOrCreateAsync<TItem>(string key, Func<Task<TItem>> factory)
        {
            return await factory();
        }

        public Task<TItem?> GetOrCreateAsync<TItem>(string key, Func<ICacheEntry, Task<TItem>> factory)
        {
            return factory(null);
        }

        public void Remove(string key)
        {
            
        }
    }
}
