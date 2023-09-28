namespace Valghalla.Application.Communication
{
    public class CommunicationLogSendType
    {
        public static readonly CommunicationLogSendType Manual = new(1, "shared.communication_log.send_type.manual");
        public static readonly CommunicationLogSendType Automatic = new(2, "shared.communication_log.send_type.automatic");

        public int Value { get; init; }
        public string Text { get; init; }

        private CommunicationLogSendType(int value, string text) => (Value, Text) = (value, text);

        public static CommunicationLogSendType Parse(int value)
        {
            if (value == Manual.Value) return Manual;
            if (value == Automatic.Value) return Automatic;

            return new(value, string.Empty);
        }

        public static bool IsUnknownType(int value) =>
            value != Manual.Value &&
            value != Automatic.Value;
    }
}
