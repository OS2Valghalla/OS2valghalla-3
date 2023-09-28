using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Commands
{
    public sealed record UpdateSpecialDietCommand(Guid Id, string Title) : ICommand<Response>;

    public sealed class UpdateSpecialDietCommandValidator : AbstractValidator<UpdateSpecialDietCommand>
    {
        public UpdateSpecialDietCommandValidator(ISpecialDietQueryRepository specialDietQueryRepository)
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Title)
                .MaximumLength(Valghalla.Application.Constants.Validation.MaximumGeneralStringLength)
                .NotEmpty();

            RuleFor(x => x)
                .Must((command) => !specialDietQueryRepository.CheckIfSpecialDietExistsAsync(command, default).Result)
                .WithMessage("administration.specialdiet.error.specialdiet_exist");
        }
    }

    internal class UpdateSpecialDietCommandHandler : ICommandHandler<UpdateSpecialDietCommand, Response>
    {
        private readonly ISpecialDietCommandRepository specialDietCommandRepository;

        public UpdateSpecialDietCommandHandler(ISpecialDietCommandRepository specialDietCommandRepository)
        {
            this.specialDietCommandRepository = specialDietCommandRepository;
        }

        public async Task<Response> Handle(UpdateSpecialDietCommand command, CancellationToken cancellationToken)
        {
            await specialDietCommandRepository.UpdateSpecialDietAsync(command, cancellationToken);

            return Response.Ok();
        }
    }
}
