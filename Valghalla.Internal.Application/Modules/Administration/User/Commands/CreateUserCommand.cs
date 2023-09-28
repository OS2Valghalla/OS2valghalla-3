using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.User.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.User.Commands
{
    public sealed record CreateUserCommand(Guid RoleId, string Name, string Cvr, string Serial) : ICommand<Response<Guid>>;

    public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator(IUserQueryRepository userQueryRepository)
        {
            RuleFor(x => x.RoleId)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.Cvr)
                .NotEmpty();

            RuleFor(x => x.Serial)
                .NotEmpty();

            RuleFor(x => x)
                .Must(command => !userQueryRepository.CheckIfUserExistsAsync(command, default).Result)
                .WithMessage("administration.user.error.user_exists");
        }
    }

    internal class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Response<Guid>>
    {
        private readonly IUserCommandRepository userCommandRepository;

        public CreateUserCommandHandler(IUserCommandRepository userCommandRepository)
        {
            this.userCommandRepository = userCommandRepository;
        }

        public async Task<Response<Guid>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var result = await userCommandRepository.CreateUserAsync(command, cancellationToken);
            return Response.Ok(result);
        }
    }
}
