using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Application.Communication;
using Valghalla.Application.TaskValidation;
using Valghalla.Internal.Application.Modules.Tasks.Commands;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Requests;

namespace Valghalla.Internal.Application.Tests.Tasks.Commands
{
    [TestClass]
    public class AssignParticipantToTaskCommandHandlerTests
    {
        private readonly IElectionWorkLocationTasksCommandRepository _mockCommandRepository;
        private readonly ITaskValidationService _mockTaskValidationService;
        private readonly IElectionWorkLocationTasksQueryRepository _mockElectionWorkLocationTasksQueryRepository;
        private readonly ICommunicationService _mockCommunicationService;

        public AssignParticipantToTaskCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IElectionWorkLocationTasksCommandRepository>();
            _mockTaskValidationService = Substitute.For<ITaskValidationService>();
            _mockElectionWorkLocationTasksQueryRepository = Substitute.For<IElectionWorkLocationTasksQueryRepository>();
            _mockCommunicationService = Substitute.For<ICommunicationService>();
        }

        [TestMethod]
        public async Task AssignParticipantToTaskCommandHandlerTests_Should_CallAssignParticipantToTaskAsyncOnRepository()
        {
            var request = new AssignParticipantToTaskRequest()
            {
                TaskAssignmentId = Guid.NewGuid(),
                ElectionId = Guid.NewGuid(),
                ParticipantId = Guid.NewGuid(),
                TaskTypeId = Guid.NewGuid()
            };
            var command = new AssignParticipantToTaskCommand()
            {
                TaskAssignmentId = request.TaskAssignmentId,
                ElectionId = request.ElectionId,
                ParticipantId = request.ParticipantId,
                TaskTypeId = request.TaskTypeId
            };
            var handler = new AssignParticipantToTaskCommandHandler(_mockCommunicationService, _mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).AssignParticipantToTaskAsync(command, default);
        }

