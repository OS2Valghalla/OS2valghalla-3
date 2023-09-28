namespace Valghalla.Application.Communication
{
    public class CommunicationLogMessageType
    {
        public static readonly CommunicationLogMessageType DigitalPost = new(1, "shared.communication_log.message_type.digital_post");
        public static readonly CommunicationLogMessageType Email = new(2, "shared.communication_log.message_type.email");
        public static readonly CommunicationLogMessageType Sms = new(3, "shared.communication_log.message_type.sms");

        public int Value { get; init; }
        public string Text { get; init; }

        private CommunicationLogMessageType(int value, string text) => (Value, Text) = (value, text);

        public static CommunicationLogMessageType Parse(int value)
        {
            if (value == DigitalPost.Value) return DigitalPost;
            if (value == Email.Value) return Email;
            if (value == Sms.Value) return Sms;

            return new(value, string.Empty);
        }

        public static bool IsUnknownType(int value) =>
            value != DigitalPost.Value &&
            value != Email.Value &&
            value != Sms.Value;
    }
}
