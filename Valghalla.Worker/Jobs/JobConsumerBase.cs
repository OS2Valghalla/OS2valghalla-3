using Valghalla.Application.Tenant;
using Valghalla.Worker.Infrastructure.Modules.Job.Repositories;

namespace Valghalla.Worker.Jobs
{
    internal abstract class JobConsumerBase<T>
    {
        protected abstract Guid JobDefinitionId { get; }

        private readonly IJobQueryRepository jobQueryRepository;
        private readonly IJobCommandRepository jobCommandRepository;

        protected readonly ILogger<T> logger;
        protected readonly ITenantContextProvider tenantContextProvider;

        public JobConsumerBase(
            ILogger<T> logger,
            ITenantContextProvider tenantContextProvider,
            IJobQueryRepository jobQueryRepository,
            IJobCommandRepository jobCommandRepository)
        {
            this.logger = logger;
            this.tenantContextProvider = tenantContextProvider;
            this.jobQueryRepository = jobQueryRepository;
            this.jobCommandRepository = jobCommandRepository;
        }

        protected async Task<bool> PrepareAsync(Guid jobId, CancellationToken cancellationToken)
        {
            var job = await jobQueryRepository.GetJobAsync(JobDefinitionId, cancellationToken);

            // if there is a job running, ignore this process
            if (job != null && job.JobId == jobId)
            {
                logger.LogInformation(
                    $"[{tenantContextProvider.CurrentTenant.Name}]" +
                    "A job already running. Skipping this one.");

                return false;
            };

            // register currrent job instance
            await jobCommandRepository.SetJobAsync(
                JobDefinitionId,
                typeof(T).FullName!,
                jobId,
                cancellationToken);

            return true;
        }

        protected async Task EndAsync(CancellationToken cancellationToken)
        {
            // release current job instance
            await jobCommandRepository.UnsetJobAsync(JobDefinitionId, cancellationToken);
        }
    }
}
