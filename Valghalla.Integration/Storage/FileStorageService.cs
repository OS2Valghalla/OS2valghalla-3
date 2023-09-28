using Microsoft.Extensions.Logging;
using Valghalla.Application.Storage;

namespace Valghalla.Integration.Storage
{
    internal class FileStorageService : IFileStorageService
    {
        private readonly ILogger<FileStorageService> logger;
        private readonly IFileStorageRepository fileStorageRepository;

        public FileStorageService(ILogger<FileStorageService> logger, IFileStorageRepository fileStorageRepository)
        {
            this.logger = logger;
            this.fileStorageRepository = fileStorageRepository;
        }

        public Task UploadAsync(Stream stream, Guid id, string fileName, CancellationToken cancellationToken)
        {
            try
            {
                return fileStorageRepository.WriteAsync(stream, id, fileName, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError("Upload file failed -- Exception: {@ex}", ex);
                throw;
            }
        }

        public Task<(Stream, string)> DownloadAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                return fileStorageRepository.ReadAsync(id, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError("Download file failed -- Exception: {@ex}", ex);
                throw;
            }
        }

        public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                return fileStorageRepository.DeleteAsync(id, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError("Delete file failed -- Exception: {@ex}", ex);
                throw;
            }
        }
    }
}
