using System.Text.Json;
using Valghalla.Application;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Web.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Web.Commands
{
    public sealed record UpdateDeclarationOfConsentPageCommand() : ICommand<Response>
    {
        public string PageContent { get; init; } = string.Empty;
        public bool IsActivated { get; init; }
    }

    internal class UpdateDeclarationOfConsentPageCommandHandler : ICommandHandler<UpdateDeclarationOfConsentPageCommand, Response>
    {
        private readonly IWebPageCommandRepository webPageCommandRepository;
        public UpdateDeclarationOfConsentPageCommandHandler(IWebPageCommandRepository webPageCommandRepository)
        {
            this.webPageCommandRepository = webPageCommandRepository;
        }

        public async Task<Response> Handle(UpdateDeclarationOfConsentPageCommand command, CancellationToken cancellationToken)
        {
            var jsonText = JsonSerializer.Serialize(command);

            await webPageCommandRepository.UpdateWebPageAsync(Constants.WebPages.WebPageName_DeclarationOfConsentPage, jsonText, cancellationToken);
            return Response.Ok();
        }
    }
}
