using Valghalla.Application.CPR;
using Valghalla.Application.Queue;
using Valghalla.Application.SMS;
using Valghalla.Application.Mail;
using Valghalla.Application.Tenant;
using Valghalla.Application.DigitalPost;

namespace Valghalla.Application.Secret
{
    public sealed record SecretValue
    {
        public CprConfiguration CPRService { get; init; } = null!;
        public TextMessageConfiguration SMS { get; init; } = null!;
        public MailMessageConfiguration Mail { get; init; } = null!;
        public DigitalPostConfiguration DigitalPost { get; init; } = null!;
        public QueueConfiguration Queue { get; init; } = null!;
        public IEnumerable<TenantContext> Tenants { get; init; } = Enumerable.Empty<TenantContext>();
        public IDictionary<string, string> Komkod { get; init; } = new Dictionary<string, string>();
    }
}
