using Valghalla.Application.Auth;

namespace Valghalla.External.API.Auth
{
    internal class UserTokenConfigurator : IUserTokenConfigurator
    {
        public string CookieName => "ValghallaExternalTokenKey";

        public bool Renewable => false;
    }
}
