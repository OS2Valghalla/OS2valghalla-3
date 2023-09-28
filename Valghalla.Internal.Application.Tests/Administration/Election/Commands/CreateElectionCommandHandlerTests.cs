using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.API.Requests.Administration.Election;
using Valghalla.Internal.Application.Modules.Administration.Election.Commands;
using Valghalla.Internal.Application.Modules.Administration.Election.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.Election.Commands
{
    [TestClass]
    public class CreateElectionCommandHandlerTests
    {
        private readonly IElectionCommandRepository _mockCommandRepository;
        private readonly IElectionQueryRepository _mockQueryRepository;

        public CreateElectionCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IElectionCommandRepository>();
            _mockQueryRepository = Substitute.For<IElectionQueryRepository>();
        }

        [TestMethod]
        public async Task CreateElectionCommandHandler_Should_CallCreateElectionAsyncOnRepository()
        {
            var request = new CreateElectionRequest()
            {
                Title = "Test Election",
                ElectionTypeId = Guid.NewGuid(),
                LockPeriod = 7,
                ElectionStartDate = DateTime.UtcNow.AddDays(-7),
                ElectionEndDate = DateTime.UtcNow.AddDays(7),
                ElectionDate = DateTime.UtcNow,
                WorkLocationIds = new[] { Guid.NewGuid() }
            };
            var command = new CreateElectionCommand()
            {
                Title = request.Title,
                ElectionTypeId = request.ElectionTypeId,
                LockPeriod = request.LockPeriod,
                ElectionStartDate = request.ElectionStartDate,
                ElectionEndDate = request.ElectionEndDate,
                ElectionDate = request.ElectionDate,
                WorkLocationIds = request.WorkLocationIds
            };
            var handler = new CreateElectionCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).CreateElectionAsync(command, default);
        }

        [TestMethod]
        public void CreateElectionCommandHandler_Should_ReturnValidationErrorOnEmptyTitle()
        {
            var command = new CreateElectionCommand()
            {
                ElectionTypeId = Guid.NewGuid(),
                LockPeriod = 7,
                ElectionStartDate = DateTime.UtcNow.AddDays(-7),
                ElectionEndDate = DateTime.UtcNow.AddDays(7),
                ElectionDate = DateTime.UtcNow,
                WorkLocationIds = new[] { Guid.NewGuid() }
            };
            var validator = new CreateElectionCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Title);
        }

        [TestMethod]
        public void CreateElectionCommandHandler_Should_ReturnValidationErrorOnEmptyElectionTypeId()
        {
            var command = new CreateElectionCommand()
            {
                Title = "Test Election",
                LockPeriod = 7,
                ElectionStartDate = DateTime.UtcNow.AddDays(-7),
                ElectionEndDate = DateTime.UtcNow.AddDays(7),
                ElectionDate = DateTime.UtcNow,
                WorkLocationIds = new[] { Guid.NewGuid() }
            };
            var validator = new CreateElectionCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ElectionTypeId);
        }

        [TestMethod]
        public void CreateElectionCommandHandler_Should_ReturnValidationErrorOnEmptyLockPeriod()
        {
            var command = new CreateElectionCommand()
            {
                Title = "Test Election",
                ElectionTypeId = Guid.NewGuid(),
                ElectionStartDate = DateTime.UtcNow.AddDays(-7),
                ElectionEndDate = DateTime.UtcNow.AddDays(7),
                ElectionDate = DateTime.UtcNow,
                WorkLocationIds = new[] { Guid.NewGuid() }
            };
            var validator = new CreateElectionCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.LockPeriod);
        }

        [TestMethod]
        public void CreateElectionCommandHandler_Should_ReturnValidationErrorOnEmptyElectionStartDate()
        {
            var command = new CreateElectionCommand()
            {
                Title = "Test Election",
                ElectionTypeId = Guid.NewGuid(),
                LockPeriod = 7,
                ElectionEndDate = DateTime.UtcNow.AddDays(7),
                ElectionDate = DateTime.UtcNow,
                WorkLocationIds = new[] { Guid.NewGuid() }
            };
            var validator = new CreateElectionCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ElectionStartDate);
        }

        [TestMethod]
        public void CreateElectionCommandHandler_Should_ReturnValidationErrorOnEmptyElectionEndDate()
        {
            var command = new CreateElectionCommand()
            {
                Title = "Test Election",
                ElectionTypeId = Guid.NewGuid(),
                LockPeriod = 7,
                ElectionStartDate = DateTime.UtcNow.AddDays(-7),
                ElectionDate = DateTime.UtcNow,
                WorkLocationIds = new[] { Guid.NewGuid() }
            };
            var validator = new CreateElectionCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ElectionEndDate);
        }

        [TestMethod]
        public void CreateElectionCommandHandler_Should_ReturnValidationErrorOnEmptyElectionDate()
        {
            var command = new CreateElectionCommand()
            {
                Title = "Test Election",
                ElectionTypeId = Guid.NewGuid(),
                LockPeriod = 7,
                ElectionStartDate = DateTime.UtcNow.AddDays(-7),
                ElectionEndDate = DateTime.UtcNow.AddDays(7),
                WorkLocationIds = new[] { Guid.NewGuid() }
            };
            var validator = new CreateElectionCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ElectionDate);
        }

        [TestMethod]
        public void CreateElectionCommandHandler_Should_ReturnValidationErrorOnEmptyWorkLocationIds()
        {
            var command = new CreateElectionCommand()
            {
                Title = "Test Election",
                ElectionTypeId = Guid.NewGuid(),
                LockPeriod = 7,
                ElectionStartDate = DateTime.UtcNow.AddDays(-7),
                ElectionEndDate = DateTime.UtcNow.AddDays(7),
                ElectionDate = DateTime.UtcNow,
            };
            var validator = new CreateElectionCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.WorkLocationIds);
        }

        [TestMethod]
        public void CreateElectionCommandHandler_Should_ReturnValidationErrorOnDuplicatedElection()
        {
            var command = new CreateElectionCommand()
            {
                Title = "Test Election",
                ElectionTypeId = Guid.NewGuid(),
                LockPeriod = 7,
                ElectionStartDate = DateTime.UtcNow.AddDays(-7),
                ElectionEndDate = DateTime.UtcNow.AddDays(7),
                ElectionDate = DateTime.UtcNow,
                WorkLocationIds = new[] { Guid.NewGuid() }
            };
            var validator = new CreateElectionCommandValidator(_mockQueryRepository);

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
