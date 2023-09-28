using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.Area.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Area.Queries;

namespace Valghalla.Internal.Application.Tests.Administration.Area.Queries
{
    [TestClass]
    public class GetAreaDetailsQueryHandlerTests
    {
        private readonly IAreaQueryRepository _mockQueryRepository;

        public GetAreaDetailsQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<IAreaQueryRepository>();
        }

        [TestMethod]
        public async Task GetAreaDetailsQueryHandler_Should_CallOnRepository()
        {
            var areaId = Guid.NewGuid();
            var query = new GetAreaDetailsQuery(areaId);
            var handler = new GetAreaDetailsQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetAreaAsync(query, default);
        }
    }
}
