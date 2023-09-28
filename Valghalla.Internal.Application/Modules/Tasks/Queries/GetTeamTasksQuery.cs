using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Tenant;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;

namespace Valghalla.Internal.Application.Modules.Tasks.Queries
{
    public sealed record GetTeamTasksQuery(Guid TeamId, Guid WorkLocationId, Guid ElectionId, bool? IsGettingRejectedTasks) : IQuery<Response>;

    public sealed class GetTeamTasksQueryValidator : AbstractValidator<GetTeamTasksQuery>
    {
        public GetTeamTasksQueryValidator()
        {
            RuleFor(x => x.TeamId)
                .NotEmpty();

            RuleFor(x => x.WorkLocationId)
                .NotEmpty();

            RuleFor(x => x.ElectionId)
                .NotEmpty();
        }
    }

    internal sealed class GetTeamTasksQueryHandler : IQueryHandler<GetTeamTasksQuery>
    {
        private readonly ITenantContextProvider tenantContextProvider;
        private readonly IElectionWorkLocationTasksQueryRepository electionWorkLocationTasksQueryRepository;

        public GetTeamTasksQueryHandler(ITenantContextProvider tenantContextProvider, IElectionWorkLocationTasksQueryRepository electionWorkLocationTasksQueryRepository)
        {
            this.tenantContextProvider = tenantContextProvider;
            this.electionWorkLocationTasksQueryRepository = electionWorkLocationTasksQueryRepository;
        }

        public async Task<Response> Handle(GetTeamTasksQuery query, CancellationToken cancellationToken)
        {
            var taskDetailsPageUrl = tenantContextProvider.CurrentTenant.ExternalDomain.TrimEnd('/') + "/opgaver/detaljer/";
            var result = query.IsGettingRejectedTasks.HasValue && query.IsGettingRejectedTasks.Value ? await electionWorkLocationTasksQueryRepository.GetRejectedTeamTasksAsync(query, cancellationToken) : await electionWorkLocationTasksQueryRepository.GetTeamTasksAsync(query, taskDetailsPageUrl, cancellationToken);
            return Response.Ok(result);
        }
    }
}
