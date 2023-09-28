using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.User.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.User.Queries;

namespace Valghalla.Internal.Application.Tests.Administration.User.Queries
{
    [TestClass]
    public class GetUsersQueryHandlerTests
    {
        private readonly IUserQueryRepository _mockQueryRepository;

        public GetUsersQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<IUserQueryRepository>();
        }

        [TestMethod]
        public async Task GetUsersQueryHandler_Should_CallOnRepository()
        {
            var query = new GetUsersQuery();
            var handler = new GetUsersQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetUsersAsync(default);
        }
    }
}
