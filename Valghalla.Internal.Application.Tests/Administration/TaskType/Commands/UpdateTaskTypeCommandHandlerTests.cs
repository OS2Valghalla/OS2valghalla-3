using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.API.Requests.Administration.TaskType;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Commands;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.TaskType.Commands
{
    [TestClass]
    public class UpdateTaskTypeCommandHandlerTests
    {
        private readonly ITaskTypeCommandRepository _mockCommandRepository;
        private readonly ITaskTypeQueryRepository _mockQueryRepository;

        public UpdateTaskTypeCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<ITaskTypeCommandRepository>();
            _mockQueryRepository = Substitute.For<ITaskTypeQueryRepository>();
        }

        [TestMethod]
        public async Task UpdateTaskTypeCommandHandlerTests_Should_CallUpdateSpecialDietAsyncOnRepository()
        {
            var request = new UpdateTaskTypeRequest()
            {
                Id = Guid.NewGuid(),
                Title = "Testing Task Type",
                ShortName = "Testing Short Name",
                Description = "Testing Description",
                StartTime = TimeSpan.FromHours(15),
                EndTime = TimeSpan.FromHours(16),
                ValidationNotRequired = false,
                Trusted = false,
                SendingReminderEnabled = false,
            };
            var command = new UpdateTaskTypeCommand()
            {
                Id = request.Id,
                Title = request.Title,
                ShortName = request.ShortName,
                Description = request.Description,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                ValidationNotRequired = request.ValidationNotRequired,
                Trusted = request.Trusted,
                SendingReminderEnabled = request.SendingReminderEnabled,
            };
            var handler = new UpdateTaskTypeCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).UpdateTaskTypeAsync(command, default);
        }

        [TestMethod]
        public void UpdateTaskTypeCommandHandlerTests_Should_ReturnValidationErrorOnEmptyId()
        {
            var request = new UpdateTaskTypeRequest()
            {
                Title = "Testing Task Type",
                ShortName = "Testing Short Name",
                Description = "Testing Description",
                StartTime = TimeSpan.FromHours(15),
                EndTime = TimeSpan.FromHours(16),
                ValidationNotRequired = false,
                Trusted = false,
                SendingReminderEnabled = false,
            };
            var command = new UpdateTaskTypeCommand()
            {
                Title = request.Title,
                ShortName = request.ShortName,
                Description = request.Description,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                ValidationNotRequired = request.ValidationNotRequired,
                Trusted = request.Trusted,
                SendingReminderEnabled = request.SendingReminderEnabled,
            };
            var validator = new UpdateTaskTypeCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }

        [TestMethod]
        public void UpdateTaskTypeCommandHandlerTests_Should_ReturnValidationErrorOnEmptyTitle()
        {
            var request = new UpdateTaskTypeRequest()
            {
                Id = Guid.NewGuid(),
                ShortName = "Testing Short Name",
                Description = "Testing Description",
                StartTime = TimeSpan.FromHours(15),
                EndTime = TimeSpan.FromHours(16),
                ValidationNotRequired = false,
                Trusted = false,
                SendingReminderEnabled = false,
            };
            var command = new UpdateTaskTypeCommand()
            {
                Id = request.Id,
                ShortName = request.ShortName,
                Description = request.Description,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                ValidationNotRequired = request.ValidationNotRequired,
                Trusted = request.Trusted,
                SendingReminderEnabled = request.SendingReminderEnabled,
            };
            var validator = new UpdateTaskTypeCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Title);
        }

        [TestMethod]
        public void UpdateTaskTypeCommandHandlerTests_Should_ReturnValidationErrorOnEmptyShortName()
        {
            var request = new UpdateTaskTypeRequest()
            {
                Id = Guid.NewGuid(),
                Title = "Testing Task Type",
                Description = "Testing Description",
                StartTime = TimeSpan.FromHours(15),
                EndTime = TimeSpan.FromHours(16),
                ValidationNotRequired = false,
                Trusted = false,
                SendingReminderEnabled = false,
            };
            var command = new UpdateTaskTypeCommand()
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                ValidationNotRequired = request.ValidationNotRequired,
                Trusted = request.Trusted,
                SendingReminderEnabled = request.SendingReminderEnabled,
            };
            var validator = new UpdateTaskTypeCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ShortName);
        }

        [TestMethod]
        public void UpdateTaskTypeCommandHandlerTests_Should_ReturnValidationErrorOnEmptyDescription()
        {
            var request = new UpdateTaskTypeRequest()
            {
                Id = Guid.NewGuid(),
                Title = "Testing Task Type",
                ShortName = "Testing Short Name",
                StartTime = TimeSpan.FromHours(15),
                EndTime = TimeSpan.FromHours(16),
                ValidationNotRequired = false,
                Trusted = false,
                SendingReminderEnabled = false,
            };
            var command = new UpdateTaskTypeCommand()
            {
                Id = request.Id,
                Title = request.Title,
                ShortName = request.ShortName,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                ValidationNotRequired = request.ValidationNotRequired,
                Trusted = request.Trusted,
                SendingReminderEnabled = request.SendingReminderEnabled,
            };
            var validator = new UpdateTaskTypeCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Description);
        }

        [TestMethod]
        public void UpdateTaskTypeCommandHandlerTests_Should_ReturnValidationErrorOnEmptyStartTime()
        {
            var request = new UpdateTaskTypeRequest()
            {
                Id = Guid.NewGuid(),
                Title = "Testing Task Type",
                ShortName = "Testing Short Name",
                Description = "Testing Description",
                EndTime = TimeSpan.FromHours(16),
                ValidationNotRequired = false,
                Trusted = false,
                SendingReminderEnabled = false,
            };
            var command = new UpdateTaskTypeCommand()
            {
                Id = request.Id,
                Title = request.Title,
                ShortName = request.ShortName,
                Description = request.Description,
                EndTime = request.EndTime,
                ValidationNotRequired = request.ValidationNotRequired,
                Trusted = request.Trusted,
                SendingReminderEnabled = request.SendingReminderEnabled,
            };
            var validator = new UpdateTaskTypeCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.StartTime);
        }

        [TestMethod]
        public void UpdateTaskTypeCommandHandlerTests_Should_ReturnValidationErrorOnEmptyEndTime()
        {
            var request = new UpdateTaskTypeRequest()
            {
                Id = Guid.NewGuid(),
                Title = "Testing Task Type",
                ShortName = "Testing Short Name",
                Description = "Testing Description",
                StartTime = TimeSpan.FromHours(15),
                ValidationNotRequired = false,
                Trusted = false,
                SendingReminderEnabled = false,
            };
            var command = new UpdateTaskTypeCommand()
            {
                Id = request.Id,
                Title = request.Title,
                ShortName = request.ShortName,
                Description = request.Description,
                StartTime = request.StartTime,
                ValidationNotRequired = request.ValidationNotRequired,
                Trusted = request.Trusted,
                SendingReminderEnabled = request.SendingReminderEnabled,
            };
            var validator = new UpdateTaskTypeCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.EndTime);
        }

        [TestMethod]
        public void UpdateTaskTypeCommandHandlerTests_Should_ReturnValidationErrorOnDuplicatedTaskType()
        {
            var request = new UpdateTaskTypeRequest()
            {
                Id = Guid.NewGuid(),
                Title = "Testing Task Type",
                ShortName = "Testing Short Name",
                Description = "Testing Description",
                StartTime = TimeSpan.FromHours(15),
                EndTime = TimeSpan.FromHours(16),
                ValidationNotRequired = false,
                Trusted = false,
                SendingReminderEnabled = false,
            };
            var command = new UpdateTaskTypeCommand()
            {
                Id = request.Id,
                Title = request.Title,
                ShortName = request.ShortName,
                Description = request.Description,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                ValidationNotRequired = request.ValidationNotRequired,
                Trusted = request.Trusted,
                SendingReminderEnabled = request.SendingReminderEnabled,
            };
            var validator = new UpdateTaskTypeCommandValidator(_mockQueryRepository);

            _mockQueryRepository
                .CheckIfTaskTypeExistsAsync(command, default)
                .Returns(Task.FromResult(true));

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("administration.task_type.error.task_type_exist");
        }
    }
}
