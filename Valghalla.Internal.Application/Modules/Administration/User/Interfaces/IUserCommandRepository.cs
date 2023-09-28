using Valghalla.Internal.Application.Modules.Administration.User.Commands;

namespace Valghalla.Internal.Application.Modules.Administration.User.Interfaces
{
    public interface IUserCommandRepository
    {
        Task<Guid> CreateUserAsync(CreateUserCommand command, CancellationToken cancellationToken);
        Task DeleteUserAsync(DeleteUserCommand command, CancellationToken cancellationToken);
        Task UpdateUserAsync(UpdateUserCommand command, CancellationToken cancellationToken);
    }
}
