using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Tenant;
using Valghalla.Internal.Application.Modules.Administration.Link.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Link.Commands
{
    public sealed record CreateTasksFilteredLinkCommand
    (Guid ElectionId, string HashValue, string Value) : ICommand<Response<string>>
    { }
    public sealed class CreateTasksFilteredLinkCommandValidator : AbstractValidator<CreateTasksFilteredLinkCommand>
    {
        public CreateTasksFilteredLinkCommandValidator()
        {
            RuleFor(x => x.ElectionId)
                .NotEmpty();

            RuleFor(x => x.HashValue)
                .NotEmpty();

            RuleFor(x => x.Value)
                .NotEmpty();
        }
    }

    internal class CreateTasksFilteredLinkCommandHandler : ICommandHandler<CreateTasksFilteredLinkCommand, Response<string>>
    {
        private readonly ITenantContextProvider tenantContextProvider;
        private readonly ILinkCommandRepository tasksFilteredlinkCommandRepository;

        public CreateTasksFilteredLinkCommandHandler(ITenantContextProvider tenantContextProvider, ILinkCommandRepository tasksFilteredlinkCommandRepository)
        {
            this.tenantContextProvider = tenantContextProvider;
            this.tasksFilteredlinkCommandRepository = tasksFilteredlinkCommandRepository;
        }

        public async Task<Response<string>> Handle(CreateTasksFilteredLinkCommand command, CancellationToken cancellationToken)
        {
            string hashValue = await tasksFilteredlinkCommandRepository.CreateTasksFilteredLinkAsync(command, cancellationToken);

            return Response.Ok(tenantContextProvider.CurrentTenant.ExternalDomain.TrimEnd('/') + "/opgave-link?id=" + hashValue);
        }
    }
}
