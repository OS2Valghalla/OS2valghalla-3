namespace Valghalla.Internal.Application.Modules.Administration.Web.Interfaces
{
    public interface IWebPageCommandRepository
    {
        Task<bool> UpdateWebPageAsync(string pageName, string pageInfo, CancellationToken cancellationToken);
    }
}