using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.Election.Commands;
using Valghalla.Internal.Application.Modules.Administration.Election.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.Election.Commands
{
    [TestClass]
    public class DeleteElectionCommandHandlerTests
    {
        private readonly IElectionCommandRepository _mockCommandRepository;
        private readonly IElectionQueryRepository _mockQueryRepository;

        public DeleteElectionCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IElectionCommandRepository>();
            _mockQueryRepository = Substitute.For<IElectionQueryRepository>();
        }

        [TestMethod]
        public async Task DeleteElectionCommandHandler_Should_CallDeleteElectionAsyncOnRepository()
        {
            Guid id = Guid.NewGuid();
            var command = new DeleteElectionCommand(id);
            var handler = new DeleteElectionCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).DeleteElectionAsync(command, default);
        }

        [TestMethod]
        public void DeleteElectionCommandHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new DeleteElectionCommand(Guid.Empty);
            var validator = new DeleteElectionCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }
    }
}
