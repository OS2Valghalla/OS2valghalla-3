namespace Valghalla.Integration.DigitalPost
{
    internal sealed record DigitalPostAccessToken
    {
        public string Value { get; init; } = null!;
        public string Type { get; init; } = null!;
        public TimeSpan ExpiresIn { get; init; }
        public DateTime RetrievedAtUtc { get; init; }

        public bool IsValid()
        {
            return (RetrievedAtUtc + ExpiresIn) > DateTime.UtcNow;
        }
    }
}
