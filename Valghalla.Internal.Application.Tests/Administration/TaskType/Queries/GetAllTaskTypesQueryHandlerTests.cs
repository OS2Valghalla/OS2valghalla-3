using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Queries;

namespace Valghalla.Internal.Application.Tests.Administration.TaskType.Queries
{
    [TestClass]
    public class GetAllTaskTypesQueryHandlerTests
    {
        private readonly ITaskTypeQueryRepository _mockQueryRepository;

        public GetAllTaskTypesQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<ITaskTypeQueryRepository>();
        }

        [TestMethod]
        public async Task GetAllTaskTypesQueryHandler_Should_CallOnRepository()
        {
            var query = new GetAllTaskTypesQuery();
            var handler = new GetAllTaskTypesQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetAllTaskTypesAsync(default);
        }
    }
}
