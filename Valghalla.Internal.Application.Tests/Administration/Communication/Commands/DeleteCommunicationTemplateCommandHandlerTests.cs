using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.Communication.Commands;
using Valghalla.Internal.Application.Modules.Administration.Communication.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.Communication.Commands
{
    [TestClass]
    public class DeleteCommunicationTemplateCommandHandlerTests
    {

        private readonly ICommunicationCommandRepository _mockCommandRepository;
        private readonly ICommunicationQueryRepository _mockQueryRepository;

        public DeleteCommunicationTemplateCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<ICommunicationCommandRepository>();
            _mockQueryRepository = Substitute.For<ICommunicationQueryRepository>();
        }

        [TestMethod]
        public async Task DeleteCommunicationTemplateCommandHandler_Should_CallDeleteCommunicationTemplateAsyncOnRepository()
        {
            Guid id = Guid.NewGuid();
            var command = new DeleteCommunicationTemplateCommand(id);
            var handler = new DeleteCommunicationTemplateCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).DeleteCommunicationTemplateAsync(command, default);
        }

        [TestMethod]
        public void DeleteCommunicationTemplateCommandHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new DeleteCommunicationTemplateCommand(Guid.Empty);
            var validator = new DeleteCommunicationTemplateCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }

        [TestMethod]
        public void DeleteCommunicationTemplateCommandHandler_Should_ReturnValidationErrorOnDuplicatedSpecialDiet()
        {
            var command = new DeleteCommunicationTemplateCommand(Guid.NewGuid());
            var validator = new DeleteCommunicationTemplateCommandValidator(_mockQueryRepository);

            _mockQueryRepository
                .CheckIsDefaultCommunicationTemplateAsync(command.Id, default)
                .Returns(Task.FromResult(true));

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("communication.error.cannot_delete_default_communication_template");
        }
    }
}
