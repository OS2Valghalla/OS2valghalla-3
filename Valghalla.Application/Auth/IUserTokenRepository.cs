namespace Valghalla.Application.Auth
{
    public interface IUserTokenRepository
    {
        Task<IEnumerable<UserToken>> GetUserTokensAsync(Guid identifier, CancellationToken cancellationToken);
        Task AddUserTokenAsync(UserToken token, CancellationToken cancellationToken);
        Task RemoveExpiredUserTokensAsync(CancellationToken cancellationToken);
    }
}
