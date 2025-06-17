using FluentValidation;

using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.WorkLocation.Queries
{
    public sealed record GetWorkLocationsByElectionIdQuery(Guid ElectionId) : IQuery<Response>;

    public sealed class GetWorkLocationsByElectionIdQueryValidator : AbstractValidator<GetWorkLocationsByElectionIdQuery>
    {
        public GetWorkLocationsByElectionIdQueryValidator()
        {
            RuleFor(x => x.ElectionId)
                .NotEmpty();
        }
    }

    internal sealed class GetWorkLocationsByElectionIdQueryHandler : IQueryHandler<GetWorkLocationsByElectionIdQuery>
    {
        private readonly IWorkLocationQueryRepository workLocationQueryRepository;

        public GetWorkLocationsByElectionIdQueryHandler(IWorkLocationQueryRepository workLocationQueryRepository)
        {
            this.workLocationQueryRepository = workLocationQueryRepository;
        }

        public async Task<Response> Handle(GetWorkLocationsByElectionIdQuery query, CancellationToken cancellationToken)
        {
            var result = await workLocationQueryRepository.GetWorkLocationsByElectionIdAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
