using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Confirmations;
using Valghalla.Internal.Application.Modules.Administration.Area.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Area.Commands
{
    public sealed record DeleteAreaCommand(Guid Id) : ConfirmationCommand<Response>;

    public sealed class DeleteAreaCommandValidator : AbstractValidator<DeleteAreaCommand>
    {
        public DeleteAreaCommandValidator(IAreaQueryRepository areaQueryRepository)
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x)
               .Must((command) => !areaQueryRepository.CheckIfAreaHasWorkLocationsAsync(command, default).Result)
               .WithMessage("administration.area.error.work_location_used");

            RuleFor(x => x)
               .Must((command) => !areaQueryRepository.CheckIfAreaIsLastOneAsync(command, default).Result)
               .WithMessage("administration.area.error.minimum_area_required");
        }
    }

    public sealed class DeleteAreaCommandConfirmator : Confirmator<DeleteAreaCommand>
    {
        public override string Title => "administration.area.delete_area_dialog.title";
        public override string Message => "administration.area.delete_area_dialog.content";
    }

    internal class DeleteAreaCommandHandler : ICommandHandler<DeleteAreaCommand, Response>
    {
        private readonly IAreaCommandRepository areaQueryRepository;

        public DeleteAreaCommandHandler(IAreaCommandRepository areaQueryRepository)
        {
            this.areaQueryRepository = areaQueryRepository;
        }

        public async Task<Response> Handle(DeleteAreaCommand command, CancellationToken cancellationToken)
        {
            await areaQueryRepository.DeleteAreaAsync(command, cancellationToken);
            return Response.Ok();
        }
    }
}
