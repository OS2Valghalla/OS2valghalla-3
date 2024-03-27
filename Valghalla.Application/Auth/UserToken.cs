using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Valghalla.Application.Auth
{
    public class UserToken
    {
        private static readonly int LIFETIME_PERIOD_MINUTES = 60;
        private static readonly int RENEWABLE_AFTER_MINUTES = 30;

        public class TokenKey
        {
            public Guid Identifier { get; init; }
            public string Code { get; init; } = null!;
        }

        public class TokenValue
        {
            public string? Name { get; init; }
            public string? Cvr { get; init; }
            public string? Cpr { get; init; }
            public string? Serial { get; init; }
            public string? Saml2NameIdFormat { get; init; }
            public string? Saml2NameId { get; init; }
            public string? Saml2SessionIndex { get; init; }
        }

        public TokenKey Key { get; init; } = new();
        public TokenValue Value { get; init; } = new();
        public DateTime CreatedAt { get; init; }
        public DateTime ExpiredAt { get; init; }
        public DateTime RefreshedAfter => CreatedAt.AddMinutes(RENEWABLE_AFTER_MINUTES);

        public bool Valid => DateTime.UtcNow < ExpiredAt;
        public bool Renewable => Valid && DateTime.UtcNow > RefreshedAfter;

        public ClaimsPrincipal ToClaimsPrincipal()
        {
            var claims = new List<Claim> {
                new(ClaimTypes.Name, Value.Saml2NameId ?? Key.Identifier.ToString()),
                new(ClaimTypes.NameIdentifier, Value.Saml2NameId ?? Key.Identifier.ToString()),
                new(ClaimsPrincipalExtension.Name, Value.Name ?? string.Empty),
                new(ClaimsPrincipalExtension.Cvr, Value.Cvr ?? string.Empty),
                new(ClaimsPrincipalExtension.Cpr, Value.Cpr ?? string.Empty),
                new(ClaimsPrincipalExtension.Serial, Value.Serial ?? string.Empty),
                new(ClaimsPrincipalExtension.Saml2NameIdFormat, Value.Saml2NameIdFormat ?? string.Empty),
                new(ClaimsPrincipalExtension.Saml2NameId, Value.Saml2NameId ?? string.Empty),
                new(ClaimsPrincipalExtension.Saml2SessionIndex, Value.Saml2SessionIndex ?? string.Empty),
            };

            var identity = new ClaimsIdentity(claims, Constants.Authentication.Scheme);
            return new(identity);
        }

        public static UserToken CreateToken(ClaimsPrincipal principal) => new()
        {
            Key = new TokenKey()
            {
                Identifier = Guid.NewGuid(),
                Code = GenerateCode()
            },
            Value = new TokenValue()
            {
                Name = principal.GetName(),
                Cvr = principal.GetCvr(),
                Cpr = principal.GetCpr(),
                Serial = principal.GetSerial(),
                Saml2NameIdFormat = principal.GetSaml2NameIdFormat(),
                Saml2NameId = principal.GetSaml2NameId(),
                Saml2SessionIndex = principal.GetSaml2SessionIndex(),
            },
            CreatedAt = DateTime.UtcNow,
            ExpiredAt = DateTime.UtcNow.AddMinutes(LIFETIME_PERIOD_MINUTES)
        };

        public static string Encode(TokenKey value)
        {
            return Base64UrlEncoder.Encode(JsonSerializer.Serialize(value));
        }

        public static TokenKey? Decode(string value)
        {
            return JsonSerializer.Deserialize<TokenKey>(Base64UrlEncoder.Decode(value));
        }

        private static string GenerateCode()
        {
            var salt = Guid.NewGuid().ToString();
            var hashObject = new HMACSHA512(Encoding.UTF8.GetBytes(Constants.Authentication.Cookie));
            var signature = hashObject.ComputeHash(Encoding.UTF8.GetBytes(salt));
            var encodedSignature = Convert.ToBase64String(signature)
                .Replace("+", string.Empty)
                .Replace("=", string.Empty)
                .Replace("/", string.Empty);

            return encodedSignature;
        }
    }
}
