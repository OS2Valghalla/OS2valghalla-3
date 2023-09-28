using System.Text.Json;
using Valghalla.Application;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Web.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Web.Commands
{
    public sealed record UpdateDisclosureStatementPageCommand() : ICommand<Response>
    {
        public string PageContent { get; init; } = string.Empty;
    }

    internal class UpdateDisclosureStatementPageCommandHandler : ICommandHandler<UpdateDisclosureStatementPageCommand, Response>
    {
        private readonly IWebPageCommandRepository webPageCommandRepository;
        public UpdateDisclosureStatementPageCommandHandler(IWebPageCommandRepository webPageCommandRepository)
        {
            this.webPageCommandRepository = webPageCommandRepository;
        }

        public async Task<Response> Handle(UpdateDisclosureStatementPageCommand command, CancellationToken cancellationToken)
        {
            var jsonText = JsonSerializer.Serialize(command);

            await webPageCommandRepository.UpdateWebPageAsync(Constants.WebPages.WebPageName_DisclosureStatementPage, jsonText, cancellationToken);
            return Response.Ok();
        }
    }
}
