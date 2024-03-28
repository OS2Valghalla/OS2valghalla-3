namespace Valghalla.Application.Auth
{
    public interface IUserTokenConfigurator
    {
        string CookieName { get; }
        bool Renewable { get; }
    }
}
