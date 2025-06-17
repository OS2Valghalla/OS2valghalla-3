using AutoMapper;

using Microsoft.EntityFrameworkCore;

using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
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
        
        public async Task<WorkLocationTemplateDetailResponse?> GetWorkLocationTemplateAsync(GetWorkLocationTemplateQuery query, CancellationToken cancellationToken)
        {
            var entity = await WorkLocationTemplates.SingleOrDefaultAsync(i => i.Id == query.WorkLocationTemplateId, cancellationToken);

            if (entity == null) return null;

            var mappedEntity = mapper.Map<WorkLocationTemplateDetailResponse>(entity);
            
            return mappedEntity;
        }

        public async Task<List<WorkLocationTemplateDetailResponse>?> GetWorkLocationTemplatesAsync(GetWorkLocationTemplatesQuery query, CancellationToken cancellationToken)
        {
            var result = new List<WorkLocationTemplateDetailResponse>();
            var entities = await WorkLocationTemplates.ToListAsync(cancellationToken);

            if (entities == null) return null;

            foreach (var entity in entities)
            {
                var mappedEntity = mapper.Map<WorkLocationTemplateDetailResponse>(entity);
                result.Add(mappedEntity);
            }
            return result;
        }

        public Task<bool> CheckIfWorkLocationTemplateHasTasksAsync(DeleteWorkLocationTemplateCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<WorkLocationResponsibleResponse>> GetWorkLocationTemplateResponsiblesAsync(GetWorkLocationTemplateResponsibleParticipantsQuery query, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        
    }
}
