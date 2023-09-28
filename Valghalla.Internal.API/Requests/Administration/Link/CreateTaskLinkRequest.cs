namespace Valghalla.Internal.API.Requests.Administration.Link
{
    public sealed record CreateTaskLinkRequest
    {
        public Guid ElectionId { get; init; }
        public Guid TaskId { get; init; }
    }
}
