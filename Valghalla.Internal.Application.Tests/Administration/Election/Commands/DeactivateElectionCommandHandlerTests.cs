using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.Election.Commands;
using Valghalla.Internal.Application.Modules.Administration.Election.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.Election.Commands
{
    [TestClass]
    public class DeactivateElectionCommandHandlerTests
    {
        private readonly IElectionCommandRepository _mockCommandRepository;
        private readonly IElectionQueryRepository _mockQueryRepository;

        public DeactivateElectionCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IElectionCommandRepository>();
            _mockQueryRepository = Substitute.For<IElectionQueryRepository>();
        }

        [TestMethod]
        public async Task DeactivateElectionCommandHandler_Should_CallDeactivateElectionAsyncOnRepository()
        {
            var command = new DeactivateElectionCommand(Guid.NewGuid());
            var handler = new DeactivateElectionCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).DeactivateElectionAsync(command, default);
        }
    }
}
