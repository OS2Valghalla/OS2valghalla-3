using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.Link.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Link.Queries;
using Valghalla.Internal.Application.Modules.Administration.Link.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.Link
{
    internal class LinkQueryRepository: ILinkQueryRepository
    {
        private readonly IQueryable<TaskLinkEntity> taskLink;
        private readonly IQueryable<TasksFilteredLinkEntity> tasksFilteredLink;
        private readonly IMapper mapper;

        public LinkQueryRepository(DataContext dataContext, IMapper mapper)
        {
            taskLink = dataContext.Set<TaskLinkEntity>().AsNoTracking();
            tasksFilteredLink = dataContext.Set<TasksFilteredLinkEntity>().AsNoTracking();
            this.mapper = mapper;
        }

        public async Task<LinkResponse?> GetTaskLinkAsync(GetTaskLinkQuery query, CancellationToken cancellationToken)
        {
            var entity = await taskLink.SingleOrDefaultAsync(i => i.HashValue == query.HashValue && i.ElectionId == query.ElectionId, cancellationToken);
            if (entity == null) return null;
            return mapper.Map<LinkResponse>(entity);
        }

        public async Task<LinkResponse?> GetTasksFilteredLinkAsync(GetTasksFilteredLinkQuery query, CancellationToken cancellationToken)
        {
            var entity = await tasksFilteredLink.SingleOrDefaultAsync(i => i.HashValue == query.HashValue && i.ElectionId == query.ElectionId, cancellationToken);
            if (entity == null) return null;
            return mapper.Map<LinkResponse>(entity);
        }

    }

}
