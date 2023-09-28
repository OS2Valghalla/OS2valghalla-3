namespace Valghalla.Application.Tenant
{
    public interface ITenantContextProvider
    {
        TenantContext CurrentTenant { get; }
    }
}
