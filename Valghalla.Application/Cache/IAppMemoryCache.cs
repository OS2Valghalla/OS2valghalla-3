namespace Valghalla.Application.Cache
{
    public interface IAppMemoryCache
    {
        TItem? GetOrCreate<TItem>(string key, Func<TItem> factory);
        Task<TItem?> GetOrCreateAsync<TItem>(string key, Func<Task<TItem>> factory);
        void Remove(string key);
    }
}
