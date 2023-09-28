using Valghalla.Application;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Cache;
using Valghalla.Application.Configuration;
using Valghalla.Application.Exceptions;
using Valghalla.Application.Storage;
using Valghalla.Application.Tenant;

namespace Valghalla.Internal.Application.Modules.App.Queries
{
    public sealed record AppCssQuery : IQuery<Response>;

    internal class AppCssQueryHandler : IQueryHandler<AppCssQuery>
    {
        private readonly ITenantContextProvider tenantContextProvider;
        private readonly IFileStorageService fileStorageService;
        private readonly IAppMemoryCache memoryCache;

        public AppCssQueryHandler(
            ITenantContextProvider tenantContextProvider,
            IFileStorageService fileStorageService,
            IAppMemoryCache memoryCache,
            AppConfiguration appConfiguration)
        {
            this.tenantContextProvider = tenantContextProvider;
            this.fileStorageService = fileStorageService;
            this.memoryCache = memoryCache;
        }

        public async Task<Response> Handle(AppCssQuery query, CancellationToken cancellationToken)
        {
            var key = "valghalla.css";

            var content = await memoryCache.GetOrCreateAsync(key, async () =>
            {
                try
                {
                    var (stream, fileName) = await fileStorageService.DownloadAsync(Constants.FileStorage.InternalCss, cancellationToken);
                    var reader = new StreamReader(stream);
                    var content = await reader.ReadToEndAsync();

                    await stream.DisposeAsync();

                    return content;
                }
                catch (Valghalla.Application.Exceptions.FileNotFoundException)
                {
                    return null;
                }
            });

            if (content == null)
            {
                memoryCache.Remove(key);
            }

            return Response.Ok(content);
        }
    }
}
