using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.API.Requests.Administration.Communication;
using Valghalla.Internal.Application.Modules.Administration.Communication.Commands;
using Valghalla.Internal.Application.Modules.Administration.Communication.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.Communication.Commands
{
    [TestClass]
    public class CreateCommunicationTemplateCommandHandlerTests
    {
        private readonly ICommunicationCommandRepository _mockCommandRepository;
        private readonly ICommunicationQueryRepository _mockQueryRepository;

        public CreateCommunicationTemplateCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<ICommunicationCommandRepository>();
            _mockQueryRepository = Substitute.For<ICommunicationQueryRepository>();
        }

        [TestMethod]
        public async Task CreateCommunicationTemplateCommandHandler_Should_CallCreateCommunicationTemplateAsyncOnRepository()
        {
            var request = new CreateCommunicationTemplateRequest()
            {
                Title = "Test Template Title",
                Subject = "Test Template Subject",
                Content = "Test Template Content",
                TemplateType = 0,                
            };
            var command = new CreateCommunicationTemplateCommand()
            {
                Title = request.Title,
                Subject = request.Subject,
                Content = request.Content,
                TemplateType = request.TemplateType
            };
            var handler = new CreateCommunicationTemplateCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).CreateCommunicationTemplateAsync(command, default);
        }

        [TestMethod]
        public void CreateCommunicationTemplateCommandHandler_Should_ReturnValidationErrorOnEmptyTitle()
        {
            var command = new CreateCommunicationTemplateCommand()
            {
                Title = string.Empty
            };
            var validator = new CreateCommunicationTemplateCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Title);
        }

        [TestMethod]
        public void CreateCommunicationTemplateCommandHandler_Should_ReturnValidationErrorOnDuplicatedTemplate()
        {
            var request = new CreateCommunicationTemplateRequest()
            {
                Title = "Test Template Title",
                TemplateType = 0,
            };

            var command = new CreateCommunicationTemplateCommand()
            {
                Title = request.Title,
                TemplateType = request.TemplateType
            };
            var validator = new CreateCommunicationTemplateCommandValidator(_mockQueryRepository);

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
