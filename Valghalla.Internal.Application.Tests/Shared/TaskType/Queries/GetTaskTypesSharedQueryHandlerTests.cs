using NSubstitute;
using Valghalla.Internal.Application.Modules.Shared.TaskType.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.TaskType.Queries;

namespace Valghalla.Internal.Application.Tests.Shared.TaskType.Queries
{
    [TestClass]
    public class GetTaskTypesSharedQueryHandlerTests
    {
        private readonly ITaskTypeSharedQueryRepository _mockTaskTypeSharedQueryRepository;

        public GetTaskTypesSharedQueryHandlerTests()
        {
            _mockTaskTypeSharedQueryRepository = Substitute.For<ITaskTypeSharedQueryRepository>();
        }

        [TestMethod]
        public async Task GetTaskTypesSharedQueryHandler_Should_CallGetTaskTypesAsyncOnRepository()
        {
            var query = new GetTaskTypesSharedQuery();
            var handler = new GetTaskTypesSharedQueryHandler(_mockTaskTypeSharedQueryRepository);

            await handler.Handle(query, default);

            await _mockTaskTypeSharedQueryRepository.Received(1).GetTaskTypesAsync(query, default);
        }
    }
}
