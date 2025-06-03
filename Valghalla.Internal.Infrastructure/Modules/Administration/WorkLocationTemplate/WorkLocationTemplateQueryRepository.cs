using AutoMapper;
using Microsoft.EntityFrameworkCore;

using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.Team.Commands;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Responses;
using Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Commands;
using Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Queries;
using Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.WorkLocationTemplate
{
    internal class WorkLocationTemplateQueryRepository : IWorkLocationTemplateQueryRepository
    {
        private readonly IQueryable<WorkLocationTemplateEntity> WorkLocationTemplates;
        //private readonly IQueryable<WorkLocationTemplateResponsibleEntity> WorkLocationTemplateResponsibles;
        private readonly IMapper mapper;

        public WorkLocationTemplateQueryRepository(DataContext dataContext, IMapper mapper)
        {
            WorkLocationTemplates = dataContext.Set<WorkLocationTemplateEntity>().AsNoTracking();
            this.mapper = mapper;
        }

        public async Task<bool> CheckIfWorkLocationTemplateExistsAsync(CreateWorkLocationTemplateCommand command, CancellationToken cancellationToken)
        {     
            var name = command.Title.Trim().ToLower();
            return await WorkLocationTemplates.AnyAsync(i => i.Title.ToLower() == name, cancellationToken);
        }

        public async Task<bool> CheckIfWorkLocationTemplateExistsAsync(UpdateWorkLocationTemplateCommand command, CancellationToken cancellationToken)
        {
            var name = command.Title.Trim().ToLower();
            return await WorkLocationTemplates.AnyAsync(i => i.Title.ToLower() == name && i.Id != command.Id, cancellationToken);
        }

        //public async Task<bool> CheckIfWorkLocationTemplateUsedInActiveElectionAsync(Guid id, CancellationToken cancellationToken)
        //{
        //    return await WorkLocationTemplates.Include(x => x.Elections).AnyAsync(x => x.Id == id && x.Elections.Any(e => e.Active), cancellationToken);
        //}
        //public async Task<bool> CheckIfWorkLocationTemplateHasTasksAsync(DeleteWorkLocationTemplateCommand command, CancellationToken cancellationToken)
        //{
        //    return await WorkLocationTemplates.Include(x => x.WorkLocationTemplateTeams).ThenInclude(x => x.TaskAssignments).AnyAsync(i =>
        //        i.Id == command.Id &&
        //        i.WorkLocationTemplateTeams.Any(t => t.TaskAssignments.Any()), cancellationToken);
        //}
        public async Task<WorkLocationTemplateDetailResponse?> GetWorkLocationTemplateAsync(GetWorkLocationTemplateQuery query, CancellationToken cancellationToken)
        {
            var entity = await WorkLocationTemplates.SingleOrDefaultAsync(i => i.Id == query.WorkLocationTemplateId, cancellationToken);

            if (entity == null) return null;

            var mappedEntity = mapper.Map<WorkLocationTemplateDetailResponse>(entity);
            //mappedEntity.HasActiveElection = await WorkLocationTemplates.Include(x => x.Elections).AnyAsync(x => x.Id == query.WorkLocationTemplateId && x.Elections.Any(e => e.Active), cancellationToken);

            return mappedEntity;
        }

        public Task<bool> CheckIfWorkLocationTemplateHasTasksAsync(DeleteWorkLocationTemplateCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<WorkLocationResponsibleResponse>> GetWorkLocationTemplateResponsiblesAsync(GetWorkLocationTemplateResponsibleParticipantsQuery query, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        //public async Task<List<WorkLocationTemplateResponsibleResponse>> GetWorkLocationTemplateResponsiblesAsync(GetWorkLocationTemplateResponsibleParticipantsQuery query, CancellationToken cancellationToken)
        //{
        //    var entities = await WorkLocationTemplateResponsibles
        //        .AsNoTracking()
        //        .Where(i => i.WorkLocationTemplateId == query.WorkLocationTemplateId)
        //        .Include(i => i.Participant)
        //        .OrderBy(i => i.Participant.FirstName + " " + i.Participant.LastName)
        //        .ToListAsync(cancellationToken);

        //    List<WorkLocationTemplateResponsibleResponse> result = new List<WorkLocationTemplateResponsibleResponse>();

        //    foreach (var entity in entities)
        //    {
        //        var item = mapper.Map<WorkLocationTemplateResponsibleResponse>(entity);

        //        item.ParticipantEmail = entity.Participant.Email;
        //        item.ParticipantFirstName = entity.Participant.FirstName;
        //        item.ParticipantLastName = entity.Participant.LastName;
        //        item.ParticipantMobileNumber = entity.Participant.MobileNumber;

        //        result.Add(item);
        //    }

        //    return result;
        //}
    }
}
