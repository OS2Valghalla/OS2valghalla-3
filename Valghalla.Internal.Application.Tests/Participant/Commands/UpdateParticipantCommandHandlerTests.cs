using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Participant.Commands;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;

namespace Valghalla.Internal.Application.Tests.Participant.Commands
{
    [TestClass]
    public class UpdateParticipantCommandHandlerTests
    {
        private readonly IParticipantCommandRepository _mockParticipantCommandRepository;
        private readonly IParticipantQueryRepository _mockParticipantQueryRepository;

        public UpdateParticipantCommandHandlerTests()
        {
            _mockParticipantCommandRepository = Substitute.For<IParticipantCommandRepository>();
            _mockParticipantQueryRepository = Substitute.For<IParticipantQueryRepository>();
        }

        [TestMethod]
        public async Task UpdateParticipantCommandHandler_Should_CallUpdateParticipantAsyncOnRepository()
        {
            var command = new UpdateParticipantCommand()
            {
                Id = Guid.NewGuid(),
                MobileNumber = "+457778888",
                Email = "test@net.xyz",
                SpecialDietIds = Array.Empty<Guid>(),
                TeamIds = new[] { Guid.NewGuid() }
            };

            var handler = new UpdateParticipantCommandHandler(_mockParticipantCommandRepository);

            await handler.Handle(command, default);

            await _mockParticipantCommandRepository.Received(1).UpdateParticipantAsync(command, default);
        }

        [TestMethod]
        public void UpdateParticipantCommandHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new UpdateParticipantCommand()
            {
                Id = Guid.Empty,
                MobileNumber = "+457778888",
                Email = "test@net.xyz",
                SpecialDietIds = Array.Empty<Guid>(),
                TeamIds = new[] { Guid.NewGuid() }
            };

            var validator = new UpdateParticipantCommandValidator(_mockParticipantQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }

        [TestMethod]
        public void UpdateParticipantCommandHandler_Should_ReturnValidationErrorOnInvalidEmail()
        {
            var command = new UpdateParticipantCommand()
            {
                Id = Guid.NewGuid(),
                MobileNumber = "+457778888",
                Email = "abc123",
                SpecialDietIds = Array.Empty<Guid>(),
                TeamIds = new[] { Guid.NewGuid() }
            };

            var validator = new UpdateParticipantCommandValidator(_mockParticipantQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Email);
        }

        [TestMethod]
        public void UpdateParticipantCommandHandler_Should_ReturnValidationErrorOnEmptyTeamIds()
        {
            var command = new UpdateParticipantCommand()
            {
                Id = Guid.NewGuid(),
                MobileNumber = "+457778888",
                Email = "test@net.xyz",
                SpecialDietIds = Array.Empty<Guid>(),
                TeamIds = Array.Empty<Guid>()
            };

            var validator = new UpdateParticipantCommandValidator(_mockParticipantQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.TeamIds);
        }

        [TestMethod]
        public void UpdateParticipantCommandHandler_Should_ReturnValidationErrorOnParticipantHasAssociatedTasks()
        {
            var command = new UpdateParticipantCommand()
            {
                Id = Guid.NewGuid(),
                MobileNumber = "+457778888",
                Email = "test@net.xyz",
                SpecialDietIds = Array.Empty<Guid>(),
                TeamIds = new[] { Guid.NewGuid() }
            };

            _mockParticipantQueryRepository
                .CheckIfParticipantHasAssociatedTasksAsync(command, default)
                .Returns(Task.FromResult(true));

            var validator = new UpdateParticipantCommandValidator(_mockParticipantQueryRepository);

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("participant.error.participant_has_associated_tasks");
        }
    }
}
