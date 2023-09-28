using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Election.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Election.Queries
{
    public sealed record GetElectionQuery(Guid Id) : IQuery<Response>;

    public sealed class GetElectionQueryValidator : AbstractValidator<GetElectionQuery>
    {
        public GetElectionQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    internal sealed class GetElectionQueryHandler : IQueryHandler<GetElectionQuery>
    {
        private readonly IElectionQueryRepository electionQueryRepository;

        public GetElectionQueryHandler(IElectionQueryRepository electionQueryRepository)
        {
            this.electionQueryRepository = electionQueryRepository;
        }

        public async Task<Response> Handle(GetElectionQuery query, CancellationToken cancellationToken)
        {
            var result = await electionQueryRepository.GetElectionAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
