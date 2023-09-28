namespace Valghalla.Application.Storage
{
    public interface IFileStorageService
    {
        Task UploadAsync(Stream stream, Guid id, string fileName, CancellationToken cancellationToken);
        Task<(Stream, string)> DownloadAsync(Guid id, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
