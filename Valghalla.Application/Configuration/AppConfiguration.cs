using Valghalla.Application.Configuration.Interfaces;

namespace Valghalla.Application.Configuration
{
    public class AppConfiguration : IConfiguration
    {
        public string Cvr { get; init; } = null!;
        public string DigitalPostCvr { get; init; } = null!;
        public string DigitalPostSender { get; init; } = null!;
        public string Komkod { get; init; } = null!;
        public string SmsSender { get; init; } = null!;
        public string MailSender { get; init; } = null!;
        public string MailAddress { get; init; } = null!;
        public string TaskReminderDay { get; init; } = null!;
    }
}
