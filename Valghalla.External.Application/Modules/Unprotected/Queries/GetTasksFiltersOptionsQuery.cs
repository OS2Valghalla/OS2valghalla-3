using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.External.Application.Modules.Unprotected.Interfaces;

namespace Valghalla.External.Application.Modules.Unprotected.Queries
{
    public sealed record GetTasksFiltersOptionsQuery(string HashValue) : IQuery<Response>;

    public sealed class GetTasksFiltersOptionsQueryValidator : AbstractValidator<GetTasksFiltersOptionsQuery>
    {
        public GetTasksFiltersOptionsQueryValidator()
        {
            RuleFor(x => x.HashValue)
                .NotEmpty();
        }
    }

    internal sealed class GetTasksFiltersOptionsQueryHandler : IQueryHandler<GetTasksFiltersOptionsQuery>
    {
        private readonly IUnprotectedTasksQueryRepository unprotectedTasksQueryRepository;

        public GetTasksFiltersOptionsQueryHandler(IUnprotectedTasksQueryRepository unprotectedTasksQueryRepository)
        {
            this.unprotectedTasksQueryRepository = unprotectedTasksQueryRepository;
        }

        public async Task<Response> Handle(GetTasksFiltersOptionsQuery query, CancellationToken cancellationToken)
        {
            var result = await unprotectedTasksQueryRepository.GetTasksFiltersOptionsAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
