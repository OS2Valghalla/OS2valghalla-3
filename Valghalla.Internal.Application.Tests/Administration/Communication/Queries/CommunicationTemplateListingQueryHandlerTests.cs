using NSubstitute;
using Valghalla.Application.QueryEngine;
using Valghalla.Application.QueryEngine.Values;
using Valghalla.Internal.Application.Modules.Administration.Communication.Queries;
using Valghalla.Internal.Application.Modules.Administration.Communication.Responses;

namespace Valghalla.Internal.Application.Tests.Administration.Communication.Queries
{
    [TestClass]
    public class CommunicationTemplateListingQueryHandlerTests
    {
        private readonly IQueryEngineRepository<CommunicationTemplateListingQueryForm, CommunicationTemplateListingItemResponse, CommunicationTemplateListingQueryFormParameters> _mockQueryEngineRepository;

        public CommunicationTemplateListingQueryHandlerTests()
        {
            _mockQueryEngineRepository = Substitute.For<IQueryEngineRepository<CommunicationTemplateListingQueryForm, CommunicationTemplateListingItemResponse, CommunicationTemplateListingQueryFormParameters>>();
        }

        [TestMethod]
        public async Task CommunicationTemplateListingQueryHandler_Should_CallExecuteQueryOnRepository()
        {
            var query = new CommunicationTemplateListingQueryForm()
            {
                TemplateType = new SingleSelectionFilterValue<int>(0)
            };

            var handler = new QueryEngineHandler<CommunicationTemplateListingQueryForm, CommunicationTemplateListingItemResponse, CommunicationTemplateListingQueryFormParameters>(_mockQueryEngineRepository);
            
            await handler.Handle(query, default);

            await _mockQueryEngineRepository.Received(1).ExecuteQuery(query, default);
        }
    }
}