        [TestMethod]
        public void AssignParticipantToTaskCommandHandlerTests_Should_ReturnValidationErrorOnEmptyElectionId()
        {
            var request = new AssignParticipantToTaskRequest()
            {
                TaskAssignmentId = Guid.NewGuid(),
                ParticipantId = Guid.NewGuid(),
                TaskTypeId = Guid.NewGuid()
            };
            var command = new AssignParticipantToTaskCommand()
            {
                TaskAssignmentId = request.TaskAssignmentId,
                ParticipantId = request.ParticipantId,
                TaskTypeId = request.TaskTypeId
            };
            var validator = new AssignParticipantToTaskCommandValidator(_mockTaskValidationService, _mockElectionWorkLocationTasksQueryRepository);

            _mockTaskValidationService
                .ExecuteAsync(request.TaskTypeId, request.ElectionId, request.ParticipantId, default)
                .Returns(Task.FromResult(new TaskValidationResult(new List<TaskValidationRule> { })));

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ElectionId);
        }

        [TestMethod]
        public void AssignParticipantToTaskCommandHandlerTests_Should_ReturnValidationErrorOnEmptyTaskAssignmentId()
        {
            var request = new AssignParticipantToTaskRequest()
            {
                ElectionId = Guid.NewGuid(),
                ParticipantId = Guid.NewGuid(),
                TaskTypeId = Guid.NewGuid()
            };
            var command = new AssignParticipantToTaskCommand()
            {
                ElectionId = request.ElectionId,
                ParticipantId = request.ParticipantId,
                TaskTypeId = request.TaskTypeId
            };
            var validator = new AssignParticipantToTaskCommandValidator(_mockTaskValidationService, _mockElectionWorkLocationTasksQueryRepository);

            _mockTaskValidationService
                .ExecuteAsync(request.TaskTypeId, request.ElectionId, request.ParticipantId, default)
                .Returns(Task.FromResult(new TaskValidationResult(new List<TaskValidationRule> { })));

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.TaskAssignmentId);
        }

        [TestMethod]
        public void AssignParticipantToTaskCommandHandlerTests_Should_ReturnValidationErrorOnEmptyParticipantId()
        {
            var request = new AssignParticipantToTaskRequest()
            {
                TaskAssignmentId = Guid.NewGuid(),
                ElectionId = Guid.NewGuid(),
                TaskTypeId = Guid.NewGuid()
            };
            var command = new AssignParticipantToTaskCommand()
            {
                TaskAssignmentId = request.TaskAssignmentId,
                ElectionId = request.ElectionId,
                TaskTypeId = request.TaskTypeId
            };
            var validator = new AssignParticipantToTaskCommandValidator(_mockTaskValidationService, _mockElectionWorkLocationTasksQueryRepository);

            _mockTaskValidationService
                .ExecuteAsync(request.TaskTypeId, request.ElectionId, request.ParticipantId, default)
                .Returns(Task.FromResult(new TaskValidationResult(new List<TaskValidationRule> { })));

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ParticipantId);
        }

        [TestMethod]
        public void AssignParticipantToTaskCommandHandlerTests_Should_ReturnValidationErrorOnEmptyTaskTypeId()
        {
            var request = new AssignParticipantToTaskRequest()
            {
                TaskAssignmentId = Guid.NewGuid(),
                ElectionId = Guid.NewGuid(),
                ParticipantId = Guid.NewGuid()
            };
            var command = new AssignParticipantToTaskCommand()
            {
                TaskAssignmentId = request.TaskAssignmentId,
                ElectionId = request.ElectionId,
                ParticipantId = request.ParticipantId
            };
            var validator = new AssignParticipantToTaskCommandValidator(_mockTaskValidationService, _mockElectionWorkLocationTasksQueryRepository);

            _mockTaskValidationService
                .ExecuteAsync(request.TaskTypeId, request.ElectionId, request.ParticipantId, default)
                .Returns(Task.FromResult(new TaskValidationResult(new List<TaskValidationRule> { })));

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.TaskTypeId);
        }

        [TestMethod]
        public void AssignParticipantToTaskCommandHandlerTests_Should_ReturnValidationErrorOnRuleIsAliveFailed()
        {
            var request = new AssignParticipantToTaskRequest()
            {
                TaskAssignmentId = Guid.NewGuid(),
                ElectionId = Guid.NewGuid(),
                ParticipantId = Guid.NewGuid(),
                TaskTypeId = Guid.NewGuid()
            };
            var command = new AssignParticipantToTaskCommand()
            {
                TaskAssignmentId = request.TaskAssignmentId,
                ElectionId = request.ElectionId,
                ParticipantId = request.ParticipantId,
                TaskTypeId = request.TaskTypeId
            };
            var validator = new AssignParticipantToTaskCommandValidator(_mockTaskValidationService, _mockElectionWorkLocationTasksQueryRepository);

            _mockTaskValidationService
                .ExecuteAsync(request.TaskTypeId, request.ElectionId, request.ParticipantId, default)
                .Returns(Task.FromResult(new TaskValidationResult(new List<TaskValidationRule> { new TaskValidationRule(TaskValidationRule.Alive.Id) })));

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("tasks.error.validation_rules_alive");
        }

        [TestMethod]
        public void AssignParticipantToTaskCommandHandlerTests_Should_ReturnValidationErrorOnRuleIsAge18OrOverFailed()
        {
            var request = new AssignParticipantToTaskRequest()
            {
                TaskAssignmentId = Guid.NewGuid(),
                ElectionId = Guid.NewGuid(),
                ParticipantId = Guid.NewGuid(),
                TaskTypeId = Guid.NewGuid()
            };
            var command = new AssignParticipantToTaskCommand()
            {
                TaskAssignmentId = request.TaskAssignmentId,
                ElectionId = request.ElectionId,
                ParticipantId = request.ParticipantId,
                TaskTypeId = request.TaskTypeId
            };
            var validator = new AssignParticipantToTaskCommandValidator(_mockTaskValidationService, _mockElectionWorkLocationTasksQueryRepository);

            _mockTaskValidationService
                .ExecuteAsync(request.TaskTypeId, request.ElectionId, request.ParticipantId, default)
                .Returns(Task.FromResult(new TaskValidationResult(new List<TaskValidationRule> { new TaskValidationRule(TaskValidationRule.Age18.Id) })));

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("tasks.error.validation_rules_age");
        }

        [TestMethod]
        public void AssignParticipantToTaskCommandHandlerTests_Should_ReturnValidationErrorOnRuleIsResidencyMunicipalityFailed()
        {
            var request = new AssignParticipantToTaskRequest()
            {
                TaskAssignmentId = Guid.NewGuid(),
                ElectionId = Guid.NewGuid(),
                ParticipantId = Guid.NewGuid(),
                TaskTypeId = Guid.NewGuid()
            };
            var command = new AssignParticipantToTaskCommand()
            {
                TaskAssignmentId = request.TaskAssignmentId,
                ElectionId = request.ElectionId,
                ParticipantId = request.ParticipantId,
                TaskTypeId = request.TaskTypeId
            };
            var validator = new AssignParticipantToTaskCommandValidator(_mockTaskValidationService, _mockElectionWorkLocationTasksQueryRepository);

            _mockTaskValidationService
                .ExecuteAsync(request.TaskTypeId, request.ElectionId, request.ParticipantId, default)
                .Returns(Task.FromResult(new TaskValidationResult(new List<TaskValidationRule> { new TaskValidationRule(TaskValidationRule.ResidencyMunicipality.Id) })));

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("tasks.error.validation_rules_municipality");
        }

        [TestMethod]
        public void AssignParticipantToTaskCommandHandlerTests_Should_ReturnValidationErrorOnRuleIsDisenfranchisedFailed()
        {
            var request = new AssignParticipantToTaskRequest()
            {
                TaskAssignmentId = Guid.NewGuid(),
                ElectionId = Guid.NewGuid(),
                ParticipantId = Guid.NewGuid(),
                TaskTypeId = Guid.NewGuid()
            };
            var command = new AssignParticipantToTaskCommand()
            {
                TaskAssignmentId = request.TaskAssignmentId,
                ElectionId = request.ElectionId,
                ParticipantId = request.ParticipantId,
                TaskTypeId = request.TaskTypeId
            };
            var validator = new AssignParticipantToTaskCommandValidator(_mockTaskValidationService, _mockElectionWorkLocationTasksQueryRepository);

            _mockTaskValidationService
                .ExecuteAsync(request.TaskTypeId, request.ElectionId, request.ParticipantId, default)
                .Returns(Task.FromResult(new TaskValidationResult(new List<TaskValidationRule> { new TaskValidationRule(TaskValidationRule.Disenfranchised.Id) })));

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("tasks.error.validation_rules_legal_adult");
        }

        [TestMethod]
        public void AssignParticipantToTaskCommandHandlerTests_Should_ReturnValidationErrorOnRuleHasDanishCitizenshipFailed()
        {
            var request = new AssignParticipantToTaskRequest()
            {
                TaskAssignmentId = Guid.NewGuid(),
                ElectionId = Guid.NewGuid(),
                ParticipantId = Guid.NewGuid(),
                TaskTypeId = Guid.NewGuid()
            };
            var command = new AssignParticipantToTaskCommand()
            {
                TaskAssignmentId = request.TaskAssignmentId,
                ElectionId = request.ElectionId,
                ParticipantId = request.ParticipantId,
                TaskTypeId = request.TaskTypeId
            };
            var validator = new AssignParticipantToTaskCommandValidator(_mockTaskValidationService, _mockElectionWorkLocationTasksQueryRepository);

            _mockTaskValidationService
                .ExecuteAsync(request.TaskTypeId, request.ElectionId, request.ParticipantId, default)
                .Returns(Task.FromResult(new TaskValidationResult(new List<TaskValidationRule> { new TaskValidationRule(TaskValidationRule.Citizenship.Id) })));

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("tasks.error.validation_rules_citizenship");
        }
    }
}
