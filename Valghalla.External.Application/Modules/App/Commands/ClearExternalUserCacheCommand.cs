using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Cache;
using Valghalla.Application.User;

namespace Valghalla.External.Application.Modules.App.Commands
{
    public sealed record ClearExternalUserCacheCommand(IEnumerable<string> CprNumbers) : ICommand<Response>;

    internal class ClearExternalUserCacheCommandHandler : ICommandHandler<ClearExternalUserCacheCommand, Response>
    {
        private readonly IAppMemoryCache appMemoryCache;

        public ClearExternalUserCacheCommandHandler(IAppMemoryCache appMemoryCache)
        {
            this.appMemoryCache = appMemoryCache;
        }

        public async Task<Response> Handle(ClearExternalUserCacheCommand command, CancellationToken cancellationToken)
        {
            foreach (var cpr in command.CprNumbers)
            {
                var cacheKey = UserContext.GetCacheKey(cpr);
                appMemoryCache.Remove(cacheKey);
            }

            return await Task.FromResult(Response.Ok());
        }
    }
}
