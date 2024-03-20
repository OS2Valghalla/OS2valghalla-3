using FluentValidation;
using Valghalla.Application;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Communication;
using Valghalla.Application.CPR;
using Valghalla.Application.Exceptions;
using Valghalla.Application.TaskValidation;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Commands;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;

namespace Valghalla.Internal.Application.Modules.Participant.Commands
{
    public sealed record CreateParticipantCommand() : ICommand<Response<Guid>>
    {
        public string Cpr { get; init; } = null!;
        public string? MobileNumber { get; init; }
        public string? Email { get; init; }
        public IEnumerable<Guid> SpecialDietIds { get; init; } = Enumerable.Empty<Guid>();
        public IEnumerable<Guid> TeamIds { get; init; } = Enumerable.Empty<Guid>();
        public Guid? TaskId { get; init; }
        public Guid? ElectionId { get; init; }
    }

    public sealed class CreateParticipantCommandValidator : AbstractValidator<CreateParticipantCommand>
    {
        public CreateParticipantCommandValidator(IParticipantQueryRepository participantQueryRepository, ITaskValidationService taskValidationService)
        {
            RuleFor(x => x.Cpr)
                .NotEmpty();

            When(x => !string.IsNullOrEmpty(x.MobileNumber), () =>
            {
                RuleFor(x => x.MobileNumber)
                .Length(Constants.Validation.MobileNumberLength).WithMessage("participant.error.mobile_number_invalid")
                .Matches("^[0-9]*$").WithMessage("participant.error.mobile_number_contain_only_numbers");
            });

            When(x => !string.IsNullOrEmpty(x.Email), () =>
            {
                RuleFor(x => x.Email).EmailAddress();
            });

            RuleFor(x => x.TeamIds)
                .NotEmpty();

            RuleFor(x => x.Cpr)
                .Must(CprValidator.IsCprValid)
                .WithMessage("participant.error.cpr_invalid");

            RuleFor(x => x)
                .Must((command) => !participantQueryRepository.CheckIfParticipantExistsAsync(command.Cpr, default).Result)
                .WithMessage("participant.error.participant_exists");

            When(x => x.TaskId.HasValue && x.ElectionId.HasValue, () =>
            {
                RuleFor(x => taskValidationService.ExecuteAsync(x.TaskId!.Value, x.ElectionId!.Value, x.Cpr, default).Result).Custom((result, context) =>
                {
                    if (!result.IsAlive())
                    {
                        context.AddFailure("tasks.error.validation_rules_alive");
                    }
                    if (!result.IsAge18OrOver())
                    {
                        context.AddFailure("tasks.error.validation_rules_age");
                    }
                    if (!result.IsResidencyMunicipality())
                    {
                        context.AddFailure("tasks.error.validation_rules_municipality");
                    }
                    if (!result.IsDisenfranchised())
                    {
                        context.AddFailure("tasks.error.validation_rules_legal_adult");
                    }
                    if (!result.HasDanishCitizenship())
                    {
                        context.AddFailure("tasks.error.validation_rules_citizenship");
                    }
                });
            });            
        }
    }

    internal class CreateParticipantCommandHandler : ICommandHandler<CreateParticipantCommand, Response<Guid>>
    {
        private readonly ICPRService cprService;
        private readonly ICommunicationService communicationService;
        private readonly IParticipantCommandRepository participantCommandRepository;
        private readonly IElectionWorkLocationTasksCommandRepository electionWorkLocationTasksCommandRepository;

        public CreateParticipantCommandHandler(ICPRService cprService, ICommunicationService communicationService, IParticipantCommandRepository participantCommandRepository, IElectionWorkLocationTasksCommandRepository electionWorkLocationTasksCommandRepository)
        {
            this.cprService = cprService;
            this.communicationService = communicationService;
            this.participantCommandRepository = participantCommandRepository;
            this.electionWorkLocationTasksCommandRepository = electionWorkLocationTasksCommandRepository;
        }

        public async Task<Response<Guid>> Handle(CreateParticipantCommand command, CancellationToken cancellationToken)
        {
            CprPersonInfo cprPersonInfo;

            try
            {
                cprPersonInfo = await cprService.ExecuteAsync(command.Cpr);
            }
            catch (CvrInputInvalidException)
            {
                return Response.Fail<Guid>("participant.error.cvr_invalid");
            }
            catch (Exception ex)
            {
                return Response.Fail<Guid>(ex.Message);
            }

            var record = cprPersonInfo.ToRecord();
            var id = await participantCommandRepository.CreateParticipantAsync(command, record, cancellationToken);

            if (command.TaskId.HasValue && command.ElectionId.HasValue)
            {
                AssignParticipantToTaskCommand assignTaskCommand = new AssignParticipantToTaskCommand
                {
                    TaskAssignmentId = command.TaskId.Value,
                    ElectionId = command.ElectionId.Value,
                    ParticipantId = id
                };

                await electionWorkLocationTasksCommandRepository.AssignCreatingParticipantToTaskAsync(assignTaskCommand, command.TeamIds, cancellationToken);

                await communicationService.SendTaskInvitationAsync(assignTaskCommand.ParticipantId, assignTaskCommand.TaskAssignmentId, cancellationToken);
            }

            return Response.Ok(id);
        }
    }
}
