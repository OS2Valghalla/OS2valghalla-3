using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.User.Commands;
using Valghalla.Internal.Application.Modules.Administration.User.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.User.Commands
{
    [TestClass]
    public class UpdateUserCommandHandlerTests
    {
        private readonly IUserCommandRepository _mockCommandRepository;

        public UpdateUserCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IUserCommandRepository>();
        }

        [TestMethod]
        public async Task UpdateUserCommandHandlerTests_Should_CallUpdateUserAsyncOnRepository()
        {
            var command = new UpdateUserCommand(Guid.NewGuid(), Guid.NewGuid());
            var handler = new UpdateUserCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).UpdateUserAsync(command, default);
        }

        [TestMethod]
        public void UpdateUserCommandHandlerTests_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new UpdateUserCommand(Guid.Empty, Guid.NewGuid());
            var validator = new UpdateUserCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }

        [TestMethod]
        public void UpdateUserCommandHandlerTests_Should_ReturnValidationErrorOnEmptyRoleId()
        {
            var command = new UpdateUserCommand(Guid.NewGuid(), Guid.Empty);
            var validator = new UpdateUserCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.RoleId);
        }
    }
}
