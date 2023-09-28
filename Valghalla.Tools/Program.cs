using Microsoft.Extensions.Configuration;
using System.CommandLine;
using Valghalla.Tools.Commands.AddExternalAuthCert;
using Valghalla.Tools.Commands.AddInternalAuthCert;
using Valghalla.Tools.Commands.UpdateSingleDatabase;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json").Build();

var rootCommand = new RootCommand
{
    UpdateSingleDatabaseCommand.CreateCommand(config),
    AddExternalAuthCertCommand.CreateCommand(config),
    AddInternalAuthCertCommand.CreateCommand(config)
};

await rootCommand.InvokeAsync(args);