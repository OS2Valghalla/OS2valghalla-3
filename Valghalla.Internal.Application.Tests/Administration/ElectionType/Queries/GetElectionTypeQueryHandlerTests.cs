using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Queries;

namespace Valghalla.Internal.Application.Tests.Administration.ElectionTypeType.Queries
{
    [TestClass]
    public class GetElectionTypeQueryHandlerTests
    {
        private readonly IElectionTypeQueryRepository _mockQueryRepository;

        public GetElectionTypeQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<IElectionTypeQueryRepository>();
        }

        [TestMethod]
        public async Task GetElectionTypeQueryHandler_Should_CallOnRepository()
        {
            var query = new GetElectionTypeQuery(Guid.NewGuid());
            var handler = new GetElectionTypeQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetElectionTypeAsync(query, default);
        }

        [TestMethod]
        public void GetElectionTypeQueryHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new GetElectionTypeQuery(Guid.Empty);
            var validator = new GetElectionTypeQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }
    }
}
