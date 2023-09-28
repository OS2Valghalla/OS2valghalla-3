using NSubstitute;
using System.Text.Json;
using Valghalla.Application;
using Valghalla.Internal.API.Requests.Administration.Web;
using Valghalla.Internal.Application.Modules.Administration.Web.Commands;
using Valghalla.Internal.Application.Modules.Administration.Web.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.Web.Commands
{
    [TestClass]
    public class UpdateFrontPageCommandHandlerTests
    {
        private readonly IWebPageCommandRepository _mockCommandRepository;

        public UpdateFrontPageCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IWebPageCommandRepository>();
        }

        [TestMethod]
        public async Task UpdateFrontPageCommandHandlerTests_Should_CallUpdateWebPageAsyncOnRepository()
        {
            var request = new UpdateFrontPageRequest()
            {
                PageContent = "Test Content",
                Title = "Test Title"
            };
            var command = new UpdateFrontPageCommand()
            {
                PageContent = request.PageContent,
                Title = request.Title,
            };
            var jsonText = JsonSerializer.Serialize(command);
            var handler = new UpdateFrontPageCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).UpdateWebPageAsync(Constants.WebPages.WebPageName_FrontPage, jsonText, default);
        }
    }
}
