namespace Valghalla.Application.Cache
{
    public interface ITenantMemoryCache
    {
        TItem? GetOrCreate<TItem>(string key, Func<TItem> factory);
        Task<TItem?> GetOrCreateAsync<TItem>(string key, Func<Task<TItem>> factory);
        void Remove(string key);
    }
}
