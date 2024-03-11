using Valghalla.Application.Cache;
using Valghalla.Application.Configuration.Interfaces;

namespace Valghalla.Integration.Configuration
{
    internal class ConfigurationContainer
    {
        private readonly ITenantMemoryCache tenantMemoryCache;
        private readonly IConfigurationRepository configurationQueryRepository;

        public ConfigurationContainer(ITenantMemoryCache tenantMemoryCache, IConfigurationRepository configurationQueryRepository)
        {
            this.tenantMemoryCache = tenantMemoryCache;
            this.configurationQueryRepository = configurationQueryRepository;
        }

        public object GetConfiguration(Type type)
        {
            var configuration = tenantMemoryCache.GetOrCreate(type.FullName!, () =>
            {
                return configurationQueryRepository.GetConfigurationAsync(type, default).Result;
            });

            return configuration!;
        }

        public void ClearConfiguration(Type type)
        {
            tenantMemoryCache.Remove(type.FullName!);
        }
    }
}
