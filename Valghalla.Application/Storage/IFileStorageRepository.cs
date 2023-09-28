namespace Valghalla.Application.Storage
{
    public interface IFileStorageRepository
    {
        Task WriteAsync(Stream stream, Guid id, string fileName, CancellationToken cancellationToken);
        Task<(Stream, string)> ReadAsync(Guid id, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
