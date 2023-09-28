using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Valghalla.MessageReceiver.Helpers
{
    public static class XmlHelper
    {
        public static PKO_PostStatus DecodePostMessageBase64(string value)
        {
            try
            {
                var postMessage = ASCIIEncoding.ASCII.GetString(System.Convert.FromBase64String(value));

                XmlSerializer serializer = new XmlSerializer(typeof(PKO_PostStatus));
                PKO_PostStatus messageStatus;

                using (TextReader reader = new StringReader(postMessage))
                {
                    messageStatus = (PKO_PostStatus)serializer.Deserialize(reader);
                }

                return messageStatus;
            }
            catch(Exception e)
            {
                throw;
                
            }
        }
    }
}
