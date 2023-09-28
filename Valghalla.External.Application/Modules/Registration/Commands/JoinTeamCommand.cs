using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.Registration.Interfaces;

namespace Valghalla.External.Application.Modules.Registration.Commands
{
    public sealed record JoinTeamCommand(string HashValue) : ICommand<Response>;

    public sealed class JoinTeamCommandValidator : AbstractValidator<JoinTeamCommand>
    {
        public JoinTeamCommandValidator(IRegistrationQueryRepository registrationQueryRepository)
        {
            RuleFor(x => x.HashValue)
                .NotEmpty();

            RuleFor(x => x)
                .Must(query => registrationQueryRepository.CheckIfTeamExistsFromLink(query.HashValue, default).Result)
                .WithMessage("registration.error.team_not_exist");
        }
    }

    internal class JoinTeamCommandHandler : ICommandHandler<JoinTeamCommand, Response>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly IRegistrationQueryRepository registrationQueryRepository;
        private readonly IRegistrationCommandRepository registrationCommandRepository;

        public JoinTeamCommandHandler(
            IUserContextProvider userContextProvider,
            IRegistrationQueryRepository registrationQueryRepository,
            IRegistrationCommandRepository registrationCommandRepository)
        {
            this.userContextProvider = userContextProvider;
            this.registrationQueryRepository = registrationQueryRepository;
            this.registrationCommandRepository = registrationCommandRepository;
        }

        public async Task<Response> Handle(JoinTeamCommand command, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser.ParticipantId!.Value;
            var teamId = await registrationQueryRepository.GetTeamIdFromLink(command.HashValue, cancellationToken);
            await registrationCommandRepository.JoinTeamAsync(participantId, teamId, cancellationToken);

            return Response.Ok();
        }
    }
}
