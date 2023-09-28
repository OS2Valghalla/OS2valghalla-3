namespace SF1601
{
    [System.SerializableAttribute()]
    public class kombi_request
    {
        public string KombiValgKode;
        public ForsendelseISamlingType ForsendelseISamling;
        public Message Message;
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oio:fjernprint:1.0.0")]
    public class ForsendelseISamlingType
    {
        public Fjernprint.ForsendelseIType ForsendelseI;
    }
}
