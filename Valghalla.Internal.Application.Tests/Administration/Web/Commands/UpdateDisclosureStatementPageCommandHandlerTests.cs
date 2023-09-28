using NSubstitute;
using System.Text.Json;
using Valghalla.Application;
using Valghalla.Internal.API.Requests.Administration.Web;
using Valghalla.Internal.Application.Modules.Administration.Web.Commands;
using Valghalla.Internal.Application.Modules.Administration.Web.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.Web.Commands
{
    [TestClass]
    public class UpdateDisclosureStatementPageCommandHandlerTests
    {
        private readonly IWebPageCommandRepository _mockCommandRepository;

        public UpdateDisclosureStatementPageCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IWebPageCommandRepository>();
        }

        [TestMethod]
        public async Task UpdateDisclosureStatementPageCommandHandlerTests_Should_CallUpdateWebPageAsyncOnRepository()
        {
            var request = new UpdateDisclosureStatementPageRequest()
            {
                PageContent = "Test Content"
            };
            var command = new UpdateDisclosureStatementPageCommand()
            {
                PageContent = request.PageContent
            };
            var jsonText = JsonSerializer.Serialize(command);
            var handler = new UpdateDisclosureStatementPageCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).UpdateWebPageAsync(Constants.WebPages.WebPageName_DisclosureStatementPage, jsonText, default);
        }
    }
}
