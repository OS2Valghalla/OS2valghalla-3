using HealthChecks.UI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;
using MimeKit;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Swashbuckle.AspNetCore.Swagger;
using System.Linq.Dynamic.Core;
using ParseException = System.Linq.Dynamic.Core.Exceptions.ParseException;

namespace Architecture.Tests
{
    [TestClass]
    public class DependencyCompatibilityTests
    {
        [TestMethod]
        public async Task HealthChecksUI_Should_PersistConfigurationUsingEFCore10()
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                EnvironmentName = Environments.Production
            });
            builder.Services.AddHealthChecksUI().AddInMemoryStorage();

            await using var app = builder.Build();
            await using var scope = app.Services.CreateAsyncScope();
            var database = scope.ServiceProvider.GetRequiredService<HealthChecksDb>();
            await database.Database.EnsureCreatedAsync();

            database.Configurations.Add(new HealthCheckConfiguration
            {
                Name = "Compatibility test",
                Uri = "http://localhost/health"
            });
            await database.SaveChangesAsync();
            database.ChangeTracker.Clear();

            var configuration = await database.Configurations.SingleAsync();
            Assert.AreEqual("Compatibility test", configuration.Name);
            Assert.AreEqual("http://localhost/health", configuration.Uri);
            Assert.AreEqual("Microsoft.EntityFrameworkCore.InMemory", database.Database.ProviderName);
        }

        [TestMethod]
        public async Task Swagger_Should_GenerateInternalApiDocument()
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                EnvironmentName = Environments.Production,
                ApplicationName = typeof(Valghalla.Internal.API.Program).Assembly.FullName
            });
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options => options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Valghalla Internal API",
                Version = "v1"
            }));

            await using var app = builder.Build();
            app.MapControllers();

            var document = app.Services.GetRequiredService<ISwaggerProvider>().GetSwagger("v1");

            Assert.AreEqual("Valghalla Internal API", document.Info.Title);
            Assert.AreEqual("v1", document.Info.Version);
            Assert.IsTrue(document.Paths.ContainsKey("/api/app/context"));
            var upload = document.Paths["/api/shared/filestorage/upload"].Operations?.Values.Single();
            Assert.IsTrue(upload?.RequestBody?.Content.ContainsKey("multipart/form-data") == true);
        }

        [TestMethod]
        public void DynamicLinq_Should_ProjectFilterSortAndPageAnalysisRows()
        {
            var values = new[]
            {
                new { Name = "Charlie", Count = 3 },
                new { Name = "Alpha", Count = 2 },
                new { Name = "Excluded", Count = 0 },
                new { Name = "Bravo", Count = 1 }
            }.AsQueryable();

            var result = values.Select("new { Name, Count }")
                .Where("Count > 0 && Name != \"Excluded\"")
                .OrderBy("Name")
                .Distinct();
            var names = result.Skip(1).Take(2).Select("Name").Cast<string>().ToArray();

            Assert.AreEqual(3, result.Count());
            CollectionAssert.AreEqual(new[] { "Bravo", "Charlie" }, names);
            Assert.ThrowsException<ParseException>(() => values.OrderBy("Name.ToString()"));
        }

        [TestMethod]
        public void Serilog_Should_RouteStructuredEventsThroughMapSink()
        {
            var sink = new CollectingSink();
            using var logger = new LoggerConfiguration()
                .WriteTo.Map("Tenant", (key, configuration) =>
                {
                    if (key == "expected")
                    {
                        configuration.Sink(sink);
                    }
                })
                .CreateLogger();

            logger.ForContext("Tenant", "other").Information("Not selected");
            logger.ForContext("Tenant", "expected").Information("Selected {Value}", 42);

            Assert.AreEqual(1, sink.Events.Count);
            Assert.AreEqual("Selected {Value}", sink.Events[0].MessageTemplate.Text);
            Assert.AreEqual(42, ((ScalarValue)sink.Events[0].Properties["Value"]).Value);
        }

        [TestMethod]
        public void MimeKit_Should_PreserveMessageWhenSerialized()
        {
            using var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Sender", "sender@example.invalid"));
            message.To.Add(new MailboxAddress("Recipient", "recipient@example.invalid"));
            message.Subject = "Compatibility test";
            message.Body = new TextPart("plain") { Text = "Message content" };
            using var stream = new MemoryStream();
            message.WriteTo(stream);
            stream.Position = 0;

            using var restored = MimeMessage.Load(stream);

            Assert.AreEqual("Compatibility test", restored.Subject);
            Assert.AreEqual("sender@example.invalid", restored.From.Mailboxes.Single().Address);
            Assert.AreEqual("recipient@example.invalid", restored.To.Mailboxes.Single().Address);
            Assert.AreEqual("Message content", restored.TextBody?.TrimEnd('\r', '\n'));
        }

        private sealed class CollectingSink : ILogEventSink
        {
            public List<LogEvent> Events { get; } = new();

            public void Emit(LogEvent logEvent)
            {
                Events.Add(logEvent);
            }
        }
    }
}
