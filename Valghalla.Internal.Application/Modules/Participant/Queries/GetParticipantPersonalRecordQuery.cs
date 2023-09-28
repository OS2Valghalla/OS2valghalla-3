using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.AuditLog;
using Valghalla.Application.CPR;
using Valghalla.Application.Exceptions;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;

namespace Valghalla.Internal.Application.Modules.Participant.Queries
{
    public sealed record GetParticipantPersonalRecordQuery(string Cpr) : IQuery<Response>;

    public sealed class GetParticipantPersonalRecordQueryValidator : AbstractValidator<GetParticipantPersonalRecordQuery>
    {
        public GetParticipantPersonalRecordQueryValidator(IParticipantQueryRepository participantQueryRepository)
        {
            RuleFor(x => x.Cpr)
                .NotEmpty();

            RuleFor(x => x.Cpr)
                .Must(CprValidator.IsCprValid)
                .WithMessage("participant.error.cpr_invalid");

            RuleFor(x => x)
                .Must((command) => !participantQueryRepository.CheckIfParticipantExistsAsync(command.Cpr, default).Result)
                .WithMessage("participant.error.participant_exists");
        }
    }

    internal class GetParticipantPersonalRecordQueryHandler : IQueryHandler<GetParticipantPersonalRecordQuery>
    {
        private readonly ICPRService cprService;
        private readonly IAuditLogService auditLogService;

        public GetParticipantPersonalRecordQueryHandler(ICPRService cprService, IAuditLogService auditLogService)
        {
            this.cprService = cprService;
            this.auditLogService = auditLogService;
        }

        public async Task<Response> Handle(GetParticipantPersonalRecordQuery query, CancellationToken cancellationToken)
        {
            CprPersonInfo cprPersonInfo;

            try
            {
                cprPersonInfo = await cprService.ExecuteAsync(query.Cpr);

                await auditLogService.AddAuditLogAsync(new ParticipantCprAuditLog(query.Cpr), cancellationToken);
            }
            catch (CvrInputInvalidException)
            {
                return Response.Fail("participant.error.cvr_invalid");
            }
            catch (Exception ex)
            {
                return Response.Fail(ex.Message);
            }

            var record = cprPersonInfo.ToRecord();
            return Response.Ok(record);
        }
    }
}
