using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.App.Interfaces;
using Valghalla.Internal.Application.Modules.App.Queries;
using Valghalla.Internal.Application.Modules.App.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.App
{
    internal class AppUserQueryRepository : IAppUserQueryRepository
    {
        private readonly IQueryable<UserEntity> users;

        public AppUserQueryRepository(DataContext dataContext)
        {
            users = dataContext.Set<UserEntity>().AsNoTracking();
        }

        public async Task<UserResponse?> GetUserAsync(GetInternalUserQuery query, CancellationToken cancellationToken)
        {
            var entity = await users
                .Where(i => i.Cvr == query.CvrNumber && i.Serial == query.Serial)
                .SingleOrDefaultAsync(cancellationToken);

            if (entity == null) return null;

            return new UserResponse()
            {
                Id = entity.Id,
                Name = entity.Name,
                ParticipantId = null,
                RoleIds = new[] { entity.RoleId },
            };
        }
    }
}
