using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Valghalla.Application.CPR;
using Valghalla.Application.DigitalPost;
using Valghalla.Application.Mail;
using Valghalla.Application.Queue;
using Valghalla.Application.Secret;
using Valghalla.Application.SMS;
using Valghalla.Application.Tenant;

namespace Valghalla.Integration.Secret
{
    internal class SecretService : ISecretService
    {
        private readonly IOptions<SecretConfiguration> options;
        private readonly IMemoryCache memoryCache;

        public SecretService(IOptions<SecretConfiguration> options, IMemoryCache memoryCache)
        {
            this.options = options;
            this.memoryCache = memoryCache;
        }

        public void ClearCache()
        {
            memoryCache.Remove(nameof(CprConfiguration));
            memoryCache.Remove(nameof(QueueConfiguration));
            memoryCache.Remove(nameof(TenantContext));
            memoryCache.Remove(nameof(TextMessageConfiguration));
            memoryCache.Remove(nameof(SecretValue.Komkod));
        }

        public async Task<CprConfiguration> GetCprConfigurationAsync(CancellationToken cancellationToken)
        {
            return (await memoryCache.GetOrCreateAsync(nameof(CprConfiguration), async cacheEntry =>
            {
                var value = await ReadSecretValueAsync(cancellationToken);
                return value.CPRService;
            }))!;
        }

        public async Task<QueueConfiguration> GetQueueConfigurationAsync(CancellationToken cancellationToken)
        {
            return (await memoryCache.GetOrCreateAsync(nameof(QueueConfiguration), async cacheEntry =>
            {
                var value = await ReadSecretValueAsync(cancellationToken);
                return value.Queue;
            }))!;
        }

        public async Task<IEnumerable<TenantContext>> GetTenantConfigurationsAsync(CancellationToken cancellationToken)
        {
            return (await memoryCache.GetOrCreateAsync(nameof(TenantContext), async cacheEntry =>
            {
                var value = await ReadSecretValueAsync(cancellationToken);
                return value.Tenants;
            }))!;
        }

        public async Task<TextMessageConfiguration> GetTextMessageConfigurationAsync(CancellationToken cancellationToken)
        {
            return (await memoryCache.GetOrCreateAsync(nameof(TextMessageConfiguration), async cacheEntry =>
            {
                var value = await ReadSecretValueAsync(cancellationToken);
                return value.SMS;
            }))!;
        }

        public async Task<MailMessageConfiguration> GetMailMessageConfigurationAsync(CancellationToken cancellationToken)
        {
            return (await memoryCache.GetOrCreateAsync(nameof(MailMessageConfiguration), async cacheEntry =>
            {
                var value = await ReadSecretValueAsync(cancellationToken);
                return value.Mail;
            }))!;
        }

        public async Task<DigitalPostConfiguration> GetDigitalPostConfigurationAsync(CancellationToken cancellationToken)
        {
            return (await memoryCache.GetOrCreateAsync(nameof(DigitalPostConfiguration), async cacheEntry =>
            {
                var value = await ReadSecretValueAsync(cancellationToken);
                return value.DigitalPost;
            }))!;
        }

        public async Task<IDictionary<string, string>> GetKomkodDictionaryAsync(CancellationToken cancellationToken)
        {
            return (await memoryCache.GetOrCreateAsync(nameof(SecretValue.Komkod), async cacheEntry =>
            {
                var value = await ReadSecretValueAsync(cancellationToken);
                return value.Komkod;
            }))!;
        }

        private async Task<SecretValue> ReadSecretValueAsync(CancellationToken cancellationToken)
        {
            var path = Path.IsPathRooted(options.Value.Path) ?
                options.Value.Path :
                Path.Combine(AppContext.BaseDirectory, options.Value.Path);

            var jsonString = await File.ReadAllTextAsync(path, cancellationToken);
            return JsonSerializer.Deserialize<SecretValue>(jsonString)!;
        }
    }
}
