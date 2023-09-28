using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.API.Requests.Administration.Election;
using Valghalla.Internal.Application.Modules.Administration.Election.Commands;
using Valghalla.Internal.Application.Modules.Administration.Election.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.Election.Commands
{
    [TestClass]
    public class UpdateElectionCommandHandlerTests
    {
        private readonly IElectionCommandRepository _mockCommandRepository;
        private readonly IElectionQueryRepository _mockQueryRepository;

        public UpdateElectionCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IElectionCommandRepository>();
            _mockQueryRepository = Substitute.For<IElectionQueryRepository>();
        }

        [TestMethod]
        public async Task UpdateElectionCommandHandler_Should_CallUpdateElectionAsyncOnRepository()
        {
            var request = new UpdateElectionRequest()
            {
                Id = Guid.NewGuid(),
                Title = "Test Election",
                LockPeriod = 7
            };
            var command = new UpdateElectionCommand()
            {
                Title = request.Title,
                LockPeriod = request.LockPeriod
            };
            var handler = new UpdateElectionCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).UpdateElectionAsync(command, default);
        }

        [TestMethod]
        public void UpdateElectionCommandHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new UpdateElectionCommand()
            {
                Title = "Test Election",
                LockPeriod = 7
            };
            var validator = new UpdateElectionCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }

        [TestMethod]
        public void UpdateElectionCommandHandler_Should_ReturnValidationErrorOnEmptyTitle()
        {
            var command = new UpdateElectionCommand()
            {
                Id = Guid.NewGuid(),
                LockPeriod = 7
            };
            var validator = new UpdateElectionCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Title);
        }

        [TestMethod]
        public void UpdateElectionCommandHandler_Should_ReturnValidationErrorOnEmptyLockPeriod()
        {
            var command = new UpdateElectionCommand()
            {
                Id = Guid.NewGuid(),
                Title = "Test Election"
            };
            var validator = new UpdateElectionCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.LockPeriod);
        }

        [TestMethod]
        public void UpdateElectionCommandHandler_Should_ReturnValidationErrorOnDuplicatedElection()
        {
            var command = new UpdateElectionCommand()
            {
                Id = Guid.NewGuid(),
                Title = "Test Election",
                LockPeriod = 7
            };
            var validator = new UpdateElectionCommandValidator(_mockQueryRepository);

            _mockQueryRepository
                .CheckIfElectionExistsAsync(command, default)
                .Returns(Task.FromResult(true));

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("administration.election.error.election_exist");
        }
    }
}
