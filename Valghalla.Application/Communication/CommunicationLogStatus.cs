namespace Valghalla.Application.Communication
{
    public class CommunicationLogStatus
    {
        public static readonly CommunicationLogStatus Awaiting = new(1, "shared.communication_log.status.awaiting");
        public static readonly CommunicationLogStatus Success = new(2, "shared.communication_log.status.success");
        public static readonly CommunicationLogStatus Error = new(3, "shared.communication_log.status.error");

        public int Value { get; init; }
        public string Text { get; init; }

        private CommunicationLogStatus(int value, string text) => (Value, Text) = (value, text);

        public static CommunicationLogStatus Parse(int value)
        {
            if (value == Awaiting.Value) return Awaiting;
            if (value == Success.Value) return Success;
            if (value == Error.Value) return Error;

            return new(value, string.Empty);
        }

        public static bool IsUnknownType(int value) =>
            value != Awaiting.Value &&
            value != Success.Value &&
            value != Error.Value;
    }
}
