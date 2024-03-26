using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Tenant;
using Valghalla.Application.User;
using Valghalla.Internal.Application.Modules.App.Interfaces;
using Valghalla.Internal.Application.Modules.App.Responses;

namespace Valghalla.Internal.Application.Modules.App.Queries
{
    public sealed record GetAppContextQuery(Guid? ElectionId): IQuery<Response>;

    internal class GetAppContextQueryHandler : IQueryHandler<GetAppContextQuery>
    {
        private readonly ITenantContextProvider tenantContextProvider;
        private readonly IUserContextProvider userContextProvider;
        private readonly IAppElectionQueryRepository appElectionQueryRepository;

        public GetAppContextQueryHandler(ITenantContextProvider tenantContextProvider, IUserContextProvider userContextProvider, IAppElectionQueryRepository appElectionQueryRepository)
        {
            this.tenantContextProvider = tenantContextProvider;
            this.userContextProvider = userContextProvider;
            this.appElectionQueryRepository = appElectionQueryRepository;
        }

        public async Task<Response> Handle(GetAppContextQuery query, CancellationToken cancellationToken)
        {
            AppElectionResponse? electionToWorkOn = null;

            if (query.ElectionId.HasValue)
            {
                electionToWorkOn = await appElectionQueryRepository.GetElectionToWorkOnAsync(query.ElectionId.Value, cancellationToken);
            }

            if (electionToWorkOn == null)
            {
                electionToWorkOn = await appElectionQueryRepository.GetDefaultElectionToWorkOnAsync(cancellationToken);
            }

            var appContext = new AppContextResponse()
            {
                User = new UserInfo()
                {
                    Id = userContextProvider.CurrentUser.UserId,
                    RoleIds = userContextProvider.CurrentUser.RoleIds,
                    Name = userContextProvider.CurrentUser.Name,
                },
                Election = electionToWorkOn,
                MunicipalityName = tenantContextProvider.CurrentTenant.Name,
                ExternalWebUrl = tenantContextProvider.CurrentTenant.ExternalDomain
            };

            return Response.Ok(appContext);
        }
    }
}
