using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Worker.Infrastructure.Modules.Job.Responses;

namespace Valghalla.Worker.Infrastructure.Modules.Job.Repositories
{
    public interface IJobQueryRepository
    {
        Task<JobResponse?> GetJobAsync(Guid id, CancellationToken cancellationToken);
    }

    internal class JobQueryRepository : IJobQueryRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<JobEntity> jobs;

        public JobQueryRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            jobs = dataContext.Set<JobEntity>().AsNoTracking();
        }

        public async Task<JobResponse?> GetJobAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await jobs
                .Where(i => i.Id == id)
                .SingleOrDefaultAsync(cancellationToken);

            return mapper.Map<JobResponse>(entity);
        }
    }
}
