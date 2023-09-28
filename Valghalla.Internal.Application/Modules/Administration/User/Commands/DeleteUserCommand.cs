using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.User.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.User.Commands
{
    public sealed record DeleteUserCommand(Guid Id, Guid UserId) : ICommand<Response>;

    public sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator(IUserQueryRepository userQueryRepository)
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.UserId)
                .NotEmpty();


            RuleFor(x => x)
                .Must(command => command.UserId != command.Id)
                .WithMessage("administration.user.remove_yourself");

            RuleFor(x => x)
                .Must((command) => userQueryRepository.CheckIfUserCanBeDeletedAsync(command, default).Result)
                .WithMessage("administration.user.connections");

        }
    }

    internal class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, Response>
    {
        private readonly IUserCommandRepository userCommandRepository;

        public DeleteUserCommandHandler(IUserCommandRepository userCommandRepository)
        {
            this.userCommandRepository = userCommandRepository;
        }

        public async Task<Response> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            await userCommandRepository.DeleteUserAsync(command, cancellationToken);

            return Response.Ok();
        }
    }
}
