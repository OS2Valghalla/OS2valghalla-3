using FluentValidation;

using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Election.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Election.Queries
{
    public sealed record GetElectionsQuery() : IQuery<Response>;

    public sealed class GetElectionsQueryValidator : AbstractValidator<GetElectionsQuery>
    {
        public GetElectionsQueryValidator()
        {
        }
    }

    internal sealed class GetElectionsQueryHandler : IQueryHandler<GetElectionsQuery>
    {
        private readonly IElectionQueryRepository electionQueryRepository;

        public GetElectionsQueryHandler(IElectionQueryRepository electionQueryRepository)
        {
            this.electionQueryRepository = electionQueryRepository;
        }

        public async Task<Response> Handle(GetElectionsQuery query, CancellationToken cancellationToken)
        {
            var result = await electionQueryRepository.GetElectionsAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
