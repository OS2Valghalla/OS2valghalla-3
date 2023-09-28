using NSubstitute;
using Valghalla.Internal.Application.Modules.Shared.WorkLocation.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.WorkLocation.Queries;

namespace Valghalla.Internal.Application.Tests.Shared.WorkLocation.Queries
{
    [TestClass]
    public class GetWorkLocationsSharedQueryHandlerTests
    {
        private readonly IWorkLocationSharedQueryRepository _mockWorkLocationSharedQueryRepository;

        public GetWorkLocationsSharedQueryHandlerTests()
        {
            _mockWorkLocationSharedQueryRepository = Substitute.For<IWorkLocationSharedQueryRepository>();
        }

        [TestMethod]
        public async Task GetWorkLocationsSharedQueryHandler_Should_CallGetWorkLocationsAsyncOnRepository()
        {
            var query = new GetWorkLocationsSharedQuery();
            var handler = new GetWorkLocationsSharedQueryHandler(_mockWorkLocationSharedQueryRepository);

            await handler.Handle(query, default);

            await _mockWorkLocationSharedQueryRepository.Received(1).GetWorkLocationsAsync(query, default);
        }
    }
}
