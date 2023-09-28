namespace Valghalla.Application.Queue
{
    public sealed record QueueConfiguration
    {
        public string Host { get; init; } = string.Empty;
        public string VirtualHost { get; init; } = string.Empty;
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public ushort Port { get; init; }
    }
}
