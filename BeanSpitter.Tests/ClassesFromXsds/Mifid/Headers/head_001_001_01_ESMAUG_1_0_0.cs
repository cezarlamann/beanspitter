﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

//
// This source code was auto-generated by xsd, Version=4.6.1055.0.
//
namespace BeanSpitter.Tests.XmlSchemaReaderTests.ClassesFromXsds.Mifid.Headers {
    using System.Xml.Serialization;


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:head.001.001.01")]
    [System.Xml.Serialization.XmlRootAttribute("AppHdr", Namespace="urn:iso:std:iso:20022:tech:xsd:head.001.001.01", IsNullable=false)]
    public partial class BusinessApplicationHeaderV01 {

        private Party9Choice__1 frField;

        private Party9Choice__1 toField;

        private string bizMsgIdrField;

        private string msgDefIdrField;

        private System.DateTime creDtField;

        private BusinessApplicationHeader1__1 rltdField;

        /// <remarks/>
        public Party9Choice__1 Fr {
            get {
                return this.frField;
            }
            set {
                this.frField = value;
            }
        }

        /// <remarks/>
        public Party9Choice__1 To {
            get {
                return this.toField;
            }
            set {
                this.toField = value;
            }
        }

        /// <remarks/>
        public string BizMsgIdr {
            get {
                return this.bizMsgIdrField;
            }
            set {
                this.bizMsgIdrField = value;
            }
        }

        /// <remarks/>
        public string MsgDefIdr {
            get {
                return this.msgDefIdrField;
            }
            set {
                this.msgDefIdrField = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreDt {
            get {
                return this.creDtField;
            }
            set {
                this.creDtField = value;
            }
        }

        /// <remarks/>
        public BusinessApplicationHeader1__1 Rltd {
            get {
                return this.rltdField;
            }
            set {
                this.rltdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:head.001.001.01")]
    public partial class Party9Choice__1 {

        private PartyIdentification42__1 itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("OrgId")]
        public PartyIdentification42__1 Item {
            get {
                return this.itemField;
            }
            set {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:head.001.001.01")]
    public partial class PartyIdentification42__1 {

        private Party10Choice__1 idField;

        /// <remarks/>
        public Party10Choice__1 Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:head.001.001.01")]
    public partial class Party10Choice__1 {

        private OrganisationIdentification7__1 itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("OrgId")]
        public OrganisationIdentification7__1 Item {
            get {
                return this.itemField;
            }
            set {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:head.001.001.01")]
    public partial class OrganisationIdentification7__1 {

        private GenericOrganisationIdentification1__1 othrField;

        /// <remarks/>
        public GenericOrganisationIdentification1__1 Othr {
            get {
                return this.othrField;
            }
            set {
                this.othrField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:head.001.001.01")]
    public partial class GenericOrganisationIdentification1__1 {

        private string idField;

        private OrganisationIdentificationSchemeName1Choice schmeNmField;

        /// <remarks/>
        public string Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }

        /// <remarks/>
        public OrganisationIdentificationSchemeName1Choice SchmeNm {
            get {
                return this.schmeNmField;
            }
            set {
                this.schmeNmField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:head.001.001.01")]
    public partial class OrganisationIdentificationSchemeName1Choice {

        private string itemField;

        private ItemChoiceType itemElementNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Cd", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("Prtry", typeof(string))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item {
            get {
                return this.itemField;
            }
            set {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType ItemElementName {
            get {
                return this.itemElementNameField;
            }
            set {
                this.itemElementNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:head.001.001.01", IncludeInSchema=false)]
    public enum ItemChoiceType {

        /// <remarks/>
        Cd,

        /// <remarks/>
        Prtry,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:head.001.001.01")]
    public partial class BusinessApplicationHeader1__1 {

        private Party9Choice__1 frField;

        private Party9Choice__1 toField;

        private string bizMsgIdrField;

        private string msgDefIdrField;

        private System.DateTime creDtField;

        /// <remarks/>
        public Party9Choice__1 Fr {
            get {
                return this.frField;
            }
            set {
                this.frField = value;
            }
        }

        /// <remarks/>
        public Party9Choice__1 To {
            get {
                return this.toField;
            }
            set {
                this.toField = value;
            }
        }

        /// <remarks/>
        public string BizMsgIdr {
            get {
                return this.bizMsgIdrField;
            }
            set {
                this.bizMsgIdrField = value;
            }
        }

        /// <remarks/>
        public string MsgDefIdr {
            get {
                return this.msgDefIdrField;
            }
            set {
                this.msgDefIdrField = value;
            }
        }

        /// <remarks/>
        public System.DateTime CreDt {
            get {
                return this.creDtField;
            }
            set {
                this.creDtField = value;
            }
        }
    }
}
