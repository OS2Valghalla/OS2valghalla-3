using FluentValidation;
using System.Runtime.ConstrainedExecution;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Cache;
using Valghalla.Application.Queue;
using Valghalla.Application.Queue.Messages;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.MyProfile.Interfaces;

namespace Valghalla.External.Application.Modules.MyProfile.Commands
{
    public sealed record DeleteMyProfileCommand(): ICommand<Response>;

    public sealed class DeleteMyProfileCommandValidator : AbstractValidator<DeleteMyProfileCommand>
    {
        public DeleteMyProfileCommandValidator(IUserContextProvider userContextProvider, IMyProfileQueryRepository myProfileQueryRepository)
        {
            RuleFor(x => x)
                .Must(command => !myProfileQueryRepository.CheckIfMyProfileHasAssignedTaskLocked(userContextProvider.CurrentUser.ParticipantId!.Value, default).Result)
                .WithMessage("my_profile.error.has_assigned_task_locked");

            RuleFor(x => x)
                .Must(command => !myProfileQueryRepository.CheckIfMyProfileHasCompletedTask(userContextProvider.CurrentUser.ParticipantId!.Value, default).Result)
                .WithMessage("my_profile.error.has_completed_task");
        }
    }

    internal class DeleteMyProfileCommandHandler : ICommandHandler<DeleteMyProfileCommand, Response>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly IMyProfileCommandRepository myProfileCommandRepository;
        private readonly IQueueService queueService;
        private readonly ITenantMemoryCache tenantMemoryCache;

        public DeleteMyProfileCommandHandler(
            IUserContextProvider userContextProvider,
            IMyProfileCommandRepository myProfileCommandRepository,
            IQueueService queueService,
            ITenantMemoryCache tenantMemoryCache)
        {
            this.userContextProvider = userContextProvider;
            this.myProfileCommandRepository = myProfileCommandRepository;
            this.queueService = queueService;
            this.tenantMemoryCache = tenantMemoryCache;
        }

        public async Task<Response> Handle(DeleteMyProfileCommand command, CancellationToken cancellationToken)
        {
            var user = userContextProvider.CurrentUser;

            await myProfileCommandRepository.DeleteMyProfileAsync(user.ParticipantId!.Value, cancellationToken);

            var cacheKey = UserContext.GetCacheKey(user.Cpr!);
            tenantMemoryCache.Remove(cacheKey);

            await queueService.PublishAsync(new ExternalUserClearCacheMessage()
            {
                CprNumbers = new[] { user.Cpr! }
            }, cancellationToken);

            return Response.Ok();
        }
    }
}
