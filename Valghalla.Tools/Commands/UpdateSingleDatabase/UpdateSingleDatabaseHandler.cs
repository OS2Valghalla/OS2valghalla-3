using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Valghalla.Tools.Utils;

namespace Valghalla.Tools.Commands.UpdateSingleDatabase
{
    internal class UpdateSingleDatabaseHandler
    {
        public IConfiguration Config { get; set; }
        public UpdateSingleDatabaseHandler(IConfiguration Config)
        {
            this.Config = Config;
        }

        public void UpdateSingleDatabase(string databaseName)
        {
            if (string.IsNullOrWhiteSpace(databaseName))
            {
                Console.WriteLine("Database name is required");
                return;
            }

            Console.WriteLine($$"""
                    ---------------------------------------------
                    - Will update single database
                    - Databasename: {{databaseName}}
                    ---------------------------------------------
                    """);

            var connection = Config.GetConnectionString("Server_connection");

            if (string.IsNullOrWhiteSpace(connection))
            {
                Console.WriteLine("Connection is missing");
                return;
            }

            connection = connection.Replace("{databaseName}", databaseName);
            RunMigrate(connection, databaseName);

            Console.WriteLine($$"""
                    ---------------------------------------------
                    - Database update successfully!
                    - Databasename: {{databaseName}}
                    ---------------------------------------------
                    """);
        }

        private static void RunMigrate(string connection, string databaseName)
        {
            try
            {
                var context = ContextUtils.CreateDbContext(connection);
                if (!context.Database.CanConnect())
                {
                    Console.WriteLine($"Can't connect to database: {databaseName}");
                    return;
                }

                context.Database.SetCommandTimeout(TimeSpan.FromMinutes(5));
                var appliedMigrations = context.Database.GetPendingMigrations();

                foreach (var migration in appliedMigrations)
                {
                    Console.WriteLine($"Will apply migration: {migration}");
                }

                if (!appliedMigrations.Any())
                    Console.WriteLine($"Database is up to date");

                context.Database.Migrate();
            }
            catch (Exception)
            {
                Console.WriteLine($"Error migrating database with the following name: {databaseName}");
                throw;
            }
        }
    }
}
