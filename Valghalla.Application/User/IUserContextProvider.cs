namespace Valghalla.Application.User
{
    public interface IUserContextProvider
    {
        UserContext CurrentUser { get; }
        bool Registered { get; }
    }
}
