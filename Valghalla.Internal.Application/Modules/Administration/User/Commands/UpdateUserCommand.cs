using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.User;
using Valghalla.Internal.Application.Modules.Administration.User.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.User.Commands
{
    public sealed record UpdateUserCommand(Guid Id, Guid RoleId) : ICommand<Response>;

    public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.RoleId)
                .NotEmpty()
                .Must(roleId => Role.TryParse(roleId, out var role))
                .WithMessage("shared.user.role.id_not_valid");
        }
    }

    internal class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, Response>
    {
        private readonly IUserCommandRepository userCommandRepository;

        public UpdateUserCommandHandler(IUserCommandRepository userCommandRepository)
        {
            this.userCommandRepository = userCommandRepository;
        }

        public async Task<Response> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            await userCommandRepository.UpdateUserAsync(command, cancellationToken);
            return Response.Ok();
        }
    }
}
