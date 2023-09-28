using NSubstitute;
using Valghalla.Internal.Application.Modules.Shared.WorkLocation.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.WorkLocation.Queries;

namespace Valghalla.Internal.Application.Tests.Shared.WorkLocation.Queries
{
    [TestClass]
    public class GetWorkLocationSharedQueryHandlerTests
    {
        private readonly IWorkLocationSharedQueryRepository _mockWorkLocationSharedQueryRepository;

        public GetWorkLocationSharedQueryHandlerTests()
        {
            _mockWorkLocationSharedQueryRepository = Substitute.For<IWorkLocationSharedQueryRepository>();
        }

        [TestMethod]
        public async Task GetWorkLocationSharedQueryHandler_Should_CallGetWorkLocationAsyncOnRepository()
        {
            var query = new GetWorkLocationSharedQuery(Guid.NewGuid(), Guid.Empty);
            var handler = new GetWorkLocationSharedQueryHandler(_mockWorkLocationSharedQueryRepository);

            await handler.Handle(query, default);

            await _mockWorkLocationSharedQueryRepository.Received(1).GetWorkLocationAsync(query, default);
        }
    }
}
