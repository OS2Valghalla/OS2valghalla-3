using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.Election.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Election.Queries;

namespace Valghalla.Internal.Application.Tests.Administration.Election.Queries
{
    [TestClass]
    public class GetElectionQueryHandlerTests
    {
        private readonly IElectionQueryRepository _mockQueryRepository;

        public GetElectionQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<IElectionQueryRepository>();
        }

        [TestMethod]
        public async Task GetElectionQueryHandler_Should_CallOnRepository()
        {
            var query = new GetElectionQuery(Guid.NewGuid());
            var handler = new GetElectionQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetElectionAsync(query, default);
        }

        [TestMethod]
        public void GetElectionQueryHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new GetElectionQuery(Guid.Empty);
            var validator = new GetElectionQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }
    }
}
