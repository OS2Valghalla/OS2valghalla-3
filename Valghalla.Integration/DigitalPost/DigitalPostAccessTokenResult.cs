using System.Text.Json.Serialization;

namespace Valghalla.Integration.DigitalPost
{
    internal sealed record DigitalPostAccessTokenResult
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; } = null!;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; init; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; init; } = null!;
    }
}
