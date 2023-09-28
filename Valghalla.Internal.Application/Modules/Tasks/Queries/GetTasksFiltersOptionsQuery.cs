using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;

namespace Valghalla.Internal.Application.Modules.Tasks.Queries
{
    public sealed record GetTasksFiltersOptionsQuery(Guid ElectionId) : IQuery<Response>;

    public sealed class GetTasksFiltersOptionsQueryValidator : AbstractValidator<GetTasksFiltersOptionsQuery>
    {
        public GetTasksFiltersOptionsQueryValidator()
        {
            RuleFor(x => x.ElectionId)
                .NotEmpty();
        }
    }

    internal sealed class GetTasksFiltersOptionsQueryHandler : IQueryHandler<GetTasksFiltersOptionsQuery>
    {
        private readonly IFilteredTasksQueryRepository filteredTasksQueryRepository;

        public GetTasksFiltersOptionsQueryHandler(IFilteredTasksQueryRepository filteredTasksQueryRepository)
        {
            this.filteredTasksQueryRepository = filteredTasksQueryRepository;
        }

        public async Task<Response> Handle(GetTasksFiltersOptionsQuery query, CancellationToken cancellationToken)
        {
            var result = await filteredTasksQueryRepository.GetTasksFiltersOptionsAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
