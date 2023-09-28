using System.Xml.Serialization;

namespace Valghalla.Integration.SMS.Models
{
    public class SmsGatewayResponse
    {
        [XmlRoot("status")]
        public class Status
        {
            [XmlElement(ElementName = "statusline")]
            public Statusline[] Statusline { get; set; } = Array.Empty<Statusline>();
        }

        public class Statusline
        {
            [XmlElement(ElementName = "code")]
            public string Code { get; set; } = string.Empty;

            [XmlElement(ElementName = "description")]
            public string Description { get; set; } = string.Empty;
        }
    }
}
