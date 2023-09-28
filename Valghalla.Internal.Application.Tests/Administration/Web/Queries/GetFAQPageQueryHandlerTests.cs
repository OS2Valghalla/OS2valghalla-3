using NSubstitute;
using Valghalla.Application;
using Valghalla.Internal.Application.Modules.Administration.Web.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Web.Queries;

namespace Valghalla.Internal.Application.Tests.Administration.Web.Queries
{
    [TestClass]
    public class GetFAQPageQueryHandlerTests
    {
        private readonly IWebPageQueryRepository _mockQueryRepository;

        public GetFAQPageQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<IWebPageQueryRepository>();
        }

        [TestMethod]
        public async Task GetFAQPageQueryHandler_Should_CallOnRepository()
        {
            var query = new GetFAQPageQuery();
            var handler = new GetFAQPageQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetWebPageAsync(Constants.WebPages.WebPageName_FAQPage, default);
        }
    }
}
