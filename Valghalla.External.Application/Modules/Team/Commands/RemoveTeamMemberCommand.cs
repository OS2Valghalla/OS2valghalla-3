using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Communication;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.Team.Interfaces;

namespace Valghalla.External.Application.Modules.Team.Commands
{
    public sealed record RemoveTeamMemberCommand(Guid TeamId, Guid ParticipantId) : ICommand<Response>;

    public sealed class RemoveTeamMemberCommandValidator : AbstractValidator<RemoveTeamMemberCommand>
    {
        public RemoveTeamMemberCommandValidator()
        {
            RuleFor(x => x.TeamId)
                .NotEmpty();

            RuleFor(x => x.ParticipantId)
                .NotEmpty();
        }
    }

    internal class RemoveTeamMemberCommandHandler : ICommandHandler<RemoveTeamMemberCommand, Response>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly ITeamCommandRepository teamCommandRepository;
        private readonly ICommunicationService communicationService;

        public RemoveTeamMemberCommandHandler(
            IUserContextProvider userContextProvider,
            ITeamCommandRepository teamCommandRepository,
            ICommunicationService communicationService)
        {
            this.userContextProvider = userContextProvider;
            this.teamCommandRepository = teamCommandRepository;
            this.communicationService = communicationService;
        }

        public async Task<Response> Handle(RemoveTeamMemberCommand command, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser.ParticipantId!.Value;
            var result = await teamCommandRepository.RemoveTeamMemberAsync(command.TeamId, command.ParticipantId, participantId, cancellationToken);

            if (result != null && !result.UserRemoved)
            {
                foreach (var taskId in result.TaskIds)
                {
                    await communicationService.SendTaskInvitationRetractedAsync(command.ParticipantId, taskId, cancellationToken);
                }
            }

            return Response.Ok();
        }
    }
}
