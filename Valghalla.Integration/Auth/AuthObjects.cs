namespace Valghalla.Integration.Auth
{
    public static class AuthObjects
    {
        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://itst.dk/oiosaml/basic_privilege_profile")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://itst.dk/oiosaml/basic_privilege_profile", IsNullable = false)]
        public class PrivilegeList
        {

            private PrivilegeGroup privilegeGroupField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
            public PrivilegeGroup PrivilegeGroup
            {
                get
                {
                    return this.privilegeGroupField;
                }
                set
                {
                    this.privilegeGroupField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public class PrivilegeGroup
        {

            private string privilegeField;

            private string scopeField;

            /// <remarks/>
            public string Privilege
            {
                get
                {
                    return this.privilegeField;
                }
                set
                {
                    this.privilegeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Scope
            {
                get
                {
                    return this.scopeField;
                }
                set
                {
                    this.scopeField = value;
                }
            }
        }
    }
}
