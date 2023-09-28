using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.Tasks.Interfaces;

namespace Valghalla.External.Application.Modules.Tasks.Queries
{
    public sealed record GetTeamResponsibleTaskPreviewQuery(string HashValue, Guid? InvitationCode) : IQuery<Response>;

    public sealed class GetTeamResponsibleTaskPreviewQueryValidator : AbstractValidator<GetTeamResponsibleTaskPreviewQuery>
    {
        public GetTeamResponsibleTaskPreviewQueryValidator()
        {
            RuleFor(x => x.HashValue)
                .NotEmpty();
        }
    }

    internal class GetTeamResponsibleTaskPreviewQueryHandler : IQueryHandler<GetTeamResponsibleTaskPreviewQuery>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly ITaskQueryRepository taskQueryRepository;

        public GetTeamResponsibleTaskPreviewQueryHandler(IUserContextProvider userContextProvider, ITaskQueryRepository taskQueryRepository)
        {
            this.userContextProvider = userContextProvider;
            this.taskQueryRepository = taskQueryRepository;
        }

        public async Task<Response> Handle(GetTeamResponsibleTaskPreviewQuery query, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser?.ParticipantId.GetValueOrDefault();
            var result = await taskQueryRepository.GetTeamResponsibleTaskPreviewAsync(query, participantId, cancellationToken);
            return Response.Ok(result);
        }
    }
}
