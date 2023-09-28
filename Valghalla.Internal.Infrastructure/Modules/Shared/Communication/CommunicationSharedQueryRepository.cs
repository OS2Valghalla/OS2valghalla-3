using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Shared.Communication.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Communication.Queries;
using Valghalla.Internal.Application.Modules.Shared.Communication.Responses;
using Valghalla.Internal.Application.Modules.Shared.Election.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Shared.Communication
{
    internal class CommunicationSharedQueryRepository: ICommunicationSharedQueryRepository
    {
        private readonly IQueryable<CommunicationTemplateEntity> communicationTemplates;
        private readonly IMapper mapper;

        public CommunicationSharedQueryRepository(DataContext dataContext, IMapper mapper)
        {
            communicationTemplates = dataContext.Set<CommunicationTemplateEntity>().AsNoTracking();
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CommunicationTemplateSharedResponse>> GetCommunicationTemplatesAsync(GetCommunicationTemplatesSharedQuery query, CancellationToken cancellationToken)
        {
            var excludedIds = new List<Guid> { 
                new Guid(Constants.DefaultCommunicationTemplates.InvitationReminderStringId),
                new Guid(Constants.DefaultCommunicationTemplates.TaskReminderStringId),
                new Guid(Constants.DefaultCommunicationTemplates.RetractedInvitationStringId)
            };

            var entities = await communicationTemplates
                .Where(x => !excludedIds.Contains(x.Id))
                .OrderByDescending(i => i.Title)
                .ToArrayAsync(cancellationToken);

            return entities
                .Select(mapper.Map<CommunicationTemplateSharedResponse>)
                .ToArray();
        }
    }
}
