using Valghalla.Application.Auth;

namespace Valghalla.Internal.API.Auth
{
    internal class CookieResolver : ICookieResolver
    {
        public string GetCookieName() => "ValghallaInternalTokenKey";
    }
}
