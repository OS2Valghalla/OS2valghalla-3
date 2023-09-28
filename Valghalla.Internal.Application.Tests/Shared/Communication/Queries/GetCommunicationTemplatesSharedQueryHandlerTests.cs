using NSubstitute;
using Valghalla.Internal.Application.Modules.Shared.Communication.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Communication.Queries;

namespace Valghalla.Internal.Application.Tests.Shared.Communication.Queries
{
    [TestClass]
    public class GetCommunicationTemplatesSharedQueryHandlerTests
    {
        private readonly ICommunicationSharedQueryRepository _mockCommunicationSharedQueryRepository;

        public GetCommunicationTemplatesSharedQueryHandlerTests()
        {
            _mockCommunicationSharedQueryRepository = Substitute.For<ICommunicationSharedQueryRepository>();
        }

        [TestMethod]
        public async Task GetCommunicationTemplatesSharedQueryHandler_Should_CallGetCommunicationTemplatesAsyncOnRepository()
        {
            var query = new GetCommunicationTemplatesSharedQuery();
            var handler = new GetCommunicationTemplatesSharedQueryHandler(_mockCommunicationSharedQueryRepository);

            await handler.Handle(query, default);

            await _mockCommunicationSharedQueryRepository.Received(1).GetCommunicationTemplatesAsync(query, default);
        }
    }
}
