using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.Registration.Interfaces;

namespace Valghalla.External.Application.Modules.Registration.Queries
{
    public sealed record GetMyProfileRegistrationQuery() : IQuery<Response>;

    internal class GetParticipantProfileQueryHandler : IQueryHandler<GetMyProfileRegistrationQuery>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly IRegistrationQueryRepository registrationQueryRepository;

        public GetParticipantProfileQueryHandler(IUserContextProvider userContextProvider, IRegistrationQueryRepository registrationQueryRepository)
        {
            this.userContextProvider = userContextProvider;
            this.registrationQueryRepository = registrationQueryRepository;
        }

        public async Task<Response> Handle(GetMyProfileRegistrationQuery query, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser.ParticipantId!.Value;
            var result = await registrationQueryRepository.GetMyProfileRegistrationAsync(participantId, cancellationToken);

            return Response.Ok(result);
        }
    }
}
