using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.Area.Commands;
using Valghalla.Internal.Application.Modules.Administration.Area.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.Area.Commands
{
    [TestClass]
    public class DeleteAreaCommandHandlerTests
    {
        private readonly IAreaCommandRepository _mockCommandRepository;
        private readonly IAreaQueryRepository _mockQueryRepository;
        public DeleteAreaCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IAreaCommandRepository>();
            _mockQueryRepository = Substitute.For<IAreaQueryRepository>();
        }

        [TestMethod]
        public async Task DeleteAreaCommandHandler_Should_CallDeleteAreaAsyncOnRepository()
        {
            Guid id = Guid.NewGuid();
            var command = new DeleteAreaCommand(id);
            var handler = new DeleteAreaCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).DeleteAreaAsync(command, default);
        }

        [TestMethod]
        public void DeleteAreaCommandHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new DeleteAreaCommand(Guid.Empty);
            var validator = new DeleteAreaCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }

        [TestMethod]
        public void DeleteAreaCommandHandler_Should_ReturnValidationErrorOnHavingWorkLocations()
        {
            Guid id = Guid.NewGuid();

            var command = new DeleteAreaCommand(id);

            _mockQueryRepository
                .CheckIfAreaHasWorkLocationsAsync(command, default)
                .Returns(Task.FromResult(true));

            var validator = new DeleteAreaCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("administration.area.error.work_location_used");
        }

        [TestMethod]
        public void DeleteAreaCommandHandler_Should_ReturnValidationErrorIsLastArea()
        {
            Guid id = Guid.NewGuid();

            var command = new DeleteAreaCommand(id);

            _mockQueryRepository
                .CheckIfAreaIsLastOneAsync(command, default)
                .Returns(Task.FromResult(true));

            var validator = new DeleteAreaCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("administration.area.error.minimum_area_required");
        }
    }
}
