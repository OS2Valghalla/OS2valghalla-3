using NSubstitute;
using Valghalla.Application;
using Valghalla.Internal.Application.Modules.Administration.Web.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Web.Queries;

namespace Valghalla.Internal.Application.Tests.Administration.Web.Queries
{
    [TestClass]
    public class GetFrontPageQueryHandlerTests
    {
        private readonly IWebPageQueryRepository _mockQueryRepository;

        public GetFrontPageQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<IWebPageQueryRepository>();
        }

        [TestMethod]
        public async Task GetFrontPageQueryHandler_Should_CallOnRepository()
        {
            var query = new GetFrontPageQuery();
            var handler = new GetFrontPageQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetWebPageAsync(Constants.WebPages.WebPageName_FrontPage, default);
        }
    }
}
