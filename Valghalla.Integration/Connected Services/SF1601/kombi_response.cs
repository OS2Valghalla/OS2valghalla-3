namespace SF1601
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class kombi_response
    {

        private bool resultField;

        private string transmissionIDField;

        private kombi_responseHovedoplysningerSvarREST hovedoplysningerSvarRESTField;

        /// <remarks/>
        public bool Result
        {
            get
            {
                return this.resultField;
            }
            set
            {
                this.resultField = value;
            }
        }

        /// <remarks/>
        public string TransmissionID
        {
            get
            {
                return this.transmissionIDField;
            }
            set
            {
                this.transmissionIDField = value;
            }
        }

        /// <remarks/>
        public kombi_responseHovedoplysningerSvarREST HovedoplysningerSvarREST
        {
            get
            {
                return this.hovedoplysningerSvarRESTField;
            }
            set
            {
                this.hovedoplysningerSvarRESTField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class kombi_responseHovedoplysningerSvarREST
    {

        private kombi_responseHovedoplysningerSvarRESTSvarReaktion svarReaktionField;

        /// <remarks/>
        public kombi_responseHovedoplysningerSvarRESTSvarReaktion SvarReaktion
        {
            get
            {
                return this.svarReaktionField;
            }
            set
            {
                this.svarReaktionField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class kombi_responseHovedoplysningerSvarRESTSvarReaktion
    {

        private kombi_responseHovedoplysningerSvarRESTSvarReaktionAdvis advisField;

        /// <remarks/>
        public kombi_responseHovedoplysningerSvarRESTSvarReaktionAdvis Advis
        {
            get
            {
                return this.advisField;
            }
            set
            {
                this.advisField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class kombi_responseHovedoplysningerSvarRESTSvarReaktionAdvis
    {

        private string advisIdField;

        private string advisTekstField;

        /// <remarks/>
        public string AdvisId
        {
            get
            {
                return this.advisIdField;
            }
            set
            {
                this.advisIdField = value;
            }
        }

        /// <remarks/>
        public string AdvisTekst
        {
            get
            {
                return this.advisTekstField;
            }
            set
            {
                this.advisTekstField = value;
            }
        }
    }


}