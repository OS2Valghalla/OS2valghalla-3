using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Queries;

namespace Valghalla.Internal.Application.Tests.Tasks.Queries
{
    [TestClass]
    public class GetElectionAreaTasksSummaryQueryHandlerTests
    {
        private readonly IElectionAreaTasksQueryRepository _mockQueryRepository;

        public GetElectionAreaTasksSummaryQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<IElectionAreaTasksQueryRepository>();
        }

        [TestMethod]
        public async Task GetElectionAreaTasksSummaryQueryHandler_Should_CallOnRepository()
        {
            var query = new GetElectionAreaTasksSummaryQuery(Guid.NewGuid(), null, null);
            var handler = new GetElectionAreaTasksSummaryQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetElectionAreaTasksSummaryAsync(query, default);
        }

        [TestMethod]
        public void GetElectionAreaTasksSummaryQueryHandler_Should_ReturnValidationErrorOnEmptyElectionId()
        {
            var command = new GetElectionAreaTasksSummaryQuery(Guid.Empty, null, null);
            var validator = new GetElectionAreaTasksSummaryQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.ElectionId);
        }
    }
}
