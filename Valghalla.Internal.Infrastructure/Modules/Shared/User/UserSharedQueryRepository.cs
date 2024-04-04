using Microsoft.EntityFrameworkCore;
using Valghalla.Application.User;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Shared.User;

namespace Valghalla.Internal.Infrastructure.Modules.Shared.User
{
    internal class UserSharedQueryRepository : IUserSharedQueryRepository
    {
        private readonly IQueryable<UserEntity> users;

        public UserSharedQueryRepository(DataContext dataContext)
        {
            users = dataContext.Set<UserEntity>().AsNoTracking();
        }

        public async Task<UserInfo?> GetUserInfoAsync(string cvrNumber, string serial, CancellationToken cancellationToken)
        {
            var entity = await users
                .Where(i => i.Cvr == cvrNumber && i.Serial == serial)
                .FirstOrDefaultAsync(cancellationToken);

            if (entity == null) return null;

            return new UserInfo()
            {
                Id = entity.Id,
                Name = entity.Name,
                ParticipantId = null,
                RoleIds = new[] { entity.RoleId },
            };
        }
    }
}
