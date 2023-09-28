using NSubstitute;
using Valghalla.Application;
using Valghalla.Internal.Application.Modules.Administration.Web.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Web.Queries;

namespace Valghalla.Internal.Application.Tests.Administration.Web.Queries
{
    [TestClass]
    public class GetElectionCommitteeContactInformationQueryHandlerTests
    {
        private readonly IElectionCommitteeContactInformationQueryRepository _mockQueryRepository;

        public GetElectionCommitteeContactInformationQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<IElectionCommitteeContactInformationQueryRepository>();
        }

        [TestMethod]
        public async Task GetElectionCommitteeContactInformationQueryHandler_Should_CallOnRepository()
        {
            var query = new GetElectionCommitteeContactInformationQuery();
            var handler = new GetElectionCommitteeContactInformationQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetWebPageAsync(Constants.WebPages.WebPageName_ElectionCommitteeContactInformation, default);
        }
    }
}
