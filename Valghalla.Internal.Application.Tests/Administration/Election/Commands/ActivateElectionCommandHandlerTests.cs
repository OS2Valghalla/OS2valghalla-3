using NSubstitute;
using Valghalla.Application.Queue;
using Valghalla.Internal.Application.Modules.Administration.Election.Commands;
using Valghalla.Internal.Application.Modules.Administration.Election.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.Election.Commands
{
    [TestClass]
    public class ActivateElectionCommandHandlerTests
    {
        private readonly IElectionCommandRepository _mockCommandRepository;
        private readonly IElectionQueryRepository _mockQueryRepository;
        private readonly IQueueService _mockQueueService;

        public ActivateElectionCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IElectionCommandRepository>();
            _mockQueryRepository = Substitute.For<IElectionQueryRepository>();
            _mockQueueService = Substitute.For<IQueueService>();
        }

        [TestMethod]
        public async Task ActivateElectionCommandHandler_Should_CallActivateElectionAsyncOnRepository()
        {
            var command = new ActivateElectionCommand(Guid.NewGuid());
            var handler = new ActivateElectionCommandHandler(_mockCommandRepository, _mockQueueService);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).ActivateElectionAsync(command, default);
        }
    }
}
