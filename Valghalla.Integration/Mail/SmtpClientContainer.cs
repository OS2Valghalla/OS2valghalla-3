using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Valghalla.Application.Secret;

namespace Valghalla.Integration.Mail
{
    internal class SmtpClientContainer
    {
        private readonly ISecretService secretService;

        private SmtpClient? _smtpClient;
        private readonly SemaphoreSlim _clientLock = new(1, 1);

        public SmtpClientContainer(ISecretService secretService)
        {
            this.secretService = secretService;
        }

        public async Task SendAsync(MimeMessage mail, CancellationToken cancellationToken)
        {
            await SendAsync(mail, false, cancellationToken);
        }

        private async Task SendAsync(MimeMessage mail, bool retry, CancellationToken cancellationToken)
        {
            var flag = retry;
            await _clientLock.WaitAsync(cancellationToken);

            try
            {
                await EnsureClient(cancellationToken);
                await _smtpClient!.NoOpAsync(cancellationToken);
                await _smtpClient!.SendAsync(mail, cancellationToken);
                flag = true;
            }
            catch
            {
                if (_smtpClient != null)
                {
                    await _smtpClient!.DisconnectAsync(true, cancellationToken);
                }

                _smtpClient = null;

                if (flag)
                {
                    throw;
                }
            }
            finally
            {
                _clientLock.Release();

                if (!flag)
                {
                    await SendAsync(mail, true, cancellationToken);
                }
            }
        }

        private async ValueTask EnsureClient(CancellationToken cancellationToken)
        {
            if (_smtpClient != null) return;

            var config = await secretService.GetMailMessageConfigurationAsync(cancellationToken);

            _smtpClient = new SmtpClient();

            if (config.UseSSL)
            {
                await _smtpClient.ConnectAsync(config.SMPT, config.Port, SecureSocketOptions.SslOnConnect, cancellationToken);
            }
            else if (config.UseStartTls)
            {
                await _smtpClient.ConnectAsync(config.SMPT, config.Port, SecureSocketOptions.StartTls, cancellationToken);
            }
            else
            {
                await _smtpClient.ConnectAsync(config.SMPT, config.Port, SecureSocketOptions.SslOnConnect, cancellationToken);
            }

            await _smtpClient.AuthenticateAsync(config.Username, config.Password, cancellationToken);
        }
    }
}
