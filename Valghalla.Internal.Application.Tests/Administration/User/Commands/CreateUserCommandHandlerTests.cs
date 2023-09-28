using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.User.Commands;
using Valghalla.Internal.Application.Modules.Administration.User.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.User.Commands
{
    [TestClass]
    public class CreateUserCommandHandlerTests
    {
        private readonly IUserCommandRepository _mockCommandRepository;
        private readonly IUserQueryRepository _mockQueryRepository;

        public CreateUserCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IUserCommandRepository>();
            _mockQueryRepository = Substitute.For<IUserQueryRepository>();
        }

        [TestMethod]
        public async Task CreateUserCommandHandlerTests_Should_CallCreateUserAsyncOnRepository()
        {
            var command = new CreateUserCommand(Guid.NewGuid(), "Testing User", "11111111", "22222222");
            var handler = new CreateUserCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).CreateUserAsync(command, default);
        }

        [TestMethod]
        public void CreateUserCommandHandlerTests_Should_ReturnValidationErrorOnEmptyRoleId()
        {
            var command = new CreateUserCommand(Guid.Empty, "Testing User", "11111111", "22222222");
            var validator = new CreateUserCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.RoleId);
        }

        [TestMethod]
        public void CreateUserCommandHandlerTests_Should_ReturnValidationErrorOnEmptyName()
        {
            var command = new CreateUserCommand(Guid.NewGuid(), string.Empty, "11111111", "22222222");
            var validator = new CreateUserCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Name);
        }

        [TestMethod]
        public void CreateUserCommandHandlerTests_Should_ReturnValidationErrorOnEmptyCvr()
        {
            var command = new CreateUserCommand(Guid.NewGuid(), "Testing User", string.Empty, "22222222");
            var validator = new CreateUserCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Cvr);
        }

        [TestMethod]
        public void CreateUserCommandHandlerTests_Should_ReturnValidationErrorOnEmptySerial()
        {
            var command = new CreateUserCommand(Guid.NewGuid(), "Testing User", "11111111", string.Empty);
            var validator = new CreateUserCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Serial);
        }

        [TestMethod]
        public void CreateUserCommandHandlerTests_Should_ReturnValidationErrorOnDuplicatedUser()
        {
            var command = new CreateUserCommand(Guid.NewGuid(), "Testing User", "11111111", "22222222");
            var validator = new CreateUserCommandValidator(_mockQueryRepository);

            _mockQueryRepository
                .CheckIfUserExistsAsync(command, default)
                .Returns(Task.FromResult(true));

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("administration.user.error.user_exists");
        }
    }
}
