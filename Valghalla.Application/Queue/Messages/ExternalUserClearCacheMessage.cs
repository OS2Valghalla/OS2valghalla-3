namespace Valghalla.Application.Queue.Messages
{
    public sealed record ExternalUserClearCacheMessage
    {
        public IEnumerable<string> CprNumbers { get; init; } = Enumerable.Empty<string>();
    }
}
