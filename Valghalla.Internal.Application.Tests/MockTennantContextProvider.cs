using Valghalla.Application.Tenant;

namespace Valghalla.Internal.Application.Tests
{
    internal class MockTennantContextProvider : ITenantContextProvider
    {
        public TenantContext CurrentTenant => new()
        {
            Name = "Test",
            ExternalDomain = "Test",
            InternalDomain = "Test",
            AngularDevServer = "Test",
            ConnectionString = "Test",
        };
    }
}
