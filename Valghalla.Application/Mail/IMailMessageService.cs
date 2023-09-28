namespace Valghalla.Application.Mail
{
    public interface IMailMessageService
    {
        Task<bool> SendWithAttachmentsAsync(MailDataWithAttachments mailData, CancellationToken cancellationToken);
    }
}
