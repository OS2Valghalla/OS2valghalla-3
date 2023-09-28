using NSubstitute;
using Valghalla.Application.QueryEngine;
using Valghalla.Internal.Application.Modules.Administration.Team.Queries;
using Valghalla.Internal.Application.Modules.Administration.Team.Responses;

namespace Valghalla.Internal.Application.Tests.Administration.Team.Queries
{
    [TestClass]
    public class TeamListingQueryHandlerTests
    {
        private readonly IQueryEngineRepository<TeamListingQueryForm, ListTeamsItemResponse, TeamListingQueryFormParameters> _mockQueryEngineRepository;

        public TeamListingQueryHandlerTests()
        {
            _mockQueryEngineRepository = Substitute.For<IQueryEngineRepository<TeamListingQueryForm, ListTeamsItemResponse, TeamListingQueryFormParameters>>();
        }

        [TestMethod]
        public async Task TeamListingQueryHandler_Should_CallExecuteQueryOnRepository()
        {
            var query = new TeamListingQueryForm()
            {
            };

            var handler = new QueryEngineHandler<TeamListingQueryForm, ListTeamsItemResponse, TeamListingQueryFormParameters>(_mockQueryEngineRepository);

            await handler.Handle(query, default);

            await _mockQueryEngineRepository.Received(1).ExecuteQuery(query, default);
        }
    }
}
