using FluentValidation;
using Valghalla.Application;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.CPR;
using Valghalla.Application.Exceptions;
using Valghalla.External.Application.Modules.Registration.Interfaces;

namespace Valghalla.External.Application.Modules.Registration.Commands
{
    public sealed record RegisterParticipantWithTeamCommand : ICommand<Response<Guid>>
    {
        public string Cpr { get; init; } = null!;
        public string HashValue { get; init; } = null!;
        public string? MobileNumber { get; init; }
        public string? Email { get; init; }
        public IEnumerable<Guid> SpecialDietIds { get; init; } = Enumerable.Empty<Guid>();
    }

    public sealed class RegisterParticipantWithTeamCommandValidator : AbstractValidator<RegisterParticipantWithTeamCommand>
    {
        public RegisterParticipantWithTeamCommandValidator(IRegistrationQueryRepository registrationQueryRepository)
        {
            RuleFor(x => x.Cpr)
                .NotEmpty();

            RuleFor(x => x.HashValue)
                .NotEmpty();

            When(x => !string.IsNullOrEmpty(x.MobileNumber), () =>
            {
                RuleFor(x => x.MobileNumber).Length(Constants.Validation.MobileNumberLength);
            });

            When(x => !string.IsNullOrEmpty(x.Email), () =>
            {
                RuleFor(x => x.Email).EmailAddress();
            });

            RuleFor(x => x)
                .Must((command) => !registrationQueryRepository.CheckIfParticipantExistsAsync(command.Cpr, default).Result)
                .WithMessage("registration.error.participant_exists");

            RuleFor(x => x)
                .Must(command => registrationQueryRepository.CheckIfTeamExistsFromLink(command.HashValue, default).Result)
                .WithMessage("registration.error.team_not_exist");
        }
    }

    internal class RegisterParticipantWithTeamCommandHandler : ICommandHandler<RegisterParticipantWithTeamCommand, Response<Guid>>
    {
        private readonly ICPRService cprService;
        private readonly IRegistrationQueryRepository registrationQueryRepository;
        private readonly IRegistrationCommandRepository registrationCommandRepository;

        public RegisterParticipantWithTeamCommandHandler(
            ICPRService cprService,
            IRegistrationQueryRepository registrationQueryRepository,
            IRegistrationCommandRepository registrationCommandRepository)
        {
            this.cprService = cprService;
            this.registrationQueryRepository = registrationQueryRepository;
            this.registrationCommandRepository = registrationCommandRepository;
        }

        public async Task<Response<Guid>> Handle(RegisterParticipantWithTeamCommand command, CancellationToken cancellationToken)
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
            var teamId = await registrationQueryRepository.GetTeamIdFromLink(command.HashValue, cancellationToken);

            var id = await registrationCommandRepository.CreateParticipantAsync(
                command.Cpr,
                command.MobileNumber,
                command.Email,
                command.SpecialDietIds,
                teamId,
                record,
                cancellationToken);

            return Response.Ok(id);
        }
    }
}
