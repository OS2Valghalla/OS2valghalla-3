using Microsoft.AspNetCore.Http;

namespace Valghalla.Application.Mail
{
    public sealed record MailDataWithAttachments
    {
        // Receiver
        public IEnumerable<string> To { get; init; } = Enumerable.Empty<string>();
        public IEnumerable<string> Bcc { get; init; } = Enumerable.Empty<string>();
        public IEnumerable<string> Cc { get; init; } = Enumerable.Empty<string>();

        // Sender
        public string? ReplyTo { get; init; }
        public string? ReplyToName { get; init; }

        // Content
        public string? Subject { get; init; }
        public string? Body { get; init; }
        public IEnumerable<(Stream, string)> Attachments { get; init; } = Enumerable.Empty<(Stream, string)>();
    }
}
