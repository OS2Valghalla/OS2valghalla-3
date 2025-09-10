using AutoMapper;

using Microsoft.EntityFrameworkCore;

using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.External.Application.Modules.Team.Interfaces;
using Valghalla.External.Application.Modules.Team.Responses;

namespace Valghalla.External.Infrastructure.Modules.Team
{
    internal class TeamQueryRepository : ITeamQueryRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<ParticipantEntity> participants;
        private readonly IQueryable<TeamEntity> teams;
        private readonly IQueryable<TaskAssignmentEntity> tasks;
        private readonly IQueryable<WorkLocationEntity> workLocations;
        private readonly IQueryable<TaskAssignmentEntity> taskAssignments;
        private readonly IQueryable<RejectedTaskAssignmentEntity> rejectedTaskAssignments;        

        public TeamQueryRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            participants = dataContext.Set<ParticipantEntity>().AsNoTracking();
            teams = dataContext.Set<TeamEntity>().AsNoTracking();
            tasks = dataContext.Set<TaskAssignmentEntity>().AsNoTracking();
            workLocations = dataContext.Set<WorkLocationEntity>().AsNoTracking();
            taskAssignments = dataContext.Set<TaskAssignmentEntity>().AsNoTracking();
            rejectedTaskAssignments = dataContext.Set<RejectedTaskAssignmentEntity>().AsNoTracking();            
        }

        public async Task<IList<TeamResponse>> GetMyTeamsAsync(Guid participantId, CancellationToken cancellationToken)
        {
            var participant = await participants
                .Where(i => i.Id == participantId)
                .SingleAsync(cancellationToken);

            var teamEntities = await teams.Include(i => i.TeamResponsibles)
                .Where(i => i.TeamResponsibles.Any(x => x.ParticipantId == participant.Id))
                .OrderBy(i => i.Name).ToListAsync(cancellationToken);

            return teamEntities.Select(mapper.Map<TeamResponse>).ToList();
        }

        public async Task<IList<TeamMemberResponse>> GetTeamMembersAsync(Guid teamId, Guid participantId, CancellationToken cancellationToken)
        {
            var responsible = await participants
                .Where(i => i.Id == participantId)
                .SingleAsync(cancellationToken);

            var teamEntities = await teams.Include(i => i.TeamResponsibles)
                .Where(i => i.TeamResponsibles.Any(x => x.ParticipantId == responsible.Id))
                .OrderBy(i => i.Name).ToListAsync(cancellationToken);

            if (!teamEntities.Any(t => t.Id == teamId)) return new List<TeamMemberResponse>();

            var participantEntities = await participants
                .Include(i => i.TeamMembers)
                .Include(i => i.TeamResponsibles)
                .Where(i => i.TeamMembers.Any(tm => tm.TeamId == teamId) || i.TeamResponsibles.Any(tr => tr.TeamId == teamId))
                .OrderBy(i => i.FirstName).ThenBy(i => i.LastName)
                .ToListAsync(cancellationToken);

            var teamMembers = participantEntities.Select(mapper.Map<TeamMemberResponse>).ToList();
            foreach (var teamMember in teamMembers)
            {
                teamMember.AssignedTasksCount = tasks.Count(t => t.TeamId == teamId && t.ParticipantId == teamMember.Id && t.TaskDate >= DateTime.Today && t.Election.Active);
                teamMember.CanBeRemoved = !tasks.Include(i => i.Election).Any(t => t.TeamId == teamId && t.ParticipantId == teamMember.Id && t.Accepted && (t.TaskDate < DateTime.Today || t.Election.ElectionEndDate < DateTime.Today));

                var memberTasks = await taskAssignments
                    .Where(t => t.TeamId == teamId && t.ParticipantId == teamMember.Id && t.Election.Active)
                    .Include(t => t.WorkLocation)
                    .Include(t => t.TaskType)
                    .ToListAsync(cancellationToken);

                var memberRejectedTasks = await rejectedTaskAssignments
                    .Where(t => t.TeamId == teamId && t.ParticipantId == teamMember.Id && t.Election.Active)
                    .Include(t => t.WorkLocation)
                    .Include(t => t.TaskType)
                    .ToListAsync(cancellationToken);

                var workLocationGroups = memberTasks
                    .Select(mt => new { mt.WorkLocationId, Location = mt.WorkLocation.Title, TaskTitle = mt.TaskType.Title, TaskDate = ToLocalCalendarDate(mt.TaskDate), TaskStatus = mt.Accepted ? 0 : (!mt.Responsed ? 1 : 2) })
                    .Concat(memberRejectedTasks.Select(rt => new { rt.WorkLocationId, Location = rt.WorkLocation.Title, TaskTitle = rt.TaskType.Title, TaskDate = ToLocalCalendarDate(rt.TaskDate), TaskStatus = 2 }))
                    .GroupBy(x => new { x.WorkLocationId, x.Location })
                    .ToList();

                teamMember.WorkLocations = workLocationGroups.Select(g => new TeamMemberWorkLocationDetailsResponse
                {
                    WorkLocationTitle = g.Key.Location,
                    Tasks = g.Select(t => new TeamMemberTaskDetailsResponse
                    {
                        TaskTitle = t.TaskTitle,
                        TaskStatus = t.TaskStatus,
                        TaskDate = DateTime.SpecifyKind(t.TaskDate, DateTimeKind.Unspecified)
                    }).OrderBy(t => t.TaskDate).ToList()
                }).OrderBy(w => w.WorkLocationTitle).ToList();
            }

            return teamMembers;
        }

        private static DateTime ToLocalCalendarDate(DateTime dt)
        {
            if (dt.Kind == DateTimeKind.Utc)
                dt = dt.ToLocalTime();
            else if (dt.Kind == DateTimeKind.Unspecified)
            {
                dt = DateTime.SpecifyKind(dt, DateTimeKind.Utc).ToLocalTime();
            }
            return dt.Date;
        }
    }

}
