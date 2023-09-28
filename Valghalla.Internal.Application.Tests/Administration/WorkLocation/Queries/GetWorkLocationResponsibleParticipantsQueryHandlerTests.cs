using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Queries;

namespace Valghalla.Internal.Application.Tests.Administration.WorkLocation.Queries
{
    [TestClass]
    public class GetWorkLocationResponsibleParticipantsQueryHandlerTests
    {
        private readonly IWorkLocationQueryRepository _mockQueryRepository;

        public GetWorkLocationResponsibleParticipantsQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<IWorkLocationQueryRepository>();
        }

        [TestMethod]
        public async Task GetWorkLocationQueryHandler_Should_CallOnRepository()
        {
            var query = new GetWorkLocationResponsibleParticipantsQuery(Guid.NewGuid());
            var handler = new GetWorkLocationResponsibleParticipantsQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetWorkLocationResponsiblesAsync(query, default);
        }

        [TestMethod]
        public void GetWorkLocationQueryHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new GetWorkLocationResponsibleParticipantsQuery(Guid.Empty);
            var validator = new GetWorkLocationResponsibleParticipantsQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.WorkLocationId);
        }
    }
}
