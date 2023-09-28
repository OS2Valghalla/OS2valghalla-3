using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Communication;

namespace Valghalla.Internal.Application.Modules.Administration.Communication.Commands
{
    public sealed record SendGroupMessageCommand() : ICommand<Response<bool>>
    {
        public Guid ElectionId { get; init; }
        public string Subject { get; init; }
        public string Content { get; init; }
        public int TemplateType { get; init; }
        public IEnumerable<Guid> FileReferenceIds { get; init; } = Enumerable.Empty<Guid>();
        public IEnumerable<Guid> WorkLocationIds { get; init; } = Enumerable.Empty<Guid>();
        public IEnumerable<Guid> TeamIds { get; init; } = Enumerable.Empty<Guid>();
        public IEnumerable<Guid> TaskTypeIds { get; init; } = Enumerable.Empty<Guid>();
        public IEnumerable<Valghalla.Application.Enums.TaskStatus> TaskStatuses { get; set; } = Enumerable.Empty<Valghalla.Application.Enums.TaskStatus>();
        public IEnumerable<DateTime> TaskDates { get; init; } = Enumerable.Empty<DateTime>();
    }

    public sealed class SendGroupMessageCommandValidator : AbstractValidator<SendGroupMessageCommand>
    {
        public SendGroupMessageCommandValidator()
        {
            RuleFor(x => x.ElectionId)
               .NotEmpty();

            RuleFor(x => x.Content)
               .NotEmpty();
        }
    }

    internal class SendGroupMessageCommandHandler : ICommandHandler<SendGroupMessageCommand, Response<bool>>
    {
        private readonly ICommunicationService communicationService;
        private readonly ICommunicationQueryRepository communicationQueryRepository;
        private readonly Interfaces.ICommunicationQueryRepository taskCommunicationQueryRepository;

        public SendGroupMessageCommandHandler(
            ICommunicationService communicationService,
            ICommunicationQueryRepository communicationQueryRepository,
            Interfaces.ICommunicationQueryRepository taskCommunicationQueryRepository)
        {
            this.communicationService = communicationService;
            this.communicationQueryRepository = communicationQueryRepository;
            this.taskCommunicationQueryRepository = taskCommunicationQueryRepository;
        }

        public async Task<Response<bool>> Handle(SendGroupMessageCommand command, CancellationToken cancellationToken)
        {
            var result = true;

            var tasksInfo = await taskCommunicationQueryRepository.GetTasksForSendingGroupMessageAsync(command.ElectionId, command.WorkLocationIds, command.TeamIds, command.TaskTypeIds, command.TaskStatuses, command.TaskDates, cancellationToken);

            await communicationService.SendGroupMessageAsync(tasksInfo, command.TemplateType, command.Subject, command.Content, command.FileReferenceIds.ToList(), cancellationToken);

            return Response.Ok(result);
        }
    }
}
