using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Commands
{
    public sealed record CreateSpecialDietCommand(string Title) : ICommand<Response<Guid>>;

    public sealed class CreateSpecialDietCommandValidator : AbstractValidator<CreateSpecialDietCommand>
    {
        public CreateSpecialDietCommandValidator(ISpecialDietQueryRepository specialDietQueryRepository)
        {
            RuleFor(x => x.Title)
                .MaximumLength(Valghalla.Application.Constants.Validation.MaximumGeneralStringLength)
                .NotEmpty();

            RuleFor(x => x)
                .Must((command) => !specialDietQueryRepository.CheckIfSpecialDietExistsAsync(command, default).Result)
                .WithMessage("administration.specialdiet.error.specialdiet_exist");
        }
    }

    internal class CreateSpecialDietCommandHandler : ICommandHandler<CreateSpecialDietCommand, Response<Guid>>
    {
        private readonly ISpecialDietCommandRepository specialDietCommandRepository;

        public CreateSpecialDietCommandHandler(ISpecialDietCommandRepository specialDietCommandRepository)
        {
            this.specialDietCommandRepository = specialDietCommandRepository;
        }

        public async Task<Response<Guid>> Handle(CreateSpecialDietCommand command, CancellationToken cancellationToken)
        {
            var specialDietId = await specialDietCommandRepository.CreateSpecialDietAsync(command, cancellationToken);

            return Response.Ok(specialDietId);
        }
    }
}
