namespace Valghalla.Application.Mail
{
    public sealed record MailMessageConfiguration
    {
        public string SMPT { get; init; } = string.Empty;
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public int Port { get; init; }
        public bool UseSSL { get; init; }
        public bool UseStartTls { get; init; }
    }
}