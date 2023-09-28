using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.MyProfile.Interfaces;

namespace Valghalla.External.Application.Modules.MyProfile.Queries
{
    public sealed record GetMyProfileQuery(): IQuery<Response>;

    internal class GetMyProfileQueryHandler : IQueryHandler<GetMyProfileQuery>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly IMyProfileQueryRepository myProfileQueryRepository;

        public GetMyProfileQueryHandler(IUserContextProvider userContextProvider, IMyProfileQueryRepository myProfileQueryRepository)
        {
            this.userContextProvider = userContextProvider;
            this.myProfileQueryRepository = myProfileQueryRepository;
        }

        public async Task<Response> Handle(GetMyProfileQuery query, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser.ParticipantId!.Value;
            var result = await myProfileQueryRepository.GetMyProfileAsync(participantId, cancellationToken);
            return Response.Ok(result);
        }
    }
}
