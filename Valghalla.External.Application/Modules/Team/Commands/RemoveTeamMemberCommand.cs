using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.Tasks.Responses;
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
        private readonly ITeamQueryRepository teamQueryRepository;
        private readonly ITeamCommandRepository teamCommandRepository;

        public RemoveTeamMemberCommandHandler(
            IUserContextProvider userContextProvider,
            ITeamQueryRepository teamQueryRepository,
            ITeamCommandRepository teamCommandRepository)
        {
            this.userContextProvider = userContextProvider;
            this.teamQueryRepository = teamQueryRepository;
            this.teamCommandRepository = teamCommandRepository;
        }

        public async Task<Response> Handle(RemoveTeamMemberCommand command, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser.ParticipantId!.Value;

            await teamCommandRepository.RemoveTeamMemberAsync(command.TeamId, command.ParticipantId, participantId, cancellationToken);

            return Response.Ok();
        }
    }
}
