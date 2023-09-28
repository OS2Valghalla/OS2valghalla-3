namespace Valghalla.Application.Exceptions
{
    public class TextMessageException : Exception
    {
        public TextMessageException(string message) : base(message) { }
    }

    public class TextMessageSenderCharactersInvalidException : TextMessageException
    {
        public TextMessageSenderCharactersInvalidException() : base("shared.error.text_message.sender_invalid_characters") { }
    }

    public class TextMessageSenderLengthInvalidException: TextMessageException
    {
        public TextMessageSenderLengthInvalidException() : base("shared.error.text_message.sender_invalid_length") { }
    }

    public class TextMessageRecipientsLengthInvalidException : TextMessageException
    {
        public TextMessageRecipientsLengthInvalidException() : base("shared.error.text_message.recipients_invalid_length") { }
    }
}
