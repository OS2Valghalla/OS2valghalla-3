using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Worker.Infrastructure.Modules.Job.Repositories
{
    public interface IJobCommandRepository
    {
        Task SetJobAsync(Guid id, string name, Guid jobId, CancellationToken cancellationToken);
        Task UnsetJobAsync(Guid id, CancellationToken cancellationToken);
    }

    internal class JobCommandRepository : IJobCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<JobEntity> jobs;

        public JobCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            jobs = dataContext.Set<JobEntity>();
        }

        public async Task SetJobAsync(Guid id, string name, Guid jobId, CancellationToken cancellationToken)
        {
            var entity = await jobs.SingleOrDefaultAsync(i => i.Id == id, cancellationToken);

            if (entity != null)
            {
                entity.Name = name;
                entity.JobId = jobId;
                jobs.Update(entity);
            }
            else
            {
                entity = new JobEntity()
                {
                    Id = id,
                    Name = name,
                    JobId = jobId
                };

                await jobs.AddAsync(entity, cancellationToken);
            }

            await dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UnsetJobAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await jobs.SingleAsync(i => i.Id == id, cancellationToken);
            entity.JobId = null;

            jobs.Update(entity);
            await dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
