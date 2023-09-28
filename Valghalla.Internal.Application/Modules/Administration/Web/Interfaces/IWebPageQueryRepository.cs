using Valghalla.Application.Web;

namespace Valghalla.Internal.Application.Modules.Administration.Web.Interfaces
{
    public interface IWebPageQueryRepository
    {
        Task<WebPage?> GetWebPageAsync(string pageName, CancellationToken cancellationToken);
    }
}
