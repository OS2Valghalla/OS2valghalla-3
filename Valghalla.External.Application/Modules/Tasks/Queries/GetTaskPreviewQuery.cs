using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
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
        private readonly IUserContextProvider userContextProvider;
        private readonly ITaskQueryRepository taskQueryRepository;

        public GetTaskPreviewQueryHandler(IUserContextProvider userContextProvider, ITaskQueryRepository taskQueryRepository)
        {
            this.userContextProvider = userContextProvider;
            this.taskQueryRepository = taskQueryRepository;
        }

        public async Task<Response> Handle(GetTaskPreviewQuery query, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser?.ParticipantId.GetValueOrDefault();
            var result = await taskQueryRepository.GetTaskPreviewAsync(query, participantId, cancellationToken);
            return Response.Ok(result);
        }
    }
}
