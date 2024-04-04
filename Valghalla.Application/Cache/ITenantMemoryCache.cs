using Microsoft.Extensions.Caching.Memory;

namespace Valghalla.Application.Cache
{
    public interface ITenantMemoryCache
    {
        TItem? GetOrCreate<TItem>(string key, Func<TItem> factory);
        TItem? GetOrCreate<TItem>(string key, Func<ICacheEntry, TItem> factory);
        Task<TItem?> GetOrCreateAsync<TItem>(string key, Func<Task<TItem>> factory);
        Task<TItem?> GetOrCreateAsync<TItem>(string key, Func<ICacheEntry, Task<TItem>> factory);
        void Remove(string key);
    }
}
