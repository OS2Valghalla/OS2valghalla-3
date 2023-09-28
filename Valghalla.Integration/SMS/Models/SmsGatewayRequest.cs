using System.Xml;
using System.Xml.Serialization;

namespace Valghalla.Integration.SMS.Models
{
    [XmlRoot("sms")]
    public class SmsGatewayRequest
    {
        [XmlElement(ElementName = "countrycode")]
        public string CountryCode { get; set; } = string.Empty;

        [XmlElement(ElementName = "number")]
        public string Number { get; set; } = string.Empty;

        [XmlElement(ElementName = "messageid")]
        public string MessageID { get; set; } = string.Empty;

        [XmlElement(ElementName = "senderalias")]
        public string SenderAlias { get; set; } = string.Empty;

        [XmlElement(ElementName = "shortcode")]
        public string ShortCode { get; set; } = string.Empty;

        [XmlElement(ElementName = "message")]
        public string Message { get; set; } = string.Empty;

        [XmlElement(ElementName = "encoding")]
        public string Encoding { get; set; } = string.Empty;

        [XmlElement(ElementName = "category")]
        public string Category { get; set; } = string.Empty;

        [XmlElement(ElementName = "category_description")]
        public string CategoryDescription { get; set; } = string.Empty;

        [XmlElement(ElementName = "port")]
        public string Port { get; set; } = string.Empty;

        [XmlElement(ElementName = "wapurl")]
        public string WAPUrl { get; set; } = string.Empty;

        [XmlElement(ElementName = "flash")]
        public string Flash { get; set; } = string.Empty;

        [XmlElement(ElementName = "price")]
        public string Price { get; set; } = string.Empty;

        [XmlElement(ElementName = "donation")]
        public string Donation { get; set; } = string.Empty;

        [XmlElement(ElementName = "callbackurl")]
        public string CallbackUrl { get; set; } = string.Empty;

        [XmlElement(ElementName = "sendtiming")]
        public string SendTiming { get; set; } = string.Empty;

        [XmlElement(ElementName = "bypassqueue")]
        public string BypassQueue { get; set; } = string.Empty;

        public static string Serialize(SmsGatewayRequest value)
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
    }
}
