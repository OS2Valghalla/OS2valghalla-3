using Valghalla.Application.Cache;

namespace Valghalla.Internal.Application.Tests
{
    internal class MockTenantMemoryCache : ITenantMemoryCache
    {
        public TItem? GetOrCreate<TItem>(string key, Func<TItem> factory)
        {
            return factory();
        }

        public async Task<TItem?> GetOrCreateAsync<TItem>(string key, Func<Task<TItem>> factory)
        {
            return await factory();
        }

        public void Remove(string key)
        {
            
        }
    }
}
