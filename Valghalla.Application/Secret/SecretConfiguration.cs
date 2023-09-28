namespace Valghalla.Application.Secret
{
    public sealed record SecretConfiguration
    {
        public string Path { get; init; } = null!;
    }
}
