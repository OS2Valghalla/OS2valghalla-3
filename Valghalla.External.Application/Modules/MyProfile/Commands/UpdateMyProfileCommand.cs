using FluentValidation;
using Valghalla.Application;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.MyProfile.Interfaces;

namespace Valghalla.External.Application.Modules.MyProfile.Commands
{
    public sealed record UpdateMyProfileCommand : ICommand<Response>
    {
        public string? MobileNumber { get; init; }
        public string? Email { get; init; }
        public IEnumerable<Guid> SpecialDietIds { get; init; } = Enumerable.Empty<Guid>();
    }

    public sealed class UpdateMyProfileCommandValidator : AbstractValidator<UpdateMyProfileCommand>
    {
        public UpdateMyProfileCommandValidator()
        {
            RuleFor(x => x.MobileNumber)
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotEmpty();

            When(x => !string.IsNullOrEmpty(x.MobileNumber), () =>
            {
                RuleFor(x => x.MobileNumber)
                .Length(Constants.Validation.MobileNumberLength)
                .Matches("^[0-9]*$");
            });

            When(x => !string.IsNullOrEmpty(x.Email), () =>
            {
                RuleFor(x => x.Email).EmailAddress();
            });
        }
    }

    internal class UpdateMyProfileCommandHandler : ICommandHandler<UpdateMyProfileCommand, Response>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly IMyProfileCommandRepository myProfileCommandRepository;

        public UpdateMyProfileCommandHandler(IUserContextProvider userContextProvider, IMyProfileCommandRepository myProfileCommandRepository)
        {
            this.userContextProvider = userContextProvider;
            this.myProfileCommandRepository = myProfileCommandRepository;
        }

        public async Task<Response> Handle(UpdateMyProfileCommand command, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser.ParticipantId!.Value;

            await myProfileCommandRepository.UpdateMyProfileAsync(participantId, command, cancellationToken);

            return Response.Ok();
        }
    }
}
