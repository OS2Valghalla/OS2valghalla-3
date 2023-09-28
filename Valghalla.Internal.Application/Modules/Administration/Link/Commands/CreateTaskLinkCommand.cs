using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Tenant;
using Valghalla.Internal.Application.Modules.Administration.Link.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Link.Commands
{
    public sealed record CreateTaskLinkCommand
    (Guid ElectionId, string HashValue, string Value) : ICommand<Response<string>>
    { }
    public sealed class CreateTaskLinkCommandValidator : AbstractValidator<CreateTaskLinkCommand>
    {
        public CreateTaskLinkCommandValidator()
        {
            RuleFor(x => x.ElectionId)
                .NotEmpty();

            RuleFor(x => x.HashValue)
                .NotEmpty();

            RuleFor(x => x.Value)
                .NotEmpty();
        }
    }

    internal class CreateTaskLinkCommandHandler : ICommandHandler<CreateTaskLinkCommand, Response<string>>
    {
        private readonly ITenantContextProvider tenantContextProvider;
        private readonly ILinkCommandRepository tasklinkCommandRepository;

        public CreateTaskLinkCommandHandler(ITenantContextProvider tenantContextProvider, ILinkCommandRepository tasklinkCommandRepository)
        {
            this.tenantContextProvider = tenantContextProvider;
            this.tasklinkCommandRepository = tasklinkCommandRepository;
        }

        public async Task<Response<string>> Handle(CreateTaskLinkCommand command, CancellationToken cancellationToken)
        {
            string hashValue = await tasklinkCommandRepository.CreateTaskLinkAsync(command, cancellationToken);

            return Response.Ok(tenantContextProvider.CurrentTenant.ExternalDomain.TrimEnd('/') + "/tasklink?id=" + hashValue);
        }
    }
}
