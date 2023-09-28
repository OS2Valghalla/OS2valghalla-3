using FluentValidation.TestHelper;
using NSubstitute;
using Valghalla.Internal.Application.Modules.Administration.Communication.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Communication.Queries;

namespace Valghalla.Internal.Application.Tests.Administration.Communication.Queries
{
    [TestClass]
    public class GetCommunicationTemplateQueryHandlerTests
    {
        private readonly ICommunicationQueryRepository _mockQueryRepository;

        public GetCommunicationTemplateQueryHandlerTests()
        {
            _mockQueryRepository = Substitute.For<ICommunicationQueryRepository>();
        }

        [TestMethod]
        public async Task GetCommunicationTemplateQueryHandler_Should_CallOnRepository()
        {
            var query = new GetCommunicationTemplateQuery(Guid.NewGuid());

            var handler = new GetCommunicationTemplateQueryHandler(_mockQueryRepository);

            await handler.Handle(query, default);

            await _mockQueryRepository.Received(1).GetCommunicationTemplateAsync(query, default);
        }

        [TestMethod]
        public void GetCommunicationTemplateQueryHandler_Should_ReturnValidationErrorOnEmptyId()
        {
            var command = new GetCommunicationTemplateQuery(Guid.Empty);
            var validator = new GetCommunicationTemplateQueryValidator();

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(command => command.Id);
        }
    }
}
