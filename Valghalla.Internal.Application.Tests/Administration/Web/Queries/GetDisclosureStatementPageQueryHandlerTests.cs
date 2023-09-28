using NSubstitute;
using Valghalla.Application;
using Valghalla.Internal.Application.Modules.Administration.Web.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Web.Queries;

namespace Valghalla.Internal.Application.Tests.Administration.Web.Queries
{
    [TestClass]
    public class GetDisclosureStatementPageQueryHandlerTests
    {
        private readonly IWebPageQueryRepository _mockQueryRepository;

        public GetDisclosureStatementPageQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<IWebPageQueryRepository>();
        }

        [TestMethod]
        public async Task GetDisclosureStatementPageQueryHandler_Should_CallOnRepository()
        {
            var query = new GetDisclosureStatementPageQuery();
            var handler = new GetDisclosureStatementPageQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetWebPageAsync(Constants.WebPages.WebPageName_DisclosureStatementPage, default);
        }
    }
}
