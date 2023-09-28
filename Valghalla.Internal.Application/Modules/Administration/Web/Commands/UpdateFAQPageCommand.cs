using System.Text.Json;
using Valghalla.Application;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Web.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Web.Commands
{
    public sealed record UpdateFAQPageCommand() : ICommand<Response>
    {
        public string PageContent { get; init; } = string.Empty;
        public bool IsActivated { get; set; }
    }

    internal class UpdateFAQPageCommandHandler : ICommandHandler<UpdateFAQPageCommand, Response>
    {
        private readonly IWebPageCommandRepository webPageCommandRepository;
        public UpdateFAQPageCommandHandler(IWebPageCommandRepository webPageCommandRepository)
        {
            this.webPageCommandRepository = webPageCommandRepository;
        }

        public async Task<Response> Handle(UpdateFAQPageCommand command, CancellationToken cancellationToken)
        {
            var jsonText = JsonSerializer.Serialize(command);

            await webPageCommandRepository.UpdateWebPageAsync(Constants.WebPages.WebPageName_FAQPage, jsonText, cancellationToken);
            return Response.Ok();
        }
    }
}
