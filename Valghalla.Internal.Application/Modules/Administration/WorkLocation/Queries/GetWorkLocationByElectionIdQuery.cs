using FluentValidation;

using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.WorkLocation.Queries
{
    public sealed record GetWorkLocationByElectionIdQuery(Guid WorkLocationId, Guid ElectionId) : IQuery<Response>;

    public sealed class GetWorkLocationByElectionIdQueryValidator : AbstractValidator<GetWorkLocationByElectionIdQuery>
    {
        public GetWorkLocationByElectionIdQueryValidator()
        {
            RuleFor(x => x.WorkLocationId)
                .NotEmpty();
            RuleFor(x => x.ElectionId)
                .NotEmpty();
        }
    }

    internal sealed class GetWorkLocationByElectionIdQueryHandler : IQueryHandler<GetWorkLocationByElectionIdQuery>
    {
        private readonly IWorkLocationQueryRepository workLocationQueryRepository;

        public GetWorkLocationByElectionIdQueryHandler(IWorkLocationQueryRepository workLocationQueryRepository)
        {
            this.workLocationQueryRepository = workLocationQueryRepository;
        }

        public async Task<Response> Handle(GetWorkLocationByElectionIdQuery query, CancellationToken cancellationToken)
        {
            var result = await workLocationQueryRepository.GetWorkLocationByElectionIdAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
