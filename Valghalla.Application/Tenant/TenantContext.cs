namespace Valghalla.Application.Tenant
{
    public sealed record TenantContext
    {
        public string Name { get; init; } = null!;
        public string InternalDomain { get; init; } = null!;
        public string ExternalDomain { get; init; } = null!;
        public string ConnectionString { get; init; } = null!;

        public string? AngularDevServer { get; init; }
    }
}
