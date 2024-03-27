using Valghalla.Application.Auth;

namespace Valghalla.External.API.Auth
{
    internal class CookieResolver : ICookieResolver
    {
        public string GetCookieName() => "ValghallaExternalTokenKey";
    }
}
