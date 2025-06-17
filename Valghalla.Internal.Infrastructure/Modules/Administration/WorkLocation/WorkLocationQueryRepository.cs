using AutoMapper;

using Microsoft.EntityFrameworkCore;

using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Commands;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Queries;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.WorkLocation
{
    internal class WorkLocationQueryRepository : IWorkLocationQueryRepository
    {
        private readonly IQueryable<WorkLocationEntity> workLocations;
        private readonly IQueryable<WorkLocationResponsibleEntity> workLocationResponsibles;
        private readonly IMapper mapper;

        public WorkLocationQueryRepository(DataContext dataContext, IMapper mapper)
        {
            workLocations = dataContext.Set<WorkLocationEntity>().AsNoTracking();
            this.mapper = mapper;
        }

        public async Task<bool> CheckIfWorkLocationExistsAsync(CreateWorkLocationCommand command, CancellationToken cancellationToken)
        {
            var name = command.Title.Trim().ToLower();
            return await workLocations.AnyAsync(i => i.Title.ToLower() == name, cancellationToken);
        }

        public async Task<bool> CheckIfWorkLocationExistsAsync(UpdateWorkLocationCommand command, CancellationToken cancellationToken)
        {
            var name = command.Title.Trim().ToLower();
            return await workLocations.AnyAsync(i => i.Title.ToLower() == name && i.Id != command.Id, cancellationToken);
        }

        public async Task<bool> CheckIfWorkLocationUsedInActiveElectionAsync(Guid id, CancellationToken cancellationToken)
        {
            return await workLocations.Include(x => x.Elections).AnyAsync(x => x.Id == id && x.Elections.Any(e => e.Active), cancellationToken);
        }
        public async Task<bool> CheckIfWorkLocationHasTasksAsync(DeleteWorkLocationCommand command, CancellationToken cancellationToken)
        {
            return await workLocations.Include(x => x.WorkLocationTeams).ThenInclude(x => x.TaskAssignments).AnyAsync(i =>
                i.Id == command.Id &&
                i.WorkLocationTeams.Any(t => t.TaskAssignments.Any()), cancellationToken);
        }
        public async Task<WorkLocationDetailResponse?> GetWorkLocationAsync(GetWorkLocationQuery query, CancellationToken cancellationToken)
        {
            var entity = await workLocations
                .Include(i => i.Area)
                .Include(i => i.WorkLocationTaskTypes).ThenInclude(wltt => wltt.TaskType).ThenInclude(ttt => ttt.TaskTypeTemplate)
                .Include(i => i.WorkLocationTeams).Include(i => i.WorkLocationResponsibles).Include(i => i.ElectionWorkLocations)
                .SingleOrDefaultAsync(i => i.Id == query.WorkLocationId, cancellationToken);

            if (entity == null) return null;

            var mappedEntity = mapper.Map<WorkLocationDetailResponse>(entity);
            mappedEntity.HasActiveElection = await workLocations.Include(x => x.Elections).AnyAsync(x => x.Id == query.WorkLocationId && x.Elections.Any(e => e.Active), cancellationToken);


            // .ThenInclude(wltt => wltt.TaskType).ThenInclude(tt => tt.TaskTypeTemplate)

            return mappedEntity;
        }

        public async Task<WorkLocationDetailResponse?> GetWorkLocationByElectionIdAsync(GetWorkLocationByElectionIdQuery query, CancellationToken cancellationToken)
        {
            var entity = await workLocations
                .Include(i => i.Area).Include(i => i.WorkLocationTaskTypes).Include(i => i.WorkLocationTeams).Include(i => i.WorkLocationResponsibles).Include(i => i.ElectionWorkLocations)
                .SingleOrDefaultAsync(i => i.Id == query.WorkLocationId && i.ElectionWorkLocations.Any(x => x.ElectionId == query.ElectionId), cancellationToken);

            if (entity == null) return null;

            var mappedEntity = mapper.Map<WorkLocationDetailResponse>(entity);
            mappedEntity.HasActiveElection = await workLocations.Include(x => x.Elections).AnyAsync(x => x.Id == query.WorkLocationId && x.Elections.Any(e => e.Active), cancellationToken);

            return mappedEntity;
        }
        public async Task<List<WorkLocationDetailResponse>?> GetWorkLocationsByElectionIdAsync(GetWorkLocationsByElectionIdQuery query, CancellationToken cancellationToken)
        {
            var entities = await workLocations
                .Include(i => i.Area)
                .Include(i => i.WorkLocationTaskTypes)
                .Include(i => i.WorkLocationTeams)
                .Include(i => i.WorkLocationResponsibles)
                .Include(i => i.ElectionWorkLocations)
                .Where(i => i.ElectionWorkLocations.Any(x => x.ElectionId == query.ElectionId))
                .ToListAsync(cancellationToken);

            if (entities == null) return null;

            var mappedEntity = entities.Select(entity => mapper.Map<WorkLocationDetailResponse>(entity));

            return mappedEntity.ToList();
        }
        public async Task<List<WorkLocationResponsibleResponse>> GetWorkLocationResponsiblesAsync(GetWorkLocationResponsibleParticipantsQuery query, CancellationToken cancellationToken)
        {
            var entities = await workLocationResponsibles
                .AsNoTracking()
                .Where(i => i.WorkLocationId == query.WorkLocationId)
                .Include(i => i.Participant)
                .OrderBy(i => i.Participant.FirstName + " " + i.Participant.LastName)
                .ToListAsync(cancellationToken);

            List<WorkLocationResponsibleResponse> result = new List<WorkLocationResponsibleResponse>();

            foreach (var entity in entities)
            {
                var item = mapper.Map<WorkLocationResponsibleResponse>(entity);

                item.ParticipantEmail = entity.Participant.Email;
                item.ParticipantFirstName = entity.Participant.FirstName;
                item.ParticipantLastName = entity.Participant.LastName;
                item.ParticipantMobileNumber = entity.Participant.MobileNumber;

                result.Add(item);
            }

            return result;
        }
    }
}
