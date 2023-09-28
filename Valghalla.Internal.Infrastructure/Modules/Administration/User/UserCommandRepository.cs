using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.User.Commands;
using Valghalla.Internal.Application.Modules.Administration.User.Interfaces;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.User
{
    internal class UserCommandRepository : IUserCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<UserEntity> users;

        public UserCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            users = dataContext.Set<UserEntity>();
        }

        public async Task<Guid> CreateUserAsync(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var entity = new UserEntity()
            {
                Id = Guid.NewGuid(),
                RoleId = command.RoleId,
                Name = command.Name,
                Cvr = command.Cvr,
                Serial = command.Serial
            };

            await users.AddAsync(entity, cancellationToken);
            await dataContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        public async Task UpdateUserAsync(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var entity = await users.SingleAsync(x => x.Id == command.Id, cancellationToken);

            entity.RoleId = command.RoleId;

            users.Update(entity);

            await dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteUserAsync(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            var entity = await users.SingleAsync(x => x.Id == command.Id, cancellationToken);

            users.Remove(entity);

            await dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
