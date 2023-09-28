using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Application.Configuration;
using Valghalla.Application.Tenant;
using Valghalla.Internal.Application.Modules.Administration.Link.Commands;
using Valghalla.Internal.Application.Modules.Administration.Link.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.Link.Commands
{
    [TestClass]
    public class CreateTaskLinkCommandHandlerTests
    {
        private readonly ILinkCommandRepository _mockCommandRepository;
        private readonly ITenantContextProvider _mockTenantContextProvider;

        public CreateTaskLinkCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<ILinkCommandRepository>();
            _mockTenantContextProvider = new MockTennantContextProvider();
        }

        [TestMethod]
        public async Task CreateTaskLinkCommandHandler_Should_CallCreateTaskLinkAsyncOnRepository()
        {
            var command = new CreateTaskLinkCommand(Guid.NewGuid(), "TestHashValue", "TestValue");
            var handler = new CreateTaskLinkCommandHandler(_mockTenantContextProvider, _mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).CreateTaskLinkAsync(command, default);
        }

        [TestMethod]
        public void CreateTaskLinkCommandHandler_Should_ReturnValidationErrorOnEmptyElectionId()
        {
            var command = new CreateTaskLinkCommand(Guid.Empty, "TestHashValue", "TestValue");
            var validator = new CreateTaskLinkCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ElectionId);
        }

        [TestMethod]
        public void CreateTaskLinkCommandHandler_Should_ReturnValidationErrorOnEmptyHashValue()
        {
            var command = new CreateTaskLinkCommand(Guid.NewGuid(), string.Empty, "TestValue");
            var validator = new CreateTaskLinkCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.HashValue);
        }

        [TestMethod]
        public void CreateTaskLinkCommandHandler_Should_ReturnValidationErrorOnEmptyValue()
        {
            var command = new CreateTaskLinkCommand(Guid.NewGuid(), "TestHashValue", string.Empty);
            var validator = new CreateTaskLinkCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Value);
        }
    }
}
