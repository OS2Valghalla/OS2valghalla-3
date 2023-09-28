using NSubstitute;
using Valghalla.Application.QueryEngine;
using Valghalla.Internal.Application.Modules.Administration.Area.Queries;
using Valghalla.Internal.Application.Modules.Administration.Area.Responses;

namespace Valghalla.Internal.Application.Tests.Administration.Area.Queries
{
    [TestClass]
    public class AreaListingQueryHandlerTests
    {
        private readonly IQueryEngineRepository<AreaListingQueryForm, AreaListingItemResponse, VoidQueryFormParameters> _mockQueryEngineRepository;

        public AreaListingQueryHandlerTests()
        {
            _mockQueryEngineRepository = Substitute.For<IQueryEngineRepository<AreaListingQueryForm, AreaListingItemResponse, VoidQueryFormParameters>>();
        }

        [TestMethod]
        public async Task AreaListingQueryHandler_Should_CallExecuteQueryOnRepository()
        {
            var query = new AreaListingQueryForm()
            {
            };

            var handler = new QueryEngineHandler<AreaListingQueryForm, AreaListingItemResponse, VoidQueryFormParameters>(_mockQueryEngineRepository);
            await handler.Handle(query, default);

            await _mockQueryEngineRepository.Received(1).ExecuteQuery(query, default);
        }
    }
}
