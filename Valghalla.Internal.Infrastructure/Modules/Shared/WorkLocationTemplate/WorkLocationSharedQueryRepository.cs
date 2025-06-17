using AutoMapper;

using Microsoft.EntityFrameworkCore;

using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Shared.WorkLocationTemplate.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.WorkLocationTemplate.Queries;
using Valghalla.Internal.Application.Modules.Shared.WorkLocationTemplate.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Shared.WorkLocation
{
    internal class WorkLocationTemplateSharedQueryRepository : IWorkLocationTemplateSharedQueryRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<WorkLocationTemplateEntity> workLocationTemplates;

        public WorkLocationTemplateSharedQueryRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            workLocationTemplates = dataContext.Set<WorkLocationTemplateEntity>().AsNoTracking();
        }

        public async Task<IEnumerable<WorkLocationTemplateSharedResponse>> GetWorkLocationTemplatesAsync(GetWorkLocationTemplatesSharedQuery query, CancellationToken cancellationToken)
        {
            var entities = await workLocationTemplates.OrderBy(x => x.Title).ToArrayAsync(cancellationToken);
            return entities.Select(mapper.Map<WorkLocationTemplateSharedResponse>);
        }

        public async Task<WorkLocationTemplateSharedResponse?> GetWorkLocationTemplateAsync(GetWorkLocationTemplateSharedQuery query, CancellationToken cancellationToken)
        {
            //var entity = await workLocationTemplates.Include(x => x.ElectionWorkLocations)
            //    .SingleOrDefaultAsync(i => i.Id == query.WorkLocationId && (!query.ElectionId.HasValue || i.ElectionWorkLocations.Any(e => e.ElectionId == query.ElectionId)), cancellationToken);

            var entity = await workLocationTemplates.SingleOrDefaultAsync(i => i.Id == query.WorkLocationId, cancellationToken);

            if (entity == null) return null;

            var mappedEntity = mapper.Map<WorkLocationTemplateSharedResponse>(entity);

            return mappedEntity;
        }
    }
}
