using Microsoft.EntityFrameworkCore;

using Valghalla.Application.User;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Commands;
using Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Interfaces;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.WorkLocationTemplate
{
    public class WorkLocationTemplateCommandRepository : IWorkLocationTemplateCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<TaskAssignmentEntity> tasks;
        private readonly DbSet<WorkLocationTemplateEntity> WorkLocationTemplates;
        private readonly IUserContextProvider userContextProvider;        
        public WorkLocationTemplateCommandRepository(DataContext dataContext,IUserContextProvider userContextProvider)
        {
            this.dataContext = dataContext;
            tasks = dataContext.Set<TaskAssignmentEntity>();
            WorkLocationTemplates = dataContext.Set<WorkLocationTemplateEntity>();
            this.userContextProvider = userContextProvider;            
        }

        public async Task<Guid> CreateWorkLocationTemplateAsync(CreateWorkLocationTemplateCommand command, CancellationToken cancellationToken)
        {         
                var WorkLocationTemplateId = Guid.NewGuid();
                var entity = new WorkLocationTemplateEntity()
                {
                    Id = WorkLocationTemplateId,
                    Title = command.Title,
                    AreaId = command.AreaId,
                    Address = command.Address,
                    PostalCode = command.PostalCode,
                    City = command.City,
                    VoteLocation = command.VoteLocation                    
                };

                await WorkLocationTemplates.AddAsync(entity, cancellationToken);
                
                await dataContext.SaveChangesAsync(cancellationToken);

                return entity.Id;            
        }

        public async Task<int> UpdateWorkLocationTemplateAsync(UpdateWorkLocationTemplateCommand command, CancellationToken cancellationToken)
        {
            var entity = await WorkLocationTemplates.SingleAsync(x => x.Id == command.Id, cancellationToken);
            
            entity.Title = command.Title;
            entity.AreaId = command.AreaId;
            entity.Address = command.Address;
            entity.PostalCode = command.PostalCode;
            entity.City = command.City;
            entity.VoteLocation = command.VoteLocation;

            WorkLocationTemplates.Update(entity);

            
            var result = await dataContext.SaveChangesAsync(cancellationToken);

            return result;
        }

        public async Task<int> DeleteWorkLocationTemplateAsync(DeleteWorkLocationTemplateCommand command, CancellationToken cancellationToken)
        {
            var entity = await WorkLocationTemplates
                .SingleAsync(x => x.Id == command.Id, cancellationToken);

            var WorkLocationTemplateTitle = entity.Title;

            WorkLocationTemplates.Remove(entity);

            var reuslt = await dataContext.SaveChangesAsync(cancellationToken);

            return reuslt;
        }
    }
}
