namespace Valghalla.Application.Abstractions.Messaging
{
    public sealed record Confirmation
    {
        public string? Title { get; init; }
        public string[]? Messages { get; init; }
    }
}
