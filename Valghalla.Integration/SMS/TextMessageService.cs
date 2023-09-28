using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Valghalla.Application.Secret;
using Valghalla.Application.SMS;
using Valghalla.Integration.SMS.Models;

namespace Valghalla.Integration.SMS
{
    internal class TextMessageService : ITextMessageService
    {
        private readonly ISecretService secretService;
        private readonly ILogger<TextMessageService> logger;
        private readonly HttpClient httpClient;

        public TextMessageService(
            HttpClient httpClient,
            ISecretService secretService,
            ILogger<TextMessageService> logger)
        {
            this.logger = logger;
            this.httpClient = httpClient;
            this.secretService = secretService;
        }

        public async Task SendTextMessageAsync(TextMessage value, CancellationToken cancellationToken)
        {
            try
            {
                await EnsureConfig(cancellationToken);

                var request = CreateRequest(value);
                var httpContent = new StringContent(Serialize(request), Encoding.UTF8, "application/xml");
                var httpResponse = await httpClient.PostAsync("sms", httpContent, cancellationToken);
                httpResponse.EnsureSuccessStatusCode();

                var response = await httpResponse.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
                var result = XmlDeserializeFromString(response);

                if (result == null || result.Statusline.Length == 0)
                {
                    logger.LogError("Unable to send message -- {@value}.", value);
                    throw new Exception("Unable to send message");
                }

                foreach (var statusline in result.Statusline)
                {
                    if ("0".Equals(statusline?.Code) == false)
                    {
                        logger.LogWarning("Failure: " + statusline.Code + " / " + statusline.Description);
                        throw new Exception("Failure: " + statusline.Code + " / " + statusline.Description);
                    }
                }


                logger.LogInformation("Sent text message -- {@response}.", response);
            }
            catch (Exception ex)
            {
                logger.LogError("Could not send text message -- Message: {@message} -- Stacktrace: {@stacktrace}.", ex.Message, ex.StackTrace);
                throw;
            }
        }

        private async Task EnsureConfig(CancellationToken cancellationToken)
        {
            if (httpClient.BaseAddress == null)
            {
                var config = await secretService.GetTextMessageConfigurationAsync(cancellationToken);

                var byteArray = Encoding.UTF8.GetBytes($"{config.Username}:{config.Password}");
                httpClient.BaseAddress = new Uri(config.BaseUrl);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                httpClient.DefaultRequestHeaders.Add("Accept", "application/xml");
            }
            else
            {
                await Task.CompletedTask;
            }
        }

        private static SmsGatewayRequest CreateRequest(TextMessage value)
        {
            return new()
            {
                SenderAlias = value.Sender,
                //CountryCode = "45",
                Message = value.Message,
                Number = string.Join(",", value.Recipients)
            };
        }

        private static string Serialize(SmsGatewayRequest value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            try
            {
                var xmlserializer = new XmlSerializer(typeof(SmsGatewayRequest));
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                var stringWriter = new StringWriterWithEncoding();
                using XmlWriter writer = XmlWriter.Create(stringWriter);
                xmlserializer.Serialize(writer, value, ns);
                return stringWriter.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }

        private static SmsGatewayResponse.Status XmlDeserializeFromString(string objectData)
        {
            var serializer = new XmlSerializer(typeof(SmsGatewayResponse.Status));
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return (SmsGatewayResponse.Status)result;
        }
    }
}
