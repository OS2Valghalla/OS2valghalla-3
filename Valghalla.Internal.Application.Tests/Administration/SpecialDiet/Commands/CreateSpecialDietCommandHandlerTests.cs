using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.API.Requests.Administration.SpecialDiet;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Commands;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.SpecialDiet.Commands
{
    [TestClass]
    public class CreateSpecialDietCommandHandlerTests
    {
        private readonly ISpecialDietCommandRepository _mockCommandRepository;
        private readonly ISpecialDietQueryRepository _mockQueryRepository;

        public CreateSpecialDietCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<ISpecialDietCommandRepository>();
            _mockQueryRepository = Substitute.For<ISpecialDietQueryRepository>();
        }

        [TestMethod]
        public async Task CreateSpecialDietCommandHandler_Should_CallCreateSpecialDietAsyncOnRepository()
        {
            var request = new CreateSpecialDietRequest()
            {
                Title = "Testing Special Diet"
            };
            var command = new CreateSpecialDietCommand(request.Title);
            var handler = new CreateSpecialDietCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).CreateSpecialDietAsync(command, default);
        }

        [TestMethod]
        public void CreateSpecialDietCommandHandler_Should_ReturnValidationErrorOnEmptyTitle()
        {
            var command = new CreateSpecialDietCommand(string.Empty);
            var validator = new CreateSpecialDietCommandValidator(_mockQueryRepository);

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Title);
        }

        [TestMethod]
        public void CreateSpecialDietCommandHandler_Should_ReturnValidationErrorOnDuplicatedSpecialDiet()
        {
            var command = new CreateSpecialDietCommand("Testing Special Diet");
            var validator = new CreateSpecialDietCommandValidator(_mockQueryRepository);

            _mockQueryRepository
                .CheckIfSpecialDietExistsAsync(command, default)
                .Returns(Task.FromResult(true));

            var result = validator.TestValidate(command);

            result
                .ShouldHaveAnyValidationError()
                .WithErrorMessage("administration.specialdiet.error.specialdiet_exist");
        }
    }
}
