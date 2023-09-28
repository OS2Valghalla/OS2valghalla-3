using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Application.Configuration;
using Valghalla.Application.Tenant;
using Valghalla.Internal.Application.Modules.Administration.Link.Commands;
using Valghalla.Internal.Application.Modules.Administration.Link.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.Link.Commands
{
    [TestClass]
    public class CreateTasksFilteredLinkCommandHandlerTests
    {
        private readonly ILinkCommandRepository _mockCommandRepository;
        private readonly ITenantContextProvider _mockTenantContextProvider;

        public CreateTasksFilteredLinkCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<ILinkCommandRepository>();
            _mockTenantContextProvider = new MockTennantContextProvider();
        }

        [TestMethod]
        public async Task CreateTasksFilteredLinkCommandHandler_Should_CallCreateTasksFilteredLinkAsyncOnRepository()
        {
            var command = new CreateTasksFilteredLinkCommand(Guid.NewGuid(), "TestHashValue", "TestValue");
            var handler = new CreateTasksFilteredLinkCommandHandler(_mockTenantContextProvider, _mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).CreateTasksFilteredLinkAsync(command, default);
        }

        [TestMethod]
        public void CreateTasksFilteredLinkCommandHandler_Should_ReturnValidationErrorOnEmptyElectionId()
        {
            var command = new CreateTasksFilteredLinkCommand(Guid.Empty, "TestHashValue", "TestValue");
            var validator = new CreateTasksFilteredLinkCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ElectionId);
        }

        [TestMethod]
        public void CreateTasksFilteredLinkCommandHandler_Should_ReturnValidationErrorOnEmptyHashValue()
        {
            var command = new CreateTasksFilteredLinkCommand(Guid.NewGuid(), string.Empty, "TestValue");
            var validator = new CreateTasksFilteredLinkCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.HashValue);
        }

        [TestMethod]
        public void CreateTasksFilteredLinkCommandHandler_Should_ReturnValidationErrorOnEmptyValue()
        {
            var command = new CreateTasksFilteredLinkCommand(Guid.NewGuid(), "TestHashValue", string.Empty);
            var validator = new CreateTasksFilteredLinkCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Value);
        }
    }
}
