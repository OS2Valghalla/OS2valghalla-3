using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Queries;

namespace Valghalla.Internal.Application.Tests.Tasks.Queries
{
    [TestClass]
    public class GetElectionAreasGeneralInfoQueryHandlerTests
    {
        private readonly IElectionAreaTasksQueryRepository _mockQueryRepository;

        public GetElectionAreasGeneralInfoQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<IElectionAreaTasksQueryRepository>();
        }

        [TestMethod]
        public async Task GetElectionAreasGeneralInfoQueryHandler_Should_CallOnRepository()
        {
            var query = new GetElectionAreasGeneralInfoQuery(Guid.NewGuid());
            var handler = new GetElectionAreasGeneralInfoQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetElectionAreasGeneralInfoAsync(query, default);
        }

        [TestMethod]
        public void GetElectionAreasGeneralInfoQueryHandler_Should_ReturnValidationErrorOnEmptyElectionId()
        {
            var command = new GetElectionAreasGeneralInfoQuery(Guid.Empty);
            var validator = new GetElectionAreasGeneralInfoQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ElectionId);
        }
    }
}
