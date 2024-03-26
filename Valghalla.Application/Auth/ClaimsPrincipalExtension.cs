using System.Security.Claims;

namespace Valghalla.Application.Auth
{
    public static class ClaimsPrincipalExtension
    {
        public const string Name = "valghalla_Name";
        public const string Cpr = "valghalla_Cpr";
        public const string Cvr = "valghalla_Cvr";
        public const string Serial = "valghalla_Serial";

        public const string Saml2NameIdFormat = "http://schemas.itfoxtec.com/ws/2014/02/identity/claims/saml2nameidformat";
        public const string Saml2NameId = "http://schemas.itfoxtec.com/ws/2014/02/identity/claims/saml2nameid";
        public const string Saml2SessionIndex = "http://schemas.itfoxtec.com/ws/2014/02/identity/claims/saml2sessionindex";

        public static string? GetName(this ClaimsPrincipal principal) => principal.FindFirstValue(Name);
        public static string? GetCpr(this ClaimsPrincipal principal) => principal.FindFirstValue(Cpr);
        public static string? GetCvr(this ClaimsPrincipal principal) => principal.FindFirstValue(Cvr);
        public static string? GetSerial(this ClaimsPrincipal principal) => principal.FindFirstValue(Serial);
        public static string? GetSaml2NameIdFormat(this ClaimsPrincipal principal) => principal.FindFirstValue(Saml2NameIdFormat);
        public static string? GetSaml2NameId(this ClaimsPrincipal principal) => principal.FindFirstValue(Saml2NameId);
        public static string? GetSaml2SessionIndex(this ClaimsPrincipal principal) => principal.FindFirstValue(Saml2SessionIndex);
    }
}
