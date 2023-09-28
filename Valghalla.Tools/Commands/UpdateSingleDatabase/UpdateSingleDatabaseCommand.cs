using Microsoft.Extensions.Configuration;
using System.CommandLine;

namespace Valghalla.Tools.Commands.UpdateSingleDatabase
{
    internal class UpdateSingleDatabaseCommand
    {
        private static Option<string> UpdateSingleDatabaseOption = new(new[] { "-d", "--databaseName" }, "Database name") { IsRequired = true };

        public static Command CreateCommand(IConfiguration configuration)
        {
            var updateSingleDatabaseCommand = new Command("updateSingle", "Updates one database at the given server")
            {
               UpdateSingleDatabaseOption
            };

            updateSingleDatabaseCommand.SetHandler((databaseNameValue) =>
            {
                var handler = new UpdateSingleDatabaseHandler(configuration);
                handler.UpdateSingleDatabase(databaseNameValue);
            }, UpdateSingleDatabaseOption);

            return updateSingleDatabaseCommand;
        }
    }
}
