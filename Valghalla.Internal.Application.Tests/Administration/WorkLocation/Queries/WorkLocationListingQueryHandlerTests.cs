using NSubstitute;
using Valghalla.Application.QueryEngine;
using Valghalla.Application.QueryEngine.Values;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Queries;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Responses;

namespace Valghalla.Internal.Application.Tests.Administration.WorkLocation.Queries
{
    [TestClass]
    public class WorkLocationListingQueryHandlerTests
    {
        private readonly IQueryEngineRepository<WorkLocationListingQueryForm, WorkLocationResponse, WorkLocationListingQueryFormParameters> _mockQueryEngineRepository;

        public WorkLocationListingQueryHandlerTests()
        {
            _mockQueryEngineRepository = Substitute.For<IQueryEngineRepository<WorkLocationListingQueryForm, WorkLocationResponse, WorkLocationListingQueryFormParameters>>();
        }

        [TestMethod]
        public async Task WorkLocationListingQueryHandler_Should_CallExecuteQueryOnRepository()
        {
            var query = new WorkLocationListingQueryForm()
            {
                Title = new FreeTextSearchValue("Test Work Location")
            };

            var handler = new QueryEngineHandler<WorkLocationListingQueryForm, WorkLocationResponse, WorkLocationListingQueryFormParameters>(_mockQueryEngineRepository);

            await handler.Handle(query, default);

            await _mockQueryEngineRepository.Received(1).ExecuteQuery(query, default);
        }
    }
}
