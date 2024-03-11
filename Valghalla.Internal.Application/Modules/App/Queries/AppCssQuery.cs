using Valghalla.Application;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Cache;
using Valghalla.Application.Storage;

namespace Valghalla.Internal.Application.Modules.App.Queries
{
    public sealed record AppCssQuery : IQuery<Response>;

    internal class AppCssQueryHandler : IQueryHandler<AppCssQuery>
    {
        private readonly IFileStorageService fileStorageService;
        private readonly ITenantMemoryCache tenantMemoryCache;

        public AppCssQueryHandler(IFileStorageService fileStorageService, ITenantMemoryCache tenantMemoryCache)
        {
            this.fileStorageService = fileStorageService;
            this.tenantMemoryCache = tenantMemoryCache;
        }

        public async Task<Response> Handle(AppCssQuery query, CancellationToken cancellationToken)
        {
            var key = "valghalla.css";

            var content = await tenantMemoryCache.GetOrCreateAsync(key, async () =>
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
                tenantMemoryCache.Remove(key);
            }

            return Response.Ok(content);
        }
    }
}
