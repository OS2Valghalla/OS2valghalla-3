using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.ElectionType.Queries
{
    public sealed record GetElectionTypeQuery(Guid Id) : IQuery<Response>;

    public sealed class GetElectionTypeQueryValidator : AbstractValidator<GetElectionTypeQuery>
    {
        public GetElectionTypeQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    internal sealed class GetElectionTypeQueryHandler : IQueryHandler<GetElectionTypeQuery>
    {
        private readonly IElectionTypeQueryRepository electionTypeQueryRepository;

        public GetElectionTypeQueryHandler(IElectionTypeQueryRepository electionTypeQueryRepository)
        {
            this.electionTypeQueryRepository = electionTypeQueryRepository;
        }

        public async Task<Response> Handle(GetElectionTypeQuery query, CancellationToken cancellationToken)
        {
            var result = await electionTypeQueryRepository.GetElectionTypeAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
