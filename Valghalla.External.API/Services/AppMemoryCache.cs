﻿using Microsoft.Extensions.Caching.Memory;
using Valghalla.Application.Cache;
using Valghalla.Application.Tenant;

namespace Valghalla.External.API.Services
{
    internal class AppMemoryCache : IAppMemoryCache
    {
        private readonly IMemoryCache memoryCache;
        private readonly ITenantContextProvider tenantContextProvider;

        public AppMemoryCache(IMemoryCache memoryCache, ITenantContextProvider tenantContextProvider)
        {
            this.memoryCache = memoryCache;
            this.tenantContextProvider = tenantContextProvider;
        }

        public TItem? GetOrCreate<TItem>(string key, Func<TItem> factory)
        {
            return memoryCache.GetOrCreate(BuildKey(key), cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return factory();
            });
        }

        public Task<TItem?> GetOrCreateAsync<TItem>(string key, Func<Task<TItem>> factory)
        {
            return memoryCache.GetOrCreateAsync(BuildKey(key), cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return factory();
            });
        }

        public void Remove(string key)
        {
            memoryCache.Remove(BuildKey(key));
        }

        private string BuildKey(string key) =>
            tenantContextProvider.CurrentTenant != null ?
            tenantContextProvider.CurrentTenant.Name + "/" + key :
            key;
    }
}
