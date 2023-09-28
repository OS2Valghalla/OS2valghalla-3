
// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://serviceplatformen.dk/xml/print/PKO_PostStatus/1/types")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://serviceplatformen.dk/xml/print/PKO_PostStatus/1/types", IsNullable = false)]
public partial class PKO_PostStatus
{

    private string transmissionIdField;

    private string messageUUIDField;

    private string kanalKodeField;

    private System.DateTime transaktionsDatoTidField;

    private string transaktionsStatusKodeField;

    private string correlationIdField;

    private object fejlDetaljerField;

    /// <remarks/>
    public string TransmissionId
    {
        get
        {
            return this.transmissionIdField;
        }
        set
        {
            this.transmissionIdField = value;
        }
    }

    /// <remarks/>
    public string MessageUUID
    {
        get
        {
            return this.messageUUIDField;
        }
        set
        {
            this.messageUUIDField = value;
        }
    }

    /// <remarks/>
    public string KanalKode
    {
        get
        {
            return this.kanalKodeField;
        }
        set
        {
            this.kanalKodeField = value;
        }
    }

    /// <remarks/>
    public System.DateTime TransaktionsDatoTid
    {
        get
        {
            return this.transaktionsDatoTidField;
        }
        set
        {
            this.transaktionsDatoTidField = value;
        }
    }

    /// <remarks/>
    public string TransaktionsStatusKode
    {
        get
        {
            return this.transaktionsStatusKodeField;
        }
        set
        {
            this.transaktionsStatusKodeField = value;
        }
    }

    /// <remarks/>
    public string CorrelationId
    {
        get
        {
            return this.correlationIdField;
        }
        set
        {
            this.correlationIdField = value;
        }
    }

    /// <remarks/>
    public object FejlDetaljer
    {
        get
        {
            return this.fejlDetaljerField;
        }
        set
        {
            this.fejlDetaljerField = value;
        }
    }
}

