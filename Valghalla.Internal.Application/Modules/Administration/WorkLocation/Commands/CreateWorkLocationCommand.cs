using FluentValidation;

using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.AuditLog;
using Valghalla.Application.Queue;
using Valghalla.Application.Queue.Messages;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Participant.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.WorkLocation.Commands
{
    public sealed record CreateWorkLocationCommand() : ICommand<Response<Guid>>
    {
        public string Title { get; init; } = string.Empty;
        public Guid AreaId { get; init; }
        public string Address { get; init; }
        public string PostalCode { get; init; }
        public string City { get; init; }
        public int VoteLocation { get; set; }
        public List<Guid> TaskTypeIds { get; init; } = new List<Guid>();
        public List<Guid> TeamIds { get; init; } = new List<Guid>();
        public List<Guid> ResponsibleIds { get; init; } = new List<Guid>();
        public List<Guid> TaskTypeTemplateIds { get; set; } = new List<Guid>();
        public Guid ElectionId { get; set; }
    }

    public sealed class CreateWorkLocationCommandValidator : AbstractValidator<CreateWorkLocationCommand>
    {
        public CreateWorkLocationCommandValidator(IWorkLocationQueryRepository workLocationQueryRepository)
        {
            RuleFor(x => x.Title)
                .MaximumLength(Valghalla.Application.Constants.Validation.MaximumGeneralStringLength)
                .NotEmpty();

            RuleFor(x => x.AreaId)
                .NotEmpty();

            RuleFor(x => x.Address)
                .MaximumLength(Valghalla.Application.Constants.Validation.MaximumGeneralStringLength)
                .NotEmpty();

            RuleFor(x => x.PostalCode)
                .MaximumLength(Valghalla.Application.Constants.Validation.MaximumGeneralStringLength)
                .NotEmpty();

            RuleFor(x => x.City)
                .MaximumLength(Valghalla.Application.Constants.Validation.MaximumGeneralStringLength)
                .NotEmpty();

            RuleFor(x => x)
               .Must((command) => !workLocationQueryRepository.CheckIfWorkLocationExistsAsync(command, default).Result)
               .WithMessage("administration.work_location.error.work_location_exist");
        }
    }

    internal class CreateWorkLocationCommandHandler : ICommandHandler<CreateWorkLocationCommand, Response<Guid>>
    {
        private readonly IWorkLocationCommandRepository workLocationCommandRepository;
        private readonly IParticipantSharedQueryRepository participantSharedQueryRepository;
        private readonly IAuditLogService auditLogService;
        private readonly IQueueService queueService;

        public CreateWorkLocationCommandHandler(
            IWorkLocationCommandRepository workLocationCommandRepository,
            IParticipantSharedQueryRepository participantSharedQueryRepository,
            IAuditLogService auditLogService,
            IQueueService queueService)
        {
            this.workLocationCommandRepository = workLocationCommandRepository;
            this.participantSharedQueryRepository = participantSharedQueryRepository;
            this.auditLogService = auditLogService;
            this.queueService = queueService;
        }

        public async Task<Response<Guid>> Handle(CreateWorkLocationCommand command, CancellationToken cancellationToken)
        {
            var id = await workLocationCommandRepository.CreateWorkLocationAsync(command, cancellationToken);
            var participants = await participantSharedQueryRepository.GetPariticipantsAsync(new Shared.Participant.Queries.GetParticipantsSharedQuery()
            {
                Values = command.ResponsibleIds,
            }, cancellationToken);

            var auditLogs = participants.Select(i => new ParticipantWorkLocationResponsibleAuditLog(true, command.Title, i.Id, i.FirstName, i.LastName, i.Birthdate));
            await auditLogService.AddAuditLogsAsync(auditLogs, cancellationToken);

            var cprNumbers = participants.Select(i => i.Cpr);
            await queueService.PublishAsync(new ExternalUserClearCacheMessage()
            {
                CprNumbers = cprNumbers
            }, cancellationToken);

            return Response.Ok(id);
        }
    }
}
