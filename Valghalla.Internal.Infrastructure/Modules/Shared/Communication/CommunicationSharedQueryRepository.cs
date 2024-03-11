using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Shared.Communication.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Communication.Queries;
using Valghalla.Internal.Application.Modules.Shared.Communication.Responses;

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
            var entities = await communicationTemplates
                .OrderByDescending(i => i.Title)
                .ToArrayAsync(cancellationToken);

            return entities
                .Select(mapper.Map<CommunicationTemplateSharedResponse>)
                .ToArray();
        }
    }
}
