using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.AuditLog;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;

namespace Valghalla.Internal.Application.Modules.Participant.Queries
{
    public sealed record GetParticipantDetailsQuery(Guid Id) : IQuery<Response>;

    public sealed class GetParticipantDetailsQueryValidator : AbstractValidator<GetParticipantDetailsQuery>
    {
        public GetParticipantDetailsQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    internal sealed class GetParticipantDetailsQueryHandler : IQueryHandler<GetParticipantDetailsQuery>
    {
        private readonly IParticipantQueryRepository participantQueryRepository;
        private readonly IAuditLogService auditLogService;

        public GetParticipantDetailsQueryHandler(IParticipantQueryRepository participantQueryRepository, IAuditLogService auditLogService)
        {
            this.participantQueryRepository = participantQueryRepository;
            this.auditLogService = auditLogService;
        }

        public async Task<Response> Handle(GetParticipantDetailsQuery query, CancellationToken cancellationToken)
        {
            var data = await participantQueryRepository.GetParticipantDetailsAsync(query, cancellationToken);

            if (data != null)
            {
                var auditLog = new ParticipantViewAuditLog(data.Id, data.FirstName!, data.LastName!, data.Birthdate);
                await auditLogService.AddAuditLogAsync(auditLog, cancellationToken);
            }

            return Response.Ok(data);
        }
    }
}
