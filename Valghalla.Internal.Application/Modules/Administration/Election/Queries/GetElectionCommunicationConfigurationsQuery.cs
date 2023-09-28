using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Election.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Election.Queries
{
    public sealed record GetElectionCommunicationConfigurationsQuery(Guid Id) : IQuery<Response>;

    public sealed class GetElectionCommunicationConfigurationsQueryValidator : AbstractValidator<GetElectionCommunicationConfigurationsQuery>
    {
        public GetElectionCommunicationConfigurationsQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    internal sealed class GetElectionCommunicationConfigurationsQueryHandler : IQueryHandler<GetElectionCommunicationConfigurationsQuery>
    {
        private readonly IElectionQueryRepository electionQueryRepository;

        public GetElectionCommunicationConfigurationsQueryHandler(IElectionQueryRepository electionQueryRepository)
        {
            this.electionQueryRepository = electionQueryRepository;
        }

        public async Task<Response> Handle(GetElectionCommunicationConfigurationsQuery query, CancellationToken cancellationToken)
        {
            var result = await electionQueryRepository.GetElectionCommunicationConfigurationsAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
