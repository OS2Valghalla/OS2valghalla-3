using Microsoft.EntityFrameworkCore;
using Valghalla.Application.User;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.External.Application.Modules.Shared.User;

namespace Valghalla.External.Infrastructure.Modules.Shared.User
{
    internal class UserSharedQueryRepository : IUserSharedQueryRepository
    {
        private readonly IQueryable<UserEntity> users;
        private readonly IQueryable<ParticipantEntity> participants;

        public UserSharedQueryRepository(DataContext dataContext)
        {
            users = dataContext.Set<UserEntity>().AsNoTracking();
            participants = dataContext.Set<ParticipantEntity>().AsNoTracking();
        }

        public async Task<UserInfo?> GetUserInfoAsync(string cprNumber, CancellationToken cancellationToken)
        {
            var userEntity = await users
                .Where(i => i.Cpr == cprNumber)
                .SingleOrDefaultAsync(cancellationToken);

            if (userEntity == null) return null;

            var participant = await participants
                .Include(i => i.TeamResponsibles)
                .Include(i => i.WorkLocationResponsibles)
                .Where(i => i.UserId == userEntity.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (participant == null) return null;

            var roleIds = new List<Guid>() { Role.Participant.Id };

            if (participant.TeamResponsibles.Any())
            {
                roleIds.Add(Role.TeamResponsible.Id);
            }

            if (participant.WorkLocationResponsibles.Any())
            {
                roleIds.Add(Role.WorkLocationResponsible.Id);
            };

            return new UserInfo()
            {
                Id = userEntity.Id,
                ParticipantId = participant.Id,
                Name = participant.FirstName + " " + participant.LastName,
                RoleIds = roleIds
            };
        }
    }
}
