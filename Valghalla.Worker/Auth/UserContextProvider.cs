using Valghalla.Application.User;

namespace Valghalla.Worker.Auth
{
    internal class UserContextProvider : IUserContextProvider
    {
        public UserContext CurrentUser => UserContext.App;

        public bool Registered => true;
    }
}
