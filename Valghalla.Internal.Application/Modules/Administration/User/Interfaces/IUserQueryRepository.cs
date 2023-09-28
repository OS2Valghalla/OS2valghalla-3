using Valghalla.Internal.Application.Modules.Administration.User.Commands;
using Valghalla.Internal.Application.Modules.Administration.User.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.User.Interfaces
{
    public interface IUserQueryRepository
    {
        Task<GetUsersResponse> GetUsersAsync(CancellationToken cancellationToken);
        Task<bool> CheckIfUserExistsAsync(CreateUserCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfUserCanBeDeletedAsync(DeleteUserCommand command, CancellationToken cancellationToken);
    }
}
