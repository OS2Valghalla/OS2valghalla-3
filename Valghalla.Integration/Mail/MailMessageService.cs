using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using MimeKit;
using Valghalla.Application.Configuration;
using Valghalla.Application.Mail;
using Valghalla.Application.Secret;

namespace Valghalla.Integration.Mail
{
    internal class MailMessageService : IMailMessageService
    {
        private readonly ISecretService secretService;
        private readonly ILogger<MailMessageService> logger;
        private readonly AppConfiguration appConfiguration;
        private readonly FileExtensionContentTypeProvider contentTypeProvider;
        private readonly SmtpClientContainer smtpClientContainer;

        public MailMessageService(
            ISecretService secretService,
            ILogger<MailMessageService> logger,
            AppConfiguration appConfiguration,
            SmtpClientContainer smtpClientContainer)
        {
            this.logger = logger;
            this.secretService = secretService;
            this.appConfiguration = appConfiguration;
            contentTypeProvider = new FileExtensionContentTypeProvider();
            this.smtpClientContainer = smtpClientContainer;
        }

        public async Task SendWithAttachmentsAsync(MailDataWithAttachments mailData, CancellationToken cancellationToken)
        {
            try
            {
                var config = await secretService.GetMailMessageConfigurationAsync(cancellationToken);

                // Initialize a new instance of the MimeKit.MimeMessage class
                var mail = new MimeMessage();

                #region Sender / Receiver
                // Sender
                mail.From.Add(new MailboxAddress(appConfiguration.MailSender, appConfiguration.MailAddress));
                mail.Sender = new MailboxAddress(appConfiguration.MailSender, appConfiguration.MailAddress);

                // Receiver
                foreach (string mailAddress in mailData.To)
                    mail.To.Add(MailboxAddress.Parse(mailAddress));

                // Set Reply to if specified in mail data
                if (!string.IsNullOrEmpty(mailData.ReplyTo))
                    mail.ReplyTo.Add(new MailboxAddress(mailData.ReplyToName, mailData.ReplyTo));

                // BCC
                // Check if a BCC was supplied in the request
                if (mailData.Bcc != null)
                {
                    // Get only addresses where value is not null or with whitespace. x = value of address
                    foreach (string mailAddress in mailData.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
                        mail.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                }

                // CC
                // Check if a CC address was supplied in the request
                if (mailData.Cc != null)
                {
                    foreach (string mailAddress in mailData.Cc.Where(x => !string.IsNullOrWhiteSpace(x)))
                        mail.Cc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                }
                #endregion

                #region Content

                // Add Content to Mime Message
                var body = new BodyBuilder();
                mail.Subject = mailData.Subject;
                body.HtmlBody = mailData.Body;
                


                // Check if we got any attachments and add the to the builder for our message
                if (mailData.Attachments != null)
                {
                    foreach (var (stream, fileName) in mailData.Attachments)
                    {
                        var bytes = ((MemoryStream)stream).ToArray();

                        // Add the attachment from the byte array
                        contentTypeProvider.TryGetContentType(fileName, out var contentType);
                        body.Attachments.Add(fileName, bytes, ContentType.Parse(contentType));
                    }
                }
                #endregion
                mail.Body = body.ToMessageBody();

                await smtpClientContainer.SendAsync(mail, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError("Could not send text message -- Message: {@message} -- Stacktrace: {@stacktrace}.", ex.Message, ex.StackTrace);
                throw;
            }
        }
    }
}
