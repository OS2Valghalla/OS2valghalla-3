namespace Valghalla.Application.DigitalPost
{
    public sealed record DigitalPostMessage
    {
        public string Cpr { get; init; } = null!;
        public string Label { get; init; } = null!;
        public string Content { get; init; } = null!;
        public IEnumerable<(Stream, string)> Attachments { get; init; } = Enumerable.Empty<(Stream, string)>();
    }
}
