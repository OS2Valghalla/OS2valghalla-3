using Microsoft.Extensions.Configuration;
using Valghalla.Database.Entities.Tables;
using Valghalla.Tools.Utils;

namespace Valghalla.Tools.Commands.AddExternalAuthCert
{
    internal class AddExternalAuthCertHandler
    {
        public IConfiguration Config { get; set; }
        private static readonly Guid _identifier = Application.Constants.FileStorage.ExternalAuthCertificate;
        private static readonly string _filename = "ExternalAuthCert";
        public AddExternalAuthCertHandler(IConfiguration Config)
        {
            this.Config = Config;
        }

        public void AddExternalAuthCert(string databaseName, string filePath, string password)
        {
            if (string.IsNullOrWhiteSpace(databaseName))
            {
                Console.WriteLine("Database name is required");
                return;
            }

            if (string.IsNullOrWhiteSpace(filePath))
            {
                Console.WriteLine("File path is required");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Password is required");
                return;
            }

            var connection = Config.GetConnectionString("Server_connection");

            if (string.IsNullOrWhiteSpace(connection))
            {
                Console.WriteLine("Connection is missing");
                return;
            }

            connection = connection.Replace("{databaseName}", databaseName);

            byte[] cert;

            try
            {
                cert = CertificateUtils.ConvertToByteArray(filePath);
            }
            catch (Exception)
            {
                Console.WriteLine("Unknown error reading certificate");
                return;
            }

            AddExternalCertificate(connection, cert);

            Console.WriteLine($$"""
                    ---------------------------------------------
                    - Certificate added!
                    - Databasename: {{databaseName}}
                    ---------------------------------------------
                    """);
        }

        private void AddExternalCertificate(string connection, byte[] cert)
        {
            var context = ContextUtils.CreateDbContext(connection);

            if (!context.Database.CanConnect())
                Console.WriteLine("Can't connect to database");


            var fileReferenceEntity = context.FileReferences
                .Where(i => i.Id == _identifier)
                .FirstOrDefault();

            using var stream = new MemoryStream(cert);
            var hash = CertificateUtils.ComputeSha256Hash(stream);
            var bytes = CertificateUtils.ConvertToBytes(stream);


            var fileEntity = new FileEntity()
            {
                Id = Guid.NewGuid(),
                Content = bytes,
                ContentHash = hash,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = new Guid("6504020c-3261-41f4-9ba7-ec380f7ad200"),
            };

            context.Files.Add(fileEntity);

            if (fileReferenceEntity != null)
            {
                fileReferenceEntity.FileId = fileEntity.Id;
                fileReferenceEntity.FileName = _filename;

                context.FileReferences.Update(fileReferenceEntity);
            }
            else
            {
                fileReferenceEntity = new FileReferenceEntity()
                {
                    Id = _identifier,
                    FileId = fileEntity.Id,
                    FileName = _filename,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = new Guid("6504020c-3261-41f4-9ba7-ec380f7ad200"),
                };

                context.FileReferences.Add(fileReferenceEntity);
            }

            context.SaveChanges();
        }
    }
}
