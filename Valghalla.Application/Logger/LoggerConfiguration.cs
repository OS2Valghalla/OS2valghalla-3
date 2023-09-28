namespace Valghalla.Application.Logger
{
    public sealed record LoggerConfiguration
    {
        public bool ApiLogging { get; init; }
        public LoggerSerilogConfiguration Serilog { get; init; } = null!;
    }

    public sealed record LoggerSerilogConfiguration
    {
        public string TenantPath { get; init; } = null!;
        public string SystemPath { get; init; } = null!;
        public string FileName { get; init; } = null!;
        public string RollingInterval { get; init; } = null!;
        public string OutputTemplate { get; init; } = null!;
    }
}
