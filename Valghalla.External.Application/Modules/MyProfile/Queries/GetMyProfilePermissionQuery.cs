using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.MyProfile.Interfaces;
using Valghalla.External.Application.Modules.MyProfile.Responses;

namespace Valghalla.External.Application.Modules.MyProfile.Queries
{
    public sealed record GetMyProfilePermissionQuery(): IQuery<Response>;

    internal class GetMyProfilePermissionQueryHandler : IQueryHandler<GetMyProfilePermissionQuery>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly IMyProfileQueryRepository myProfileQueryRepository;

        public GetMyProfilePermissionQueryHandler(
            IUserContextProvider userContextProvider,
            IMyProfileQueryRepository myProfileQueryRepository)
        {
            this.userContextProvider = userContextProvider;
            this.myProfileQueryRepository = myProfileQueryRepository;
        }

        public async Task<Response> Handle(GetMyProfilePermissionQuery query, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser.ParticipantId!.Value;
            var taskCompleted = await myProfileQueryRepository.CheckIfMyProfileHasCompletedTask(participantId, cancellationToken);
            var taskLocked = await myProfileQueryRepository.CheckIfMyProfileHasAssignedTaskLocked(participantId, cancellationToken);

            return Response.Ok(new MyProfilePermissionResponse()
            {
                TaskCompleted = taskCompleted,
                TaskLocked = taskLocked
            });
        }
    }
}
