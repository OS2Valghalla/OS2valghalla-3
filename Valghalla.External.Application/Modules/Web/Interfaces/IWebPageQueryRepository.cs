using Valghalla.Application.Web;

namespace Valghalla.External.Application.Modules.Web.Interfaces
{
    public interface IWebPageQueryRepository
    {
        Task<WebPage?> GetWebPageAsync(string pageName, CancellationToken cancellationToken);
    }
}
