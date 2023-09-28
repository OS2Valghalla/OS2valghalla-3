using Microsoft.Extensions.Configuration;
using System.CommandLine;

namespace Valghalla.Tools.Commands.AddExternalAuthCert
{
    internal class AddExternalAuthCertCommand
    {
        private static readonly Option<string> DatabaseNameOption = new(new[] { "-d", "--databaseName" }, "Database name") { IsRequired = true };
        private static readonly Option<string> FilePathOption = new(new[] { "-f", "--filePath" }, "File path to the certificate") { IsRequired = true };
        private static readonly Option<string> PasswordOption = new(new[] { "-p", "--password" }, "Password of the certificate") { IsRequired = true };


        public static Command CreateCommand(IConfiguration configuration)
        {
            var addExternalAuthCertCommand = new Command("addExternalAuthCert", "Adds the given auth certificate to the selected database")
            {
                DatabaseNameOption,
                FilePathOption,
                PasswordOption,
            };

            addExternalAuthCertCommand.SetHandler((databaseNameValue, filePathValue, passwordValue) =>
            {
                var handler = new AddExternalAuthCertHandler(configuration);
                handler.AddExternalAuthCert(databaseNameValue, filePathValue, passwordValue);
            }, DatabaseNameOption, FilePathOption, PasswordOption);

            return addExternalAuthCertCommand;
        }
    }
}
