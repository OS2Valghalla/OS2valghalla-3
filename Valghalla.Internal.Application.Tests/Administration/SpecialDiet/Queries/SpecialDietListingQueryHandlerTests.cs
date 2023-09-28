using NSubstitute;
using Valghalla.Application.QueryEngine;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Queries;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Responses;

namespace Valghalla.Internal.Application.Tests.Administration.SpecialDiet.Queries
{
    [TestClass]
    public class SpecialDietListingQueryHandlerTests
    {
        private readonly IQueryEngineRepository<SpecialDietListingQueryForm, SpecialDietResponse, SpecialDietListingQueryFormParameters> _mockQueryEngineRepository;

        public SpecialDietListingQueryHandlerTests()
        {
            _mockQueryEngineRepository = Substitute.For<IQueryEngineRepository<SpecialDietListingQueryForm, SpecialDietResponse, SpecialDietListingQueryFormParameters>>();
        }

        [TestMethod]
        public async Task SpecialDietListingQueryHandler_Should_CallExecuteQueryOnRepository()
        {
            var query = new SpecialDietListingQueryForm()
            {
            };

            var handler = new QueryEngineHandler<SpecialDietListingQueryForm, SpecialDietResponse, SpecialDietListingQueryFormParameters>(_mockQueryEngineRepository);

            await handler.Handle(query, default);

            await _mockQueryEngineRepository.Received(1).ExecuteQuery(query, default);
        }
    }
}
