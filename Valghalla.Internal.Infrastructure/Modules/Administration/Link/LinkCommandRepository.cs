using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.Link.Commands;
using Valghalla.Internal.Application.Modules.Administration.Link.Interfaces;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.Link
{
    internal class LinkCommandRepository: ILinkCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<TaskLinkEntity> taskLinks;
        private readonly DbSet<TasksFilteredLinkEntity> tasksFilteredLink;

        public LinkCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            taskLinks = dataContext.Set<TaskLinkEntity>();
            tasksFilteredLink = dataContext.Set<TasksFilteredLinkEntity>();
        }

        public async Task<string> CreateTaskLinkAsync(CreateTaskLinkCommand command, CancellationToken cancellationToken)
        {
            var existingEntity = await taskLinks.FirstOrDefaultAsync(x => x.ElectionId == command.ElectionId && x.Value == command.Value);
            if (existingEntity != null)
            {
                return existingEntity.HashValue;
            }

            var entity = new TaskLinkEntity()
            {
                Id = Guid.NewGuid(),
                ElectionId = command.ElectionId,
                HashValue = command.HashValue,
                Value = command.Value
            };

            await taskLinks.AddAsync(entity, cancellationToken);
            await dataContext.SaveChangesAsync(cancellationToken);

            return entity.HashValue;
        }

        public async Task<string> CreateTasksFilteredLinkAsync(CreateTasksFilteredLinkCommand command, CancellationToken cancellationToken)
        {
            var existingEntity = await tasksFilteredLink.FirstOrDefaultAsync(x => x.ElectionId == command.ElectionId && x.Value == command.Value);
            if (existingEntity != null)
            {
                return existingEntity.HashValue;
            }

            var entity = new TasksFilteredLinkEntity()
            {
                Id = Guid.NewGuid(),
                ElectionId = command.ElectionId,
                HashValue = command.HashValue,
                Value = command.Value
            };

            await tasksFilteredLink.AddAsync(entity, cancellationToken);
            await dataContext.SaveChangesAsync(cancellationToken);

            return entity.HashValue;
        }
    }
}
