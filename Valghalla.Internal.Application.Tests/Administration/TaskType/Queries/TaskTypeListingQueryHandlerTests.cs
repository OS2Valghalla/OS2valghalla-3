using NSubstitute;
using Valghalla.Application.QueryEngine;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Queries;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Responses;

namespace Valghalla.Internal.Application.Tests.Administration.TaskType.Queries
{
    [TestClass]
    public class TaskTypeListingQueryHandlerTests
    {
        private readonly IQueryEngineRepository<TaskTypeListingQueryForm, TaskTypeListingItemResponse, VoidQueryFormParameters> _mockQueryEngineRepository;

        public TaskTypeListingQueryHandlerTests()
        {
            _mockQueryEngineRepository = Substitute.For<IQueryEngineRepository<TaskTypeListingQueryForm, TaskTypeListingItemResponse, VoidQueryFormParameters>>();
        }

        [TestMethod]
        public async Task TaskTypeListingQueryHandler_Should_CallExecuteQueryOnRepository()
        {
            var query = new TaskTypeListingQueryForm()
            {
            };

            var handler = new QueryEngineHandler<TaskTypeListingQueryForm, TaskTypeListingItemResponse, VoidQueryFormParameters>(_mockQueryEngineRepository);

            await handler.Handle(query, default);

            await _mockQueryEngineRepository.Received(1).ExecuteQuery(query, default);
        }
    }
}
