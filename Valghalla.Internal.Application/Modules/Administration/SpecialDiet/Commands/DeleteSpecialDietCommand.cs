using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Confirmations;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Commands
{
    public sealed record DeleteSpecialDietCommand(Guid Id) : ConfirmationCommand<Response>;

    public sealed class DeleteSpecialDietCommandValidator : AbstractValidator<DeleteSpecialDietCommand>
    {
        public DeleteSpecialDietCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    public sealed class DeleteSpecialDietCommandConfirmator : Confirmator<DeleteSpecialDietCommand>
    {
        public override string Title => "administration.specialdiet.action.delete_confirmation.title";
        public override string Message => "administration.specialdiet.action.delete_confirmation.content_noActiveElection";

        public DeleteSpecialDietCommandConfirmator(ISpecialDietQueryRepository specialDietQueryRepository)
        {
            WhenAsync(async (command, cancellationToken) => await specialDietQueryRepository.CheckHasActiveElectionAsync(cancellationToken))
                .WithMessage("administration.specialdiet.action.delete_confirmation.content_hasActiveElection");
        }
    }


    internal class DeleteSpecialDietCommandHandler : ICommandHandler<DeleteSpecialDietCommand, Response>
    {
        private readonly ISpecialDietCommandRepository specialDietCommandRepository;

        public DeleteSpecialDietCommandHandler(ISpecialDietCommandRepository specialDietCommandRepository)
        {
            this.specialDietCommandRepository = specialDietCommandRepository;
        }

        public async Task<Response> Handle(DeleteSpecialDietCommand command, CancellationToken cancellationToken)
        {
            await specialDietCommandRepository.DeleteSpecialDietAsync(command, cancellationToken);

            return Response.Ok();
        }
    }
}
