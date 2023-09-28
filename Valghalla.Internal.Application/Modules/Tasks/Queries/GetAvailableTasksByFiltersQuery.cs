using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Tenant;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Requests;

namespace Valghalla.Internal.Application.Modules.Tasks.Queries
{
    public sealed record GetAvailableTasksByFiltersQuery(Guid ElectionId, TasksFilterRequest Filters) : IQuery<Response>;

    public sealed class GetAvailableTasksByFiltersQueryValidator : AbstractValidator<GetAvailableTasksByFiltersQuery>
    {
        public GetAvailableTasksByFiltersQueryValidator()
        {
            RuleFor(x => x.ElectionId)
                .NotEmpty();
        }
    }

    internal sealed class GetAvailableTasksByFiltersQueryHandler : IQueryHandler<GetAvailableTasksByFiltersQuery>
    {
        private readonly ITenantContextProvider tenantContextProvider;
        private readonly IFilteredTasksQueryRepository filteredTasksQueryRepository;

        public GetAvailableTasksByFiltersQueryHandler(ITenantContextProvider tenantContextProvider, IFilteredTasksQueryRepository filteredTasksQueryRepository)
        {
            this.tenantContextProvider = tenantContextProvider;
            this.filteredTasksQueryRepository = filteredTasksQueryRepository;
        }

        public async Task<Response> Handle(GetAvailableTasksByFiltersQuery query, CancellationToken cancellationToken)
        {
            var taskDetailsPageUrl = tenantContextProvider.CurrentTenant.ExternalDomain.TrimEnd('/') + "/opgaver/detaljer/";
            var result = await filteredTasksQueryRepository.GetAvailableTasksByFiltersAsync(query, taskDetailsPageUrl, cancellationToken);
            return Response.Ok(result);
        }
    }
}
