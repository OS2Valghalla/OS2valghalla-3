using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.User.Commands;
using Valghalla.Internal.Application.Modules.Administration.User.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.User.Commands
{
    [TestClass]
    public class DeleteUserCommandHandlerTests
    {
        private readonly IUserCommandRepository _mockCommandRepository;
        private readonly IUserQueryRepository _mockQueryRepository;

        public DeleteUserCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IUserCommandRepository>();
            _mockQueryRepository = Substitute.For<IUserQueryRepository>();
        }

        [TestMethod]
        public async Task DeleteUserCommandHandler_Should_CallDeleteUserAsyncOnRepository()
        {
            Guid id = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var command = new DeleteUserCommand(id, userId);
            var handler = new DeleteUserCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).DeleteUserAsync(command, default);
        }

        [TestMethod]
        public void DeleteUserCommandHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new DeleteUserCommand(Guid.Empty, Guid.NewGuid());
            var validator = new DeleteUserCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }

        [TestMethod]
        public void DeleteUserCommandHandler_Should_ReturnValidationErrorOnEmptyUserId()
        {
            var command = new DeleteUserCommand(Guid.NewGuid(), Guid.Empty);
            var validator = new DeleteUserCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.UserId);
        }

        [TestMethod]
        public void DeleteUserCommandHandler_Should_ReturnValidationErrorOnSelfRemoving()
        {
            Guid id = Guid.NewGuid();

            var command = new DeleteUserCommand(id, id);

            var validator = new DeleteUserCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("administration.user.remove_yourself");
        }

        [TestMethod]
        public void DeleteUserCommandHandler_Should_ReturnValidationErrorOnUserHasConnections()
        {
            Guid id = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var command = new DeleteUserCommand(id, userId);

            _mockQueryRepository
                .CheckIfUserCanBeDeletedAsync(command, default)
                .Returns(Task.FromResult(false));

            var validator = new DeleteUserCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("administration.user.connections");
        }
    }
}
