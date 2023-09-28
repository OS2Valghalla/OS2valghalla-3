using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Participant.Commands;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;

namespace Valghalla.Internal.Application.Tests.Participant.Commands
{
    [TestClass]
    public class DeleteParticipantCommandHandlerTests
    {
        private readonly IParticipantGdprCommandRepository _mockParticipantGdprCommandRepository;

        public DeleteParticipantCommandHandlerTests()
        {
            _mockParticipantGdprCommandRepository = Substitute.For<IParticipantGdprCommandRepository>();
        }

        [TestMethod]
        public async Task DeleteParticipantCommandHandler_Should_CallDeleteParticipantAsyncOnRepository()
        {
            var command = new DeleteParticipantCommand(Guid.NewGuid());
            var handler = new DeleteParticipantCommandHandler(_mockParticipantGdprCommandRepository, new MockQueueService());

            await handler.Handle(command, default);

            await _mockParticipantGdprCommandRepository.Received(1).DeleteParticipantAsync(command, default);
        }

        [TestMethod]
        public void DeleteParticipantCommandHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new DeleteParticipantCommand(Guid.Empty);
            var validator = new DeleteParticipantCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }
    }
}
