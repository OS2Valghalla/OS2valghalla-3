using System.Text.RegularExpressions;
using Valghalla.Application.Exceptions;

namespace Valghalla.Application.SMS
{
    public partial class TextMessage
    {
        [GeneratedRegex("^([a-zA-Z0-9])+$")]
        private static partial Regex ValidSenderRegex();

        public string Sender { get; set; } = null!;
        public string Message { get; set; } = null!;
        public IEnumerable<string> Recipients { get; set; } = null!;

        public TextMessage(string sender, string message, IEnumerable<string> recipients)
        {
            Sender = sender;
            Message = message;
            Recipients = recipients;

            if (string.IsNullOrEmpty(sender))
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (!ValidSenderRegex().Match(sender).Success)
            {
                throw new TextMessageSenderCharactersInvalidException();
            }

            if (sender.Length > 11)
            {
                throw new TextMessageSenderLengthInvalidException();
            }

            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (recipients == null)
            {
                throw new ArgumentNullException(nameof(recipients));
            }

            if (!recipients.Any())
            {
                throw new TextMessageRecipientsLengthInvalidException();
            }
        }
    }
}
