namespace Valghalla.Application.CPR
{
    public interface ICPRService
    {
        Task<CprPersonInfo> ExecuteAsync(string cpr);
    }
}
