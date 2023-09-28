using NSubstitute;
using Valghalla.Application;
using Valghalla.Internal.Application.Modules.Administration.Web.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Web.Queries;

namespace Valghalla.Internal.Application.Tests.Administration.Web.Queries
{
    [TestClass]
    public class GetDeclarationOfConsentPageQueryHandlerTests
    {
        private readonly IWebPageQueryRepository _mockQueryRepository;

        public GetDeclarationOfConsentPageQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<IWebPageQueryRepository>();
        }

        [TestMethod]
        public async Task GetDeclarationOfConsentPageQueryHandler_Should_CallOnRepository()
        {
            var query = new GetDeclarationOfConsentPageQuery();
            var handler = new GetDeclarationOfConsentPageQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetWebPageAsync(Constants.WebPages.WebPageName_DeclarationOfConsentPage, default);
        }
    }
}
