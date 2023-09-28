namespace Valghalla.Application.Queue
{
    public interface IQueueMessage { }

    public record LoggedQueueMessage<T> : QueueMessage<T>
    {
        public Guid LogId { get; init; }

        public LoggedQueueMessage(string targetDomain, T data, Guid logId) : base(targetDomain, data)
        {
            LogId = logId;
        }
    }

    public record QueueMessage<T> : QueueMessage
    {
        public T Data { get; init; }

        public QueueMessage(string targetDomain, T data): base(targetDomain) => Data = data;
    }

    public record QueueMessage : IQueueMessage
    {
        public string TargetDomain { get; init; }

        public QueueMessage(string targetDomain) => TargetDomain = targetDomain;
    }
}
