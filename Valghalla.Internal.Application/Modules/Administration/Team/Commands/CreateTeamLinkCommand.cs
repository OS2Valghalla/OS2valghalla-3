using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Tenant;
using Valghalla.Internal.Application.Modules.Administration.Team.Interfaces;

namespace Valghalla.Internal.Application.Modules.Team.Commands
{
    public sealed record CreateTeamLinkCommand(string HashValue, string Value) : ICommand<Response<string>>
    { }
    public sealed class CreateTeamLinkCommandValidator : AbstractValidator<CreateTeamLinkCommand>
    {
        public CreateTeamLinkCommandValidator()
        {
            RuleFor(x => x.HashValue)
                .NotEmpty();

            RuleFor(x => x.Value)
                .NotEmpty();
        }
    }

    internal class CreateTeamLinkCommandHandler : ICommandHandler<CreateTeamLinkCommand, Response<string>>
    {
        private readonly ITeamCommandRepository teamCommandRepository;
        private readonly ITenantContextProvider tenantContextProvider;
        public CreateTeamLinkCommandHandler(ITeamCommandRepository teamCommandRepository, ITenantContextProvider tenantContextProvider)
        {
            this.teamCommandRepository = teamCommandRepository;
            this.tenantContextProvider = tenantContextProvider;
        }

        public async Task<Response<string>> Handle(CreateTeamLinkCommand command, CancellationToken cancellationToken)
        {
            string hashValue = await teamCommandRepository.CreateTeamLinkAsync(command, cancellationToken);

            return Response.Ok($"{tenantContextProvider.CurrentTenant.ExternalDomain}/team-link?id=" + hashValue);
        }
    }
}
