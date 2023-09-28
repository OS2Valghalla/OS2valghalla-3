using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application.User;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.User.Commands;
using Valghalla.Internal.Application.Modules.Administration.User.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.User.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.User
{
    internal class UserQueryRepository : IUserQueryRepository
    {
        private readonly IQueryable<UserEntity> users;
        private readonly IMapper mapper;

        public UserQueryRepository(DataContext dataContext, IMapper mapper)
        {
            users = dataContext.Set<UserEntity>().AsNoTracking();
            this.mapper = mapper;
        }

        public async Task<GetUsersResponse> GetUsersAsync(CancellationToken cancellationToken)
        {
            var total = await users
                .Where(i =>
                    i.RoleId != Role.Participant.Id &&
                    i.RoleId != Role.TeamResponsible.Id &&
                    i.RoleId != Role.WorkLocationResponsible.Id)
                .CountAsync(cancellationToken);

            var entities = await users
                .Where(i =>
                    i.RoleId != Role.Participant.Id &&
                    i.RoleId != Role.TeamResponsible.Id &&
                    i.RoleId != Role.WorkLocationResponsible.Id)
                .ToListAsync(cancellationToken);

            var items = mapper.Map<List<GetUsersItem>>(entities).OrderBy(x => x.Name).ToList();

            return new()
            {
                Items = items,
                Total = total
            };
        }

        public async Task<bool> CheckIfUserExistsAsync(CreateUserCommand command, CancellationToken cancellationToken)
        {
            return await users
                .Where(i => i.Cvr == command.Cvr && i.Serial == command.Serial)
                .AnyAsync(cancellationToken);
        }

        public async Task<bool> CheckIfUserCanBeDeletedAsync(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            var exist = await users.AnyAsync(x => x.Id == command.Id && x.Participant != null, cancellationToken);
            return !exist;
        }
    }
}
