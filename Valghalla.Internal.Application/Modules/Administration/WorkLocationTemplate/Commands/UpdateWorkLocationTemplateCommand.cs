﻿using FluentValidation;

using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.AuditLog;
using Valghalla.Application.Queue;
using Valghalla.Application.Queue.Messages;
using Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Participant.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Commands
{
    public sealed record UpdateWorkLocationTemplateCommand() : ICommand<Response>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public Guid AreaId { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public int VoteLocation { get; set; }
        public List<Guid> TaskTypeIds { get; init; } = new List<Guid>();
        public List<Guid> TeamIds { get; init; } = new List<Guid>();
        public List<Guid> ResponsibleIds { get; init; } = new List<Guid>();
    }

    public sealed class UpdateWorkLocationTemplateCommandValidator : AbstractValidator<UpdateWorkLocationTemplateCommand>
    {
        public UpdateWorkLocationTemplateCommandValidator(IWorkLocationTemplateQueryRepository buildingQueryRepository)
        {
            RuleFor(x => x.Id)
                .NotEmpty();

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
               .Must((command) => !buildingQueryRepository.CheckIfWorkLocationTemplateExistsAsync(command, default).Result)
               .WithMessage("administration.work_location.error.work_location_exist");
        }
    }

    internal class UpdateWorkLocationTemplateCommandHandler : ICommandHandler<UpdateWorkLocationTemplateCommand, Response>
    {
        private readonly IWorkLocationTemplateCommandRepository workLocationCommandRepository;
        private readonly IParticipantSharedQueryRepository participantSharedQueryRepository;
        private readonly IAuditLogService auditLogService;
        private readonly IQueueService queueService;

        public UpdateWorkLocationTemplateCommandHandler(
            IWorkLocationTemplateCommandRepository workLocationCommandRepository,
            IParticipantSharedQueryRepository participantSharedQueryRepository,
            IAuditLogService auditLogService,
            IQueueService queueService)
        {
            this.workLocationCommandRepository = workLocationCommandRepository;
            this.participantSharedQueryRepository = participantSharedQueryRepository;
            this.auditLogService = auditLogService;
            this.queueService = queueService;
        }

        public async Task<Response> Handle(UpdateWorkLocationTemplateCommand command, CancellationToken cancellationToken)
        {
            var reuslt = await workLocationCommandRepository.UpdateWorkLocationTemplateAsync(command, cancellationToken);

            //var addedResponsibles = await participantSharedQueryRepository.GetPariticipantsAsync(new Shared.Participant.Queries.GetParticipantsSharedQuery()
            //{
            //    Values = addedResponsibleIds,
            //}, cancellationToken);

            //var removedResponsibles = await participantSharedQueryRepository.GetPariticipantsAsync(new Shared.Participant.Queries.GetParticipantsSharedQuery()
            //{
            //    Values = removedResponsibleIds,
            //}, cancellationToken);

            //var addedResponsibleAuditLogs = addedResponsibles
            //    .Select(i => new ParticipantWorkLocationResponsibleAuditLog(true, command.Title, i.Id, i.FirstName, i.LastName, i.Birthdate));

            //var removedResponsibleAuditLogs = removedResponsibles
            //    .Select(i => new ParticipantWorkLocationResponsibleAuditLog(false, command.Title, i.Id, i.FirstName, i.LastName, i.Birthdate));

            //await auditLogService.AddAuditLogsAsync(addedResponsibleAuditLogs.Concat(removedResponsibleAuditLogs), cancellationToken);

            //var cprNumbers = addedResponsibles.Concat(removedResponsibles).Select(i => i.Cpr);
            //await queueService.PublishAsync(new ExternalUserClearCacheMessage()
            //{
            //    CprNumbers = cprNumbers,
            //}, cancellationToken);

            return Response.Ok();
        }
    }
}
