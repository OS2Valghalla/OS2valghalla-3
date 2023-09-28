using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.API.Requests.Administration.Area;
using Valghalla.Internal.Application.Modules.Administration.Area.Commands;
using Valghalla.Internal.Application.Modules.Administration.Area.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.Area.Commands
{
    [TestClass]
    public class CreateAreaCommandHandlerTests
    {
        private readonly IAreaCommandRepository _mockCommandRepository;
        private readonly IAreaQueryRepository _mockQueryRepository;
        public CreateAreaCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<IAreaCommandRepository>();
            _mockQueryRepository = Substitute.For<IAreaQueryRepository>();
        }

        [TestMethod]
        public async Task CreateAreaCommandHandler_Should_CallCreateAreaAsyncOnRepository()
        {
            var request = new CreateAreaRequest()
            {
                Name = "Testing Area",
                Description = "Area description"
            };
            var command = new CreateAreaCommand(request.Name, request.Description);
            var handler = new CreateAreaCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).CreateAreaAsync(command, default);
        }

        [TestMethod]
        public void CreateAreaCommandHandler_Should_ReturnValidationErrorOnEmptyName()
        {
            var request = new CreateAreaRequest()
            {
                Name = ""
            };
            var command = new CreateAreaCommand(request.Name, request.Description);
            var validator = new CreateAreaCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Name);
        }

        [TestMethod]
        public void CreateAreaCommandHandler_Should_ReturnValidationErrorOnDuplicatedArea()
        {
            var name = "Testing Area";

            var request = new CreateAreaRequest()
            {
                Name = name
            };

            var command = new CreateAreaCommand(request.Name, request.Description);
            var validator = new CreateAreaCommandValidator(_mockQueryRepository);

            _mockQueryRepository
                .CheckIfAreaExistsAsync(command, default)
                .Returns(Task.FromResult(true));

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("administration.area.error.area_exist");
        }
    }
}
