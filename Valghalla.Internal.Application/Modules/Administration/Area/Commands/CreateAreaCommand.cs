using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Area.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Area.Commands
{
    public sealed record CreateAreaCommand(string Name, string? Description) : ICommand<Response<Guid>>;

    public sealed class CreateAreaCommandValidator : AbstractValidator<CreateAreaCommand>
    {
        public CreateAreaCommandValidator(IAreaQueryRepository areaQueryRepository)
        {
            RuleFor(x => x.Name)
                .MaximumLength(Valghalla.Application.Constants.Validation.MaximumGeneralStringLength)
                .NotEmpty();

            RuleFor(x => x)
                .Must((command) => !areaQueryRepository.CheckIfAreaExistsAsync(command, default).Result)
                .WithMessage("administration.area.error.area_exist");
        }
    }

    internal class CreateAreaCommandHandler : ICommandHandler<CreateAreaCommand, Response<Guid>>
    {
        private readonly IAreaCommandRepository areaCommandRepository;
        public CreateAreaCommandHandler(IAreaCommandRepository areaCommandRepository)
        {
            this.areaCommandRepository = areaCommandRepository;
        }

        public async Task<Response<Guid>> Handle(CreateAreaCommand command, CancellationToken cancellationToken)
        {
            var id = await areaCommandRepository.CreateAreaAsync(command, cancellationToken);
            return Response.Ok(id);
        }
    }
}
