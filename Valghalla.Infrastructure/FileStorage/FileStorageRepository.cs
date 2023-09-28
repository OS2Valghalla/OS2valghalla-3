using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Valghalla.Application.Exceptions;
using Valghalla.Application.Storage;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Infrastructure.FileStorage
{
    internal class FileStorageRepository : IFileStorageRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<FileEntity> files;
        private readonly DbSet<FileReferenceEntity> fileReferences;

        public FileStorageRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            files = dataContext.Set<FileEntity>();
            fileReferences = dataContext.Set<FileReferenceEntity>();
        }

        public async Task WriteAsync(Stream stream, Guid id, string fileName, CancellationToken cancellationToken)
        {
            var fileReferenceEntity = await fileReferences
                .Where(i => i.Id == id)
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken);

            if (fileReferenceEntity != null && fileReferenceEntity.FileId.HasValue)
            {
                throw new FileExistException(id);
            }

            var hash = await ComputeSha256HashAsync(stream, cancellationToken);
            var bytes = await ConvertToBytesAsync(stream, cancellationToken);

            var fileEntity = new FileEntity()
            {
                Id = Guid.NewGuid(),
                Content = bytes,
                ContentHash = hash
            };

            await files.AddAsync(fileEntity, cancellationToken);

            if (fileReferenceEntity != null)
            {
                fileReferenceEntity.FileId = fileEntity.Id;
                fileReferenceEntity.FileName = fileName;

                fileReferences.Update(fileReferenceEntity);
            }
            else
            {
                fileReferenceEntity = new FileReferenceEntity()
                {
                    Id = id,
                    FileId = fileEntity.Id,
                    FileName = fileName
                };

                await fileReferences.AddAsync(fileReferenceEntity, cancellationToken);
            }

            await dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<(Stream, string)> ReadAsync(Guid id, CancellationToken cancellationToken)
        {
            var fileReferenceEntity = await fileReferences
                .Where(i => i.Id == id)
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken);

            if (fileReferenceEntity == null)
            {
                throw new Application.Exceptions.FileNotFoundException(id);
            }

            var fileEntity = await files
                .Where(i => i.Id == fileReferenceEntity.FileId)
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken);

            if (fileEntity == null)
            {
                throw new Application.Exceptions.FileNotFoundException(id);
            }

            return (new MemoryStream(fileEntity.Content), fileReferenceEntity.FileName);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var fileReferenceEntity = await fileReferences
                .Where(i => i.Id == id)
                .SingleOrDefaultAsync(cancellationToken);

            if (fileReferenceEntity == null)
            {
                throw new Application.Exceptions.FileNotFoundException(id);
            }

            var fileEntity = await files
                .Where(i => i.Id == fileReferenceEntity.FileId)
                .SingleOrDefaultAsync(cancellationToken);

            if (fileEntity == null)
            {
                throw new Application.Exceptions.FileNotFoundException(id);
            }

            fileReferenceEntity.FileId = null;
            fileReferences.Update(fileReferenceEntity);
            files.Remove(fileEntity);

            await dataContext.SaveChangesAsync(cancellationToken);
        }

        private static async Task<byte[]> ConvertToBytesAsync(Stream stream, CancellationToken cancellationToken)
        {
            stream.Position = 0;

            using var bufferedStream = new BufferedStream(stream);
            using var memoryStream = new MemoryStream();
            await bufferedStream.CopyToAsync(memoryStream, cancellationToken);
            return memoryStream.ToArray();
        }

        private static async Task<string> ComputeSha256HashAsync(Stream stream, CancellationToken cancellationToken)
        {
            stream.Position = 0;

            using var sha256Hash = SHA256.Create();
            var bytes = await sha256Hash.ComputeHashAsync(stream, cancellationToken);

            var builder = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
