using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.External.Application.Modules.Unprotected.Interfaces;
using Valghalla.External.Application.Modules.Unprotected.Request;

namespace Valghalla.External.Application.Modules.Unprotected.Queries
{
    public sealed record GetUnprotectedAvailableTasksByFiltersQuery(string HashValue, UnprotectedTasksFilterRequest Filters) : IQuery<Response>;

    public sealed class GetUnprotectedAvailableTasksByFiltersQueryValidator : AbstractValidator<GetUnprotectedAvailableTasksByFiltersQuery>
    {
        public GetUnprotectedAvailableTasksByFiltersQueryValidator()
        {
            RuleFor(x => x.HashValue)
                .NotEmpty();
        }
    }

    internal sealed class GetUnprotectedAvailableTasksByFiltersQueryHandler : IQueryHandler<GetUnprotectedAvailableTasksByFiltersQuery>
    {
        private readonly IUnprotectedTasksQueryRepository unprotectedTasksQueryRepository;
        public GetUnprotectedAvailableTasksByFiltersQueryHandler(IUnprotectedTasksQueryRepository unprotectedTasksQueryRepository)
        {
            this.unprotectedTasksQueryRepository = unprotectedTasksQueryRepository;
        }

        public async Task<Response> Handle(GetUnprotectedAvailableTasksByFiltersQuery query, CancellationToken cancellationToken)
        {
            var result = await unprotectedTasksQueryRepository.GetAvailableTasksByFiltersAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
