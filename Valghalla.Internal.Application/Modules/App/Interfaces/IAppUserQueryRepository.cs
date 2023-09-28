using Valghalla.Internal.Application.Modules.App.Queries;
using Valghalla.Internal.Application.Modules.App.Responses;

namespace Valghalla.Internal.Application.Modules.App.Interfaces
{
    public interface IAppUserQueryRepository
    {
        Task<UserResponse?> GetUserAsync(GetInternalUserQuery query, CancellationToken cancellationToken);
    }
}
