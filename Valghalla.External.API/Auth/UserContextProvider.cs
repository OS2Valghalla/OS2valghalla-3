using Valghalla.Application.User;

namespace Valghalla.External.API.Auth
{
    internal class UserContextProvider : IUserContextProvider
    {
        public UserContext CurrentUser
        {
            get
            {
                return internalProvider.CurrentUser;
            }
        }

        public bool Registered
        {
            get
            {
                return
                    CurrentUser != null &&
                    !string.IsNullOrEmpty(CurrentUser.Cpr);
            }
        }

        private readonly UserContextInternalProvider internalProvider;

        public UserContextProvider(UserContextInternalProvider internalProvider)
        {
            this.internalProvider = internalProvider;
        }
    }

    public class UserContextInternalProvider
    {
        public UserContext CurrentUser { get; private set; } = null!;

        /// <summary>
        /// USE WITH CAUTION. <br/>
        /// There should be only <see cref="UserContextHandlingMiddleware"/> using this method <br/>
        /// The exception case is when <see cref="RegistrationCommandController"/> using this method for <see cref="RegistrationCommandController.VerifyUserActivation(string, string, CancellationToken)"/>
        /// </summary>
        /// <param name="value"></param>
        public void SetUserContext(UserContext value)
        {
            if (CurrentUser == null)
            {
                CurrentUser = value;
            }
        }
    }
}
