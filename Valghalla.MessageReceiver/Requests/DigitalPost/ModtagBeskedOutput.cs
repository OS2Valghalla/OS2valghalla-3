
// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
//[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oio:sts:1.0.0")]
//[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oio:sts:1.0.0", IsNullable = false)]
public partial class ModtagBeskedOutput
{

    private StandardRetur standardReturField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
    public StandardRetur StandardRetur
    {
        get
        {
            return this.standardReturField;
        }
        set
        {
            this.standardReturField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
//[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
//[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class StandardRetur
{

    private object statusKodeField;

    private object fejlbeskedTekstField;

    /// <remarks/>
    public object StatusKode
    {
        get
        {
            return this.statusKodeField;
        }
        set
        {
            this.statusKodeField = value;
        }
    }

    /// <remarks/>
    public object FejlbeskedTekst
    {
        get
        {
            return this.fejlbeskedTekstField;
        }
        set
        {
            this.fejlbeskedTekstField = value;
        }
    }
}

