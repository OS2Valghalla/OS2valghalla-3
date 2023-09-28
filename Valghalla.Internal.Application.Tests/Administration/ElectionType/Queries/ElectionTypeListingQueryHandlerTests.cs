using NSubstitute;
using Valghalla.Application.QueryEngine;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Queries;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Responses;

namespace Valghalla.Internal.Application.Tests.Administration.ElectionType.Queries
{
    [TestClass]
    public class ElectionTypeListingQueryHandlerTests
    {
        private readonly IQueryEngineRepository<ElectionTypeListingQueryForm, ElectionTypeResponse, VoidQueryFormParameters> _mockQueryEngineRepository;

        public ElectionTypeListingQueryHandlerTests()
        {
            _mockQueryEngineRepository = Substitute.For<IQueryEngineRepository<ElectionTypeListingQueryForm, ElectionTypeResponse, VoidQueryFormParameters>>();
        }

        [TestMethod]
        public async Task ElectionTypeListingQueryHandler_Should_CallExecuteQueryOnRepository()
        {
            var query = new ElectionTypeListingQueryForm()
            {
            };

            var handler = new QueryEngineHandler<ElectionTypeListingQueryForm, ElectionTypeResponse, VoidQueryFormParameters>(_mockQueryEngineRepository);

            await handler.Handle(query, default);

            await _mockQueryEngineRepository.Received(1).ExecuteQuery(query, default);
        }
    }
}
