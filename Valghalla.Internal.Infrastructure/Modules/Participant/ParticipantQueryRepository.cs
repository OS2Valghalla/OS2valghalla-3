using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Participant.Commands;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;
using Valghalla.Internal.Application.Modules.Participant.Queries;
using Valghalla.Internal.Application.Modules.Participant.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Participant
{
    internal class ParticipantQueryRepository : IParticipantQueryRepository
    {
        private readonly IQueryable<ParticipantEntity> participants;
        private readonly IQueryable<UserEntity> users;
        private readonly IQueryable<TeamMemberEntity> teamMembers;
        private readonly IQueryable<TeamResponsibleEntity> teamResponsibles;
        private readonly IQueryable<WorkLocationResponsibleEntity> workLocationResponsibles;
        private readonly IQueryable<TaskAssignmentEntity> taskAssignments;
        private readonly IQueryable<RejectedTaskAssignmentEntity> rejectedTaskAssignments;

        private readonly IMapper mapper;

        public ParticipantQueryRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            participants = dataContext.Set<ParticipantEntity>().AsNoTracking();
            users = dataContext.Set<UserEntity>().AsNoTracking();
            taskAssignments = dataContext.Set<TaskAssignmentEntity>().AsNoTracking();
            rejectedTaskAssignments = dataContext.Set<RejectedTaskAssignmentEntity>().AsNoTracking();
            teamMembers = dataContext.Set<TeamMemberEntity>().AsNoTracking();
            teamResponsibles = dataContext.Set<TeamResponsibleEntity>().AsNoTracking();
            workLocationResponsibles = dataContext.Set<WorkLocationResponsibleEntity>().AsNoTracking();
        }

        public async Task<bool> CheckIfParticipantExistsAsync(string cprNumber, CancellationToken cancellationToken)
        {
            var cpr = cprNumber.Trim().ToLower();

            var participantExists = await participants
                .AnyAsync(i => i.Cpr.ToLower() == cpr, cancellationToken);

            var userExists = await users
                .AnyAsync(i => i.Cpr != null && i.Cpr.ToLower() == cpr, cancellationToken);

            return participantExists || userExists;
        }

        public async Task<bool> CheckIfParticipantHasAssociatedTasksAsync(UpdateParticipantCommand command, CancellationToken cancellationToken)
        {
            var currentTeamIds = await teamMembers
                .Where(i => i.ParticipantId == command.Id)
                .Select(i => i.TeamId)
                .ToArrayAsync(cancellationToken);

            var teamIdsToRemove = currentTeamIds
                .Where(currentTeamId => !command.TeamIds.Any(teamId => teamId == currentTeamId));

            return await taskAssignments
                .Where(i =>
                    i.ParticipantId == command.Id &&
                    teamIdsToRemove.Contains(i.TeamId) &&
                    i.Accepted &&
                    i.Responsed)
                .AnyAsync(cancellationToken);
        }

        public async Task<ParticipantDetailResponse?> GetParticipantDetailsAsync(GetParticipantDetailsQuery query, CancellationToken cancellationToken)
        {
            var entity = await participants
                .Include(i => i.TeamMembers)
                .Include(i => i.SpecialDietParticipants)
                .SingleOrDefaultAsync(i => i.Id == query.Id, cancellationToken);

            return mapper.Map<ParticipantDetailResponse>(entity);
        }

        public async Task<IEnumerable<Guid>> GetTeamResponsibleRightsAsync(GetTeamResponsibleRightsQuery query, CancellationToken cancellationToken)
        {
            return await teamResponsibles
                .Where(i => i.ParticipantId == query.Id)
                .Select(i => i.TeamId)
                .ToArrayAsync(cancellationToken);
        }

        public async Task<IEnumerable<Guid>> GetWorkLocationResponsibleRightsAsync(GetWorkLocationResponsibleRightsQuery query, CancellationToken cancellationToken)
        {
            return await workLocationResponsibles
                .Where(i => i.ParticipantId == query.Id)
                .Select(i => i.WorkLocationId)
                .ToArrayAsync(cancellationToken);
        }

        public async Task<IList<ParticipantTaskResponse>> GetParticipantTasksAsync(GetParticipantTasksQuery query, CancellationToken cancellationToken)
        {
            var tasks = await taskAssignments.Include(t => t.WorkLocation).Include(t => t.Election).Include(t => t.TaskType).Include(t => t.Team)
                .Where(i => i.ParticipantId == query.Id)
                .OrderByDescending(x => x.TaskDate).ThenBy(x => x.TaskType.Title).ToListAsync(cancellationToken);

            var result = tasks.Select(mapper.Map<ParticipantTaskResponse>).ToList();

            var rejectedTasks = await rejectedTaskAssignments.Include(t => t.WorkLocation).Include(t => t.Election).Include(t => t.TaskType).Include(t => t.Team)
                .Where(i => i.ParticipantId == query.Id)
                .OrderByDescending(x => x.TaskDate).ThenBy(x => x.TaskType.Title).ToListAsync(cancellationToken);

            result.AddRange(rejectedTasks.Select(mapper.Map<ParticipantTaskResponse>).ToList());

            return result;
        }
    }
}
