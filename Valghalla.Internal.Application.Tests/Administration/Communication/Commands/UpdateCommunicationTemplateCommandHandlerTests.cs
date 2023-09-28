using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.API.Requests.Administration.Communication;
using Valghalla.Internal.Application.Modules.Administration.Communication.Commands;
using Valghalla.Internal.Application.Modules.Administration.Communication.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.Communication.Commands
{
    [TestClass]
    public class UpdateCommunicationTemplateCommandHandlerTests
    {
        private readonly ICommunicationCommandRepository _mockCommandRepository;
        private readonly ICommunicationQueryRepository _mockQueryRepository;

        public UpdateCommunicationTemplateCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<ICommunicationCommandRepository>();
            _mockQueryRepository = Substitute.For<ICommunicationQueryRepository>();
        }

        [TestMethod]
        public async Task UpdateCommunicationTemplateCommandHandler_Should_CallUpdateCommunicationTemplateAsyncOnRepository()
        {
            var request = new UpdateCommunicationTemplateRequest()
            {
                Id = Guid.NewGuid(),
                Title = "Test Template Title",
                Subject = "Test Template Subject",
                Content = "Test Template Content",
                TemplateType = 0,
            };
            var command = new UpdateCommunicationTemplateCommand()
            {
                Id = request.Id,
                Title = request.Title,
                Subject = request.Subject,
                Content = request.Content,
                TemplateType = request.TemplateType
            };
            var handler = new UpdateCommunicationTemplateCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).UpdateCommunicationTemplateAsync(command, default);
        }

        [TestMethod]
        public void UpdateCommunicationTemplateCommandHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new UpdateCommunicationTemplateCommand()
            {
                Id = Guid.Empty,
                Title = "Test Template Title",
            };
            var validator = new UpdateCommunicationTemplateCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }

        [TestMethod]
        public void UpdateCommunicationTemplateCommandHandler_Should_ReturnValidationErrorOnEmptyTitle()
        {
            var command = new UpdateCommunicationTemplateCommand()
            {
                Id = Guid.NewGuid(),
                Title = string.Empty
            };
            var validator = new UpdateCommunicationTemplateCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Title);
        }

        [TestMethod]
        public void UpdateCommunicationTemplateCommandHandler_Should_ReturnValidationErrorOnDuplicatedTemplate()
        {
            var request = new UpdateCommunicationTemplateRequest()
            {
                Id = Guid.NewGuid(),
                Title = "Test Template Title",
                TemplateType = 0,
            };

            var command = new UpdateCommunicationTemplateCommand()
            {
                Id = request.Id,
                Title = request.Title,
                TemplateType = request.TemplateType
            };
            var validator = new UpdateCommunicationTemplateCommandValidator(_mockQueryRepository);

            _mockQueryRepository
                .CheckIfCommunicationTemplateExistsAsync(command, default)
                .Returns(Task.FromResult(true));

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("communication.error.communication_template_exist");
        }
    }
}
