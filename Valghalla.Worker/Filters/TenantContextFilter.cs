using MassTransit;
using Valghalla.Application.Queue;
using Valghalla.Application.Secret;
using Valghalla.Worker.Auth;

namespace Valghalla.Worker.Filters
{
    internal class TenantContextFilter<T> : IFilter<ConsumeContext<T>> where T : QueueMessage
    {
        private readonly ISecretService secretService;
        private readonly IHostEnvironment environment;
        private readonly TenantContextInternalProvider tenantContextProvider;

        public TenantContextFilter(
            ISecretService secretService,
            IHostEnvironment environment,
            TenantContextInternalProvider tenantContextProvider)
        {
            this.secretService = secretService;
            this.environment = environment;
            this.tenantContextProvider = tenantContextProvider;
        }

        public void Probe(ProbeContext context)
        {
            
        }

        public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
        {
            var domain = environment.IsDevelopment() ? "localhost" : context.Message.TargetDomain;
            var tenants = await secretService.GetTenantConfigurationsAsync(context.CancellationToken);

            if (!tenants.Any())
            {
                throw new Exception("Could not get database tenant information");
            }

            var tenantContext = tenants.SingleOrDefault(i =>
                i.InternalDomain.Equals(domain, StringComparison.OrdinalIgnoreCase) ||
                i.ExternalDomain.Equals(domain, StringComparison.OrdinalIgnoreCase));

            if (tenantContext == null)
            {
                throw new Exception("Could not find database tenant information matching domain: " + domain);
            }

            tenantContextProvider.SetTenantContext(tenantContext);

            await next.Send(context);
        }
    }
}
