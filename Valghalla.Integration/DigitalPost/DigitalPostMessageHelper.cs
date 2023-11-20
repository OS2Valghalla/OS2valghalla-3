using Microsoft.AspNetCore.StaticFiles;
using SF1601;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Valghalla.Application.Configuration;
using Valghalla.Application.DigitalPost;

namespace Valghalla.Integration.DigitalPost
{
    internal class DigitalPostMessageHelper
    {
        private readonly AppConfiguration appConfiguration;
        private readonly FileExtensionContentTypeProvider contentTypeProvider;

        public DigitalPostMessageHelper(AppConfiguration appConfiguration)
        {
            this.appConfiguration = appConfiguration;
            contentTypeProvider = new FileExtensionContentTypeProvider();
        }

        public string CreateSendingMessage(DigitalPostMessage message, Guid messageUUID, string timestamp)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true
            };

            var xns = new XmlSerializerNamespaces();
            xns.Add(string.Empty, string.Empty);

            var kombiRequest = new kombi_request
            {
                KombiValgKode = "Digital Post"
            };

            var startserializer = new XmlSerializer(typeof(kombi_request));
            var startXML = "";

            using (var sww1 = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww1, settings))
                {
                    startserializer.Serialize(writer, kombiRequest, xns);
                    startXML = sww1.ToString();
                }
            }

            var startdocument = new XmlDocument();
            startdocument.LoadXml(startXML);

            var kombiMessage = new Message
            {
                memoVersion = 1.1M,
                memoSchVersion = "1.1.0",
                MessageHeader = CreateMessageHeader(message.Cpr, message.Label, messageUUID),
                MessageBody = CreateMessageBody(message.Content, timestamp, message.Attachments)
            };

            kombiRequest.Message = kombiMessage;

            var xsSubmit1 = new XmlSerializer(typeof(Message));
            var xml1 = "";

            using (var sww1 = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww1, settings))
                {
                    xsSubmit1.Serialize(writer, kombiRequest.Message);
                    xml1 = sww1.ToString();
                }
            }

            var xfrag = startdocument.CreateDocumentFragment();
            xfrag.InnerXml = xml1;

            startdocument.LastChild.AppendChild(xfrag);
            return startdocument.InnerXml;
        }

        public void ValidateResponseMessage(string data)
        {
            var xml = XDocument.Parse(data).ToString();
            var serializer = new XmlSerializer(typeof(kombi_response));
            kombi_response? messageReply;

            using (var reader = new StringReader(xml))
            {
                var value = serializer.Deserialize(reader) ??
                    throw new Exception("Could not deserialize xml response from digital post");

                messageReply = (kombi_response)value;
            }

            if (messageReply == null) throw new Exception("Could not deserialize xml response from digital post");

            if (messageReply.Result == true)
            {
                if (messageReply.HovedoplysningerSvarREST.SvarReaktion.Advis.AdvisTekst.ToLower() != "received")
                {
                    throw new DigitalPostException("Error occurred when sending message to digital post", xml);
                }
            }
            else
            {
                throw new DigitalPostException("Error occurred when sending message to digital post", xml);
            }
        }

        private MessageHeader CreateMessageHeader(string cpr, string label, Guid messageUUID)
        {
            var kombiMessageHeader = new MessageHeader
            {
                messageType = "DIGITALPOST",
                messageUUID = messageUUID.ToString(),
                label = label,
                mandatory = false,
                legalNotification = false,
                Sender = new Sender
                {
                    senderID = appConfiguration.DigitalPostCvr,
                    idType = "CVR",
                    label = appConfiguration.DigitalPostSender
                },

                Recipient = new Recipient()
                {
                    recipientID = cpr,
                    idType = "CPR"
                }
            };

            return kombiMessageHeader;
        }

        private MessageBody CreateMessageBody(string content, string timestamp, IEnumerable<(Stream, string)> attachments)
        {
            var kombiMessageBody = new MessageBody
            {
                createdDateTime = timestamp,
                MainDocument = new MainDocument()
                {
                    File = new SF1601.File[]
                    {
                        new SF1601.File("text/html", "content.html", "da", Encoding.UTF8.GetBytes(content))
                    }
                }
            };

            var files = new List<SF1601.File>();
            foreach (var (stream, fileName) in attachments)
            {
                contentTypeProvider.TryGetContentType(fileName, out var contentType);
                var bytes = ((MemoryStream)stream).ToArray();
                var file = new SF1601.File(contentType, fileName, "da", bytes);
                files.Add(file);
            }

            kombiMessageBody.AdditionalDocument = new AdditionalDocument[]
            {
                new AdditionalDocument()
                {
                    File = files.ToArray()
                }
            };

            return kombiMessageBody;
        }
    }
}
