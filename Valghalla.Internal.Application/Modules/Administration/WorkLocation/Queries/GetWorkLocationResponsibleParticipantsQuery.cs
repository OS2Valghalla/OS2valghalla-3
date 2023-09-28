using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.WorkLocation.Queries
{
    public sealed record GetWorkLocationResponsibleParticipantsQuery(Guid WorkLocationId) : IQuery<Response>;

    public sealed class GetWorkLocationResponsibleParticipantsQueryValidator : AbstractValidator<GetWorkLocationResponsibleParticipantsQuery>
    {
        public GetWorkLocationResponsibleParticipantsQueryValidator()
        {
            RuleFor(x => x.WorkLocationId)
                .NotEmpty();
        }
    }

    internal sealed class GetWorkLocationResponsibleParticipantsQueryHandler : IQueryHandler<GetWorkLocationResponsibleParticipantsQuery>
    {
        private readonly IWorkLocationQueryRepository workLocationQueryRepository;

        public GetWorkLocationResponsibleParticipantsQueryHandler(IWorkLocationQueryRepository workLocationQueryRepository)
        {
            this.workLocationQueryRepository = workLocationQueryRepository;
        }

        public async Task<Response> Handle(GetWorkLocationResponsibleParticipantsQuery query, CancellationToken cancellationToken)
        {
            var result = await workLocationQueryRepository.GetWorkLocationResponsiblesAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
