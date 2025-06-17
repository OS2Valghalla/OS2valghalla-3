using AutoMapper;

using Microsoft.EntityFrameworkCore;

using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.Area.Responses;
using Valghalla.Internal.Application.Modules.Administration.Communication.Responses;
using Valghalla.Internal.Application.Modules.Administration.Election.Commands;
using Valghalla.Internal.Application.Modules.Administration.Election.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Election.Queries;
using Valghalla.Internal.Application.Modules.Administration.Election.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.Election
{
    public class ElectionQueryRepository : IElectionQueryRepository
    {
        private readonly IQueryable<ElectionEntity> elections;
        private readonly IMapper mapper;

        public ElectionQueryRepository(DataContext dataContext, IMapper mapper)
        {
            elections = dataContext.Set<ElectionEntity>().AsNoTracking();
            this.mapper = mapper;
        }

        public async Task<bool> CheckIfElectionExistsAsync(CreateElectionCommand command, CancellationToken cancellationToken)
        {
            var name = command.Title.Trim().ToLower();
            return await elections.AnyAsync(i => i.Title.ToLower() == name && i.ElectionDate == command.ElectionDate, cancellationToken);
        }

        public async Task<bool> CheckIfElectionExistsAsync(UpdateElectionCommand command, CancellationToken cancellationToken)
        {
            var entity = await elections
                .SingleAsync(x => x.Id == command.Id, cancellationToken);

            var name = command.Title.Trim().ToLower();
            return await elections.AnyAsync(i => i.Title.ToLower() == name && i.ElectionDate == entity.ElectionDate && i.Id != command.Id, cancellationToken);
        }

        public async Task<bool> CheckIfElectionExistsAsync(DuplicateElectionCommand command, CancellationToken cancellationToken)
        {
            var name = command.Title.Trim().ToLower();
            return await elections.AnyAsync(i => i.Title.ToLower() == name && i.ElectionDate == command.ElectionDate, cancellationToken);
        }

        public async Task<bool> CheckIfElectionIsActiveAsync(Guid electionId, CancellationToken cancellationToken)
        {
            var entity = await elections
                .SingleAsync(x => x.Id == electionId, cancellationToken);

            return entity.Active;
        }

        public async Task<ElectionDetailsResponse?> GetElectionAsync(GetElectionQuery query, CancellationToken cancellationToken)
        {
            var entity = await elections
                .Include(i => i.WorkLocations)
                .SingleOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

            if (entity == null) return null;
            return mapper.Map<ElectionDetailsResponse>(entity);
        }
        public async Task<List<ElectionDetailsResponse>?> GetElectionsAsync(GetElectionsQuery query, CancellationToken cancellationToken)
        {
            var entities = await elections.ToListAsync(cancellationToken);

            if (entities == null) return null;
            return entities.Select(mapper.Map<ElectionDetailsResponse>).ToList();
        }
        public async Task<ElectionCommunicationConfigurationsResponse?> GetElectionCommunicationConfigurationsAsync(GetElectionCommunicationConfigurationsQuery query, CancellationToken cancellationToken)
        {
            var entity = await elections
                .Include(i => i.ConfirmationOfRegistrationCommunicationTemplate)
                .Include(i => i.ConfirmationOfCancellationCommunicationTemplate)
                .Include(i => i.InvitationCommunicationTemplate)
                .Include(i => i.InvitationReminderCommunicationTemplate)
                .Include(i => i.TaskReminderCommunicationTemplate)
                .Include(i => i.RetractedInvitationCommunicationTemplate)
                .Include(i => i.RemovedFromTaskCommunicationTemplate)
                .Include(i => i.RemovedByValidationCommunicationTemplate)
                .Include(i => i.ElectionTaskTypeCommunicationTemplates).ThenInclude(i => i.ConfirmationOfRegistrationCommunicationTemplate)
                .Include(i => i.ElectionTaskTypeCommunicationTemplates).ThenInclude(i => i.ConfirmationOfCancellationCommunicationTemplate)
                .Include(i => i.ElectionTaskTypeCommunicationTemplates).ThenInclude(i => i.InvitationCommunicationTemplate)
                .Include(i => i.ElectionTaskTypeCommunicationTemplates).ThenInclude(i => i.InvitationReminderCommunicationTemplate)
                .Include(i => i.ElectionTaskTypeCommunicationTemplates).ThenInclude(i => i.TaskReminderCommunicationTemplate)
                .Include(i => i.ElectionTaskTypeCommunicationTemplates).ThenInclude(i => i.RetractedInvitationCommunicationTemplate)
                .Include(i => i.ElectionTaskTypeCommunicationTemplates).ThenInclude(i => i.RemovedFromTaskCommunicationTemplate)
                .Include(i => i.ElectionTaskTypeCommunicationTemplates).ThenInclude(i => i.RemovedByValidationCommunicationTemplate)
                .SingleOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

            if (entity == null) return null;
            var mappedEntity = mapper.Map<ElectionCommunicationConfigurationsResponse>(entity);
            foreach (var electionTaskTypeCommunicationTemplate in entity.ElectionTaskTypeCommunicationTemplates)
            {
                mappedEntity.ElectionTaskTypeCommunicationTemplates.Add(new ElectionTaskTypeCommunicationTemplateResponse
                {
                    ElectionId = electionTaskTypeCommunicationTemplate.ElectionId,
                    TaskTypeId = electionTaskTypeCommunicationTemplate.TaskTypeId,
                    ConfirmationOfRegistrationCommunicationTemplate = electionTaskTypeCommunicationTemplate.ConfirmationOfRegistrationCommunicationTemplate != null ? mapper.Map<CommunicationTemplateListingItemResponse>(electionTaskTypeCommunicationTemplate.ConfirmationOfRegistrationCommunicationTemplate) : null,
                    ConfirmationOfCancellationCommunicationTemplate = electionTaskTypeCommunicationTemplate.ConfirmationOfCancellationCommunicationTemplate != null ? mapper.Map<CommunicationTemplateListingItemResponse>(electionTaskTypeCommunicationTemplate.ConfirmationOfCancellationCommunicationTemplate) : null,
                    InvitationCommunicationTemplate = electionTaskTypeCommunicationTemplate.InvitationCommunicationTemplate != null ? mapper.Map<CommunicationTemplateListingItemResponse>(electionTaskTypeCommunicationTemplate.InvitationCommunicationTemplate) : null,
                    InvitationReminderCommunicationTemplate = electionTaskTypeCommunicationTemplate.InvitationReminderCommunicationTemplate != null ? mapper.Map<CommunicationTemplateListingItemResponse>(electionTaskTypeCommunicationTemplate.InvitationReminderCommunicationTemplate) : null,
                    TaskReminderCommunicationTemplate = electionTaskTypeCommunicationTemplate.TaskReminderCommunicationTemplate != null ? mapper.Map<CommunicationTemplateListingItemResponse>(electionTaskTypeCommunicationTemplate.TaskReminderCommunicationTemplate) : null,
                    RetractedInvitationCommunicationTemplate = electionTaskTypeCommunicationTemplate.RetractedInvitationCommunicationTemplate != null ? mapper.Map<CommunicationTemplateListingItemResponse>(electionTaskTypeCommunicationTemplate.RetractedInvitationCommunicationTemplate) : null,
                    RemovedFromTaskCommunicationTemplate = electionTaskTypeCommunicationTemplate.RemovedFromTaskCommunicationTemplate != null ? mapper.Map<CommunicationTemplateListingItemResponse>(electionTaskTypeCommunicationTemplate.RemovedFromTaskCommunicationTemplate) : null,
                    RemovedByValidationCommunicationTemplate = electionTaskTypeCommunicationTemplate.RemovedByValidationCommunicationTemplate != null ? mapper.Map<CommunicationTemplateListingItemResponse>(electionTaskTypeCommunicationTemplate.RemovedByValidationCommunicationTemplate) : null,
                });
            }

            return mappedEntity;
        }
    }
}
