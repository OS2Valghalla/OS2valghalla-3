using Valghalla.Application.Auth;

namespace Valghalla.Internal.API.Auth
{
    internal class UserTokenConfigurator : IUserTokenConfigurator
    {
        public string CookieName => "ValghallaInternalTokenKey";

        public bool Renewable => true;
    }
}
