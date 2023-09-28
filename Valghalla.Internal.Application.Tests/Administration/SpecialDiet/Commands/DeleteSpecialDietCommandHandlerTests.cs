using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Commands;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Interfaces;

namespace Valghalla.Internal.Application.Tests.Administration.SpecialDiet.Commands
{
    [TestClass]
    public class DeleteSpecialDietCommandHandlerTests
    {
        private readonly ISpecialDietCommandRepository _mockCommandRepository;

        public DeleteSpecialDietCommandHandlerTests()
        {
            _mockCommandRepository = Substitute.For<ISpecialDietCommandRepository>();
        }

        [TestMethod]
        public async Task DeleteSpecialDietCommandHandler_Should_CallDeleteSpecialDietAsyncOnRepository()
        {
            Guid id = Guid.NewGuid();
            var command = new DeleteSpecialDietCommand(id);
            var handler = new DeleteSpecialDietCommandHandler(_mockCommandRepository);

            await handler.Handle(command, default);

            await _mockCommandRepository.Received(1).DeleteSpecialDietAsync(command, default);
        }

        [TestMethod]
        public void DeleteSpecialDietCommandHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new DeleteSpecialDietCommand(Guid.Empty);
            var validator = new DeleteSpecialDietCommandValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }
    }
}
