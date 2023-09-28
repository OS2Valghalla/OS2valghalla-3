using NSubstitute;
using Valghalla.Application.QueryEngine;
using Valghalla.Internal.Application.Modules.Administration.Election.Queries;
using Valghalla.Internal.Application.Modules.Administration.Election.Responses;

namespace Valghalla.Internal.Application.Tests.Administration.Election.Queries
{
    [TestClass]
    public class ElectionListingQueryHandlerTests
    {
        private readonly IQueryEngineRepository<ElectionListingQueryForm, ElectionListingItemResponse, VoidQueryFormParameters> _mockQueryEngineRepository;

        public ElectionListingQueryHandlerTests()
        {
            _mockQueryEngineRepository = Substitute.For<IQueryEngineRepository<ElectionListingQueryForm, ElectionListingItemResponse, VoidQueryFormParameters>>();
        }

        [TestMethod]
        public async Task ElectionListingQueryHandler_Should_CallExecuteQueryOnRepository()
        {
            var query = new ElectionListingQueryForm()
            {
            };

            var handler = new QueryEngineHandler<ElectionListingQueryForm, ElectionListingItemResponse, VoidQueryFormParameters>(_mockQueryEngineRepository);

            await handler.Handle(query, default);

            await _mockQueryEngineRepository.Received(1).ExecuteQuery(query, default);
        }
    }
}
