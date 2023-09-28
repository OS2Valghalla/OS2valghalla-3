using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Area.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Area.Commands
{
    public sealed record UpdateAreaCommand(Guid Id, string Name, string? Description) : ICommand<Response>;

    public sealed class UpdateAreaCommandValidator : AbstractValidator<UpdateAreaCommand>
    {
        public UpdateAreaCommandValidator(IAreaQueryRepository areaQueryRepository)
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .MaximumLength(Valghalla.Application.Constants.Validation.MaximumGeneralStringLength)
                .NotEmpty();

            RuleFor(x => x)
               .Must((command) => !areaQueryRepository.CheckIfAreaExistsAsync(command, default).Result)
               .WithMessage("administration.area.error.area_exist");
        }
    }

    internal class UpdateAreaCommandHandler : ICommandHandler<UpdateAreaCommand, Response>
    {
        private readonly IAreaCommandRepository areaCommandRepository;

        public UpdateAreaCommandHandler(IAreaCommandRepository areaCommandRepository)
        {
            this.areaCommandRepository = areaCommandRepository;
        }

        public async Task<Response> Handle(UpdateAreaCommand command, CancellationToken cancellationToken)
        {
            await areaCommandRepository.UpdateAreaAsync(command, cancellationToken);
            return Response.Ok();
        }
    }
}
