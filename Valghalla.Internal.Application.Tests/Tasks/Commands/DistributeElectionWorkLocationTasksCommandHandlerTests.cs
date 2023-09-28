using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Tasks.Commands;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Requests;

namespace Valghalla.Internal.Application.Tests.Tasks.Commands
{
    [TestClass]
    public class DistributeElectionWorkLocationTasksCommandHandlerTests
    {
        private readonly IElectionWorkLocationTasksCommandRepository _mockCommandRepository;
        private readonly IElectionWorkLocationTasksQueryRepository _mockQueryRepository;

        public DistributeElectionWorkLocationTasksCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IElectionWorkLocationTasksCommandRepository>();
            _mockQueryRepository = Substitute.For<IElectionWorkLocationTasksQueryRepository>();
        }

        [TestMethod]
        public async Task DistributeElectionWorkLocationTasksCommandHandlerTests_Should_CallDistributeElectionWorkLocationTasksAsyncOnRepository()
        {
            var request = new ElectionWorkLocationTasksDistributingRequest()
            {
                ElectionId = Guid.NewGuid(),
                WorkLocationId = Guid.NewGuid(),
            };
            var command = new DistributeElectionWorkLocationTasksCommand()
            {
                ElectionId = request.ElectionId,
                WorkLocationId = request.WorkLocationId
            };
            var handler = new DistributeElectionWorkLocationTasksCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).DistributeElectionWorkLocationTasksAsync(command, default);
        }

        [TestMethod]
        public void DistributeElectionWorkLocationTasksCommandHandlerTests_Should_ReturnValidationErrorOnEmptyElectionId()
        {
            var request = new ElectionWorkLocationTasksDistributingRequest()
            {
                WorkLocationId = Guid.NewGuid(),
            };
            var command = new DistributeElectionWorkLocationTasksCommand()
            {
                WorkLocationId = request.WorkLocationId
            };
            var validator = new DistributeElectionWorkLocationTasksCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ElectionId);
        }

        [TestMethod]
        public void DistributeElectionWorkLocationTasksCommandHandlerTests_Should_ReturnValidationErrorOnEmptyWorkLocationId()
        {
            var request = new ElectionWorkLocationTasksDistributingRequest()
            {
                ElectionId = Guid.NewGuid(),
            };
            var command = new DistributeElectionWorkLocationTasksCommand()
            {
                ElectionId = request.WorkLocationId
            };
            var validator = new DistributeElectionWorkLocationTasksCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.WorkLocationId);
        }

        [TestMethod]
        public void DistributeElectionWorkLocationTasksCommandHandlerTests_Should_ReturnValidationErrorOnInvalidWorkLocation()
        {
            var request = new ElectionWorkLocationTasksDistributingRequest()
            {
                ElectionId = Guid.NewGuid(),
                WorkLocationId = Guid.NewGuid(),
            };
            var command = new DistributeElectionWorkLocationTasksCommand()
            {
                ElectionId = request.ElectionId,
                WorkLocationId = request.WorkLocationId
            };
            var validator = new DistributeElectionWorkLocationTasksCommandValidator(_mockQueryRepository);

            _mockQueryRepository
                .CheckIfWorkLocationInElectionAsync(request.WorkLocationId, request.ElectionId, default)
                .Returns(Task.FromResult(false));

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("tasks.error.invalid_work_location");
        }
    }
}
