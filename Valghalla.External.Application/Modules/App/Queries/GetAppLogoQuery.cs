using Valghalla.Application;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Cache;
using Valghalla.Application.Storage;

namespace Valghalla.External.Application.Modules.App.Queries
{
    public sealed record GetAppLogoQuery() : IQuery<Response>;

    internal class GetAppLogoQueryHandler : IQueryHandler<GetAppLogoQuery>
    {
        private readonly IAppMemoryCache appMemoryCache;
        private readonly IFileStorageService fileStorageService;

        public GetAppLogoQueryHandler(
            IAppMemoryCache appMemoryCache,
            IFileStorageService fileStorageService)
        {
            this.appMemoryCache = appMemoryCache;
            this.fileStorageService = fileStorageService;
        }

        public async Task<Response> Handle(GetAppLogoQuery query, CancellationToken cancellationToken)
        {
            var key = "MunicipalityLogo";

            var content = await appMemoryCache.GetOrCreateAsync(key, async () =>
            {
                try
                {
                    var (stream, fileName) = await fileStorageService.DownloadAsync(Constants.FileStorage.MunicipalityLogo, cancellationToken);
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
                appMemoryCache.Remove(key);
            }

            return Response.Ok(content);
        }
    }
}
