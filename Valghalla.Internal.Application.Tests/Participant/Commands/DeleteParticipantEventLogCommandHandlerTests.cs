using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Participant.Commands;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;

namespace Valghalla.Internal.Application.Tests.Participant.Commands
{
    [TestClass]
    public class DeleteParticipantEventLogCommandHandlerTests
    {
        private readonly IParticipantEventLogCommandRepository _mockParticipantEventLogCommandRepository;

        public DeleteParticipantEventLogCommandHandlerTests()
        {
            _mockParticipantEventLogCommandRepository = Substitute.For<IParticipantEventLogCommandRepository>();
        }

        [TestMethod]
        public async Task DeleteParticipantEventLogCommandHandler_Should_CallDeleteParticipantEventLogAsyncOnRepository()
        {
            var command = new DeleteParticipantEventLogCommand(Guid.NewGuid());
            var handler = new DeleteParticipantEventLogCommandHandler(_mockParticipantEventLogCommandRepository);

            await handler.Handle(command, default);

            await _mockParticipantEventLogCommandRepository.Received(1).DeleteParticipantEventLogAsync(command, default);
        }

        [TestMethod]
        public void DeleteParticipantEventLogCommandHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new DeleteParticipantEventLogCommand(Guid.Empty);
            var validator = new DeleteParticipantEventLogCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }
    }
}
