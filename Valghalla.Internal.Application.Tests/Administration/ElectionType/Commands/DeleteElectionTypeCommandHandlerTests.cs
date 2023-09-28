using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Commands;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.ElectionType.Commands
{
    [TestClass]
    public class DeleteElectionTypeCommandHandlerTests
    {
        private readonly IElectionTypeCommandRepository _mockCommandRepository;
        private readonly IElectionTypeQueryRepository _mockQueryRepository;

        public DeleteElectionTypeCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IElectionTypeCommandRepository>();
            _mockQueryRepository = Substitute.For<IElectionTypeQueryRepository>();
        }

        [TestMethod]
        public async Task DeleteElectionTypeCommandHandler_Should_CallDeleteElectionTypeAsyncOnRepository()
        {
            Guid id = Guid.NewGuid();
            var command = new DeleteElectionTypeCommand(id);
            var handler = new DeleteElectionTypeCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).DeleteElectionTypeAsync(command, default);
        }

        [TestMethod]
        public void DeleteElectionTypeCommandHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new DeleteElectionTypeCommand(Guid.Empty);
            var validator = new DeleteElectionTypeCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }

        [TestMethod]
        public void DeleteElectionTypeCommandHandler_Should_ReturnValidationErrorOnHavingElectionConnection()
        {
            Guid id = Guid.NewGuid();

            var command = new DeleteElectionTypeCommand(id);

            _mockQueryRepository
                .CheckIfElectionTypeCanBeDeletedAsync(id, default)
                .Returns(Task.FromResult(false));

            var validator = new DeleteElectionTypeCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("administration.election_type.error.existing_election_connection_when_delete");
        }
    }
}
