namespace Valghalla.Application.Mail
{
    public interface IMailMessageService
    {
        Task SendWithAttachmentsAsync(MailDataWithAttachments mailData, CancellationToken cancellationToken);
    }
}
