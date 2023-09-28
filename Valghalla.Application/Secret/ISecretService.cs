using Valghalla.Application.CPR;
using Valghalla.Application.Queue;
using Valghalla.Application.SMS;
using Valghalla.Application.Mail;
using Valghalla.Application.Tenant;
using Valghalla.Application.DigitalPost;

namespace Valghalla.Application.Secret
{
    public interface ISecretService
    {
        void ClearCache();
        Task<CprConfiguration> GetCprConfigurationAsync(CancellationToken cancellationToken);
        Task<TextMessageConfiguration> GetTextMessageConfigurationAsync(CancellationToken cancellationToken);
        Task<MailMessageConfiguration> GetMailMessageConfigurationAsync(CancellationToken cancellationToken);
        Task<DigitalPostConfiguration> GetDigitalPostConfigurationAsync(CancellationToken cancellationToken);
        Task<QueueConfiguration> GetQueueConfigurationAsync(CancellationToken cancellationToken);
        Task<IEnumerable<TenantContext>> GetTenantConfigurationsAsync(CancellationToken cancellationToken);
        Task<IDictionary<string, string>> GetKomkodDictionaryAsync(CancellationToken cancellationToken);
    }
}
