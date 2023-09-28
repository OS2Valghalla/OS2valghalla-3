using NSubstitute;
using System.Text.Json;
using Valghalla.Application;
using Valghalla.Internal.API.Requests.Administration.Web;
using Valghalla.Internal.Application.Modules.Administration.Web.Commands;
using Valghalla.Internal.Application.Modules.Administration.Web.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.Web.Commands
{
    [TestClass]
    public class UpdateDeclarationOfConsentPageCommandHandlerTests
    {
        private readonly IWebPageCommandRepository _mockCommandRepository;

        public UpdateDeclarationOfConsentPageCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IWebPageCommandRepository>();
        }

        [TestMethod]
        public async Task UpdateDeclarationOfConsentPageCommandHandlerTests_Should_CallUpdateWebPageAsyncOnRepository()
        {
            var request = new UpdateDeclarationOfConsentPageRequest()
            {
                PageContent = "Test Content",
                IsActivated = true
            };
            var command = new UpdateDeclarationOfConsentPageCommand()
            {
                PageContent = request.PageContent,
                IsActivated = request.IsActivated
            };
            var jsonText = JsonSerializer.Serialize(command);
            var handler = new UpdateDeclarationOfConsentPageCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).UpdateWebPageAsync(Constants.WebPages.WebPageName_DeclarationOfConsentPage, jsonText, default);
        }
    }
}
