using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.External.Application.Modules.MyProfile.Interfaces;
using Valghalla.External.Application.Modules.MyProfile.Responses;

namespace Valghalla.External.Infrastructure.Modules.MyProfile
{
    internal class MyProfileQueryRepository : IMyProfileQueryRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<ParticipantEntity> participants;
        private readonly IQueryable<TaskAssignmentEntity> taskAssignments;
        private readonly IQueryable<ElectionEntity> elections;

        public MyProfileQueryRepository(IMapper mapper, DataContext dataContext)
        {
            this.mapper = mapper;
            participants = dataContext.Set<ParticipantEntity>().AsNoTracking();
            taskAssignments = dataContext.Set<TaskAssignmentEntity>().AsNoTracking();
            elections = dataContext.Set<ElectionEntity>().AsNoTracking();
        }

        public async Task<bool> CheckIfMyProfileHasAssignedTaskLocked(Guid participantId, CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow;

            var taskAssignmentItems = await taskAssignments
                .Where(i =>
                    i.ParticipantId == participantId &&
                    i.TaskDate > today &&
                    i.Accepted &&
                    i.Responsed)
                .Select(i => new { i.Id, i.TaskDate, i.ElectionId })
                .ToArrayAsync(cancellationToken);

            var electionIds = taskAssignmentItems.Select(i => i.ElectionId).Distinct();

            var electionItems = await elections
                .Where(i => electionIds.Contains(i.Id))
                .Select(i => new { i.Id, i.LockPeriod })
                .ToArrayAsync(cancellationToken);

            var locked = false;

            foreach (var electionItem in electionItems)
            {
                var relevantTaskAssignmentItems = taskAssignmentItems
                    .Where(i => i.ElectionId == electionItem.Id)
                    .ToArray();

                foreach (var taskAssignmentItem in relevantTaskAssignmentItems)
                {
                    var lockedDate = taskAssignmentItem.TaskDate.AddDays(electionItem.LockPeriod * -1);

                    if (lockedDate <= today)
                    {
                        locked = true;
                        break;
                    }
                }

                if (locked)
                {
                    break;
                }
            }

            return locked;
        }

        public async Task<bool> CheckIfMyProfileHasCompletedTask(Guid participantId, CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow;

            return await taskAssignments
                .Where(i =>
                    i.ParticipantId == participantId &&
                    i.TaskDate <= today &&
                    i.Accepted &&
                    i.Responsed)
                .AnyAsync(cancellationToken);
        }

        public async Task<MyProfileResponse?> GetMyProfileAsync(Guid participantId, CancellationToken cancellationToken)
        {
            var entity = await participants
                .Include(i => i.SpecialDietParticipants)
                .Where(i => i.Id == participantId)
                .SingleOrDefaultAsync(cancellationToken);

            return mapper.Map<MyProfileResponse>(entity);
        }
    }
}
