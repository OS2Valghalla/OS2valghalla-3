using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Auth;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.Tasks.Interfaces;

namespace Valghalla.External.Application.Modules.Tasks.Queries
{
    public sealed record GetTaskPreviewQuery(string HashValue, Guid? InvitationCode) : IQuery<Response>;

    public sealed class GetTaskPreviewQueryValidator : AbstractValidator<GetTaskPreviewQuery>
    {
        public GetTaskPreviewQueryValidator()
        {
            RuleFor(x => x.HashValue)
                .NotEmpty();
        }
    }

    internal class GetTaskPreviewQueryHandler : IQueryHandler<GetTaskPreviewQuery>
    {
        private readonly IUserService userService;
        private readonly IUserTokenManager userTokenManager;
        private readonly ITaskQueryRepository taskQueryRepository;

        public GetTaskPreviewQueryHandler(
            IUserService userService,
            IUserTokenManager userTokenManager,
            ITaskQueryRepository taskQueryRepository)
        {
            this.userService = userService;
            this.userTokenManager = userTokenManager;
            this.taskQueryRepository = taskQueryRepository;
        }

        public async Task<Response> Handle(GetTaskPreviewQuery query, CancellationToken cancellationToken)
        {
            Guid? participantId = null;

            var token = await userTokenManager.EnsureUserTokenAsync(cancellationToken);

            if (token != null)
            {
                var userInfo = await userService.GetUserInfoAsync(token.ToClaimsPrincipal(), cancellationToken);

                if (userInfo != null)
                {
                    participantId = userInfo.ParticipantId.GetValueOrDefault();
                }
            }     

            var result = await taskQueryRepository.GetTaskPreviewAsync(query, participantId, cancellationToken);
            return Response.Ok(result);
        }
    }
}
