using Valghalla.Application.Cache;
using Valghalla.Application.Configuration.Interfaces;

namespace Valghalla.Integration.Configuration
{
    internal class ConfigurationContainer
    {
        private readonly IAppMemoryCache appMemoryCache;
        private readonly IConfigurationRepository configurationQueryRepository;

        public ConfigurationContainer(IAppMemoryCache appMemoryCache, IConfigurationRepository configurationQueryRepository)
        {
            this.appMemoryCache = appMemoryCache;
            this.configurationQueryRepository = configurationQueryRepository;
        }

        public object GetConfiguration(Type type)
        {
            var configuration = appMemoryCache.GetOrCreate(type.FullName!, () =>
            {
                return configurationQueryRepository.GetConfigurationAsync(type, default).Result;
            });

            return configuration!;
        }

        public void ClearConfiguration(Type type)
        {
            appMemoryCache.Remove(type.FullName!);
        }
    }
}
