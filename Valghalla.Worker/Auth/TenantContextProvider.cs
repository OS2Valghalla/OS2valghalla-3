using Valghalla.Application.Tenant;

namespace Valghalla.Worker.Auth
{
    internal class TenantContextProvider : ITenantContextProvider
    {
        public TenantContext CurrentTenant
        {
            get
            {
                return internalProvider.CurrentTenant;
            }
        }

        private readonly TenantContextInternalProvider internalProvider;

        public TenantContextProvider(TenantContextInternalProvider internalProvider)
        {
            this.internalProvider = internalProvider;
        }
    }

    internal class TenantContextInternalProvider
    {
        public TenantContext CurrentTenant { get; private set; } = null!;

        public void SetTenantContext(TenantContext value)
        {
            if (CurrentTenant == null)
            {
                CurrentTenant = value;
            }
        }
    }
}
