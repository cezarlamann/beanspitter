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
namespace BeanSpitter.Tests.XmlSchemaReaderTests.ClassesFromXsds.Mifid.International.Feedback {
    using System.Xml.Serialization;


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:auth.031.001.01")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:auth.031.001.01", IsNullable=false)]
    public partial class Document {

        private MessageReportHeader4__1[] finInstrmRptgStsAdvcField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("StsAdvc", IsNullable=false)]
        public MessageReportHeader4__1[] FinInstrmRptgStsAdvc {
            get {
                return this.finInstrmRptgStsAdvcField;
            }
            set {
                this.finInstrmRptgStsAdvcField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:auth.031.001.01")]
    public partial class MessageReportHeader4__1 {

        private string msgRptIdrField;

        private StatusAdviceReport3__1 msgStsField;

        private StatusReportRecord3__1[] rcrdStsField;

        /// <remarks/>
        public string MsgRptIdr {
            get {
                return this.msgRptIdrField;
            }
            set {
                this.msgRptIdrField = value;
            }
        }

        /// <remarks/>
        public StatusAdviceReport3__1 MsgSts {
            get {
                return this.msgStsField;
            }
            set {
                this.msgStsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RcrdSts")]
        public StatusReportRecord3__1[] RcrdSts {
            get {
                return this.rcrdStsField;
            }
            set {
                this.rcrdStsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:auth.031.001.01")]
    public partial class StatusAdviceReport3__1 {

        private ReportingMessageStatus1Code__1 stsField;

        private GenericValidationRuleIdentification1__1[] vldtnRuleField;

        private OriginalReportStatistics3__1 sttstcsField;

        /// <remarks/>
        public ReportingMessageStatus1Code__1 Sts {
            get {
                return this.stsField;
            }
            set {
                this.stsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("VldtnRule")]
        public GenericValidationRuleIdentification1__1[] VldtnRule {
            get {
                return this.vldtnRuleField;
            }
            set {
                this.vldtnRuleField = value;
            }
        }

        /// <remarks/>
        public OriginalReportStatistics3__1 Sttstcs {
            get {
                return this.sttstcsField;
            }
            set {
                this.sttstcsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:auth.031.001.01")]
    public enum ReportingMessageStatus1Code__1 {

        /// <remarks/>
        ACPT,

        /// <remarks/>
        PART,

        /// <remarks/>
        RJCT,

        /// <remarks/>
        RMDR,

        /// <remarks/>
        CRPT,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:auth.031.001.01")]
    public partial class GenericValidationRuleIdentification1__1 {

        private string idField;

        private string descField;

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
        public string Desc {
            get {
                return this.descField;
            }
            set {
                this.descField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:auth.031.001.01")]
    public partial class StatusReportRecord3__1 {

        private string orgnlRcrdIdField;

        private ReportingRecordStatus1Code__1 stsField;

        private GenericValidationRuleIdentification1__1[] vldtnRuleField;

        /// <remarks/>
        public string OrgnlRcrdId {
            get {
                return this.orgnlRcrdIdField;
            }
            set {
                this.orgnlRcrdIdField = value;
            }
        }

        /// <remarks/>
        public ReportingRecordStatus1Code__1 Sts {
            get {
                return this.stsField;
            }
            set {
                this.stsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("VldtnRule")]
        public GenericValidationRuleIdentification1__1[] VldtnRule {
            get {
                return this.vldtnRuleField;
            }
            set {
                this.vldtnRuleField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:auth.031.001.01")]
    public enum ReportingRecordStatus1Code__1 {

        /// <remarks/>
        ACPT,

        /// <remarks/>
        RJCT,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:auth.031.001.01")]
    public partial class NumberOfRecordsPerStatus1__1 {

        private string dtldNbOfRcrdsField;

        private ReportingRecordStatus1Code__1 dtldStsField;

        /// <remarks/>
        public string DtldNbOfRcrds {
            get {
                return this.dtldNbOfRcrdsField;
            }
            set {
                this.dtldNbOfRcrdsField = value;
            }
        }

        /// <remarks/>
        public ReportingRecordStatus1Code__1 DtldSts {
            get {
                return this.dtldStsField;
            }
            set {
                this.dtldStsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:auth.031.001.01")]
    public partial class OriginalReportStatistics3__1 {

        private string ttlNbOfRcrdsField;

        private NumberOfRecordsPerStatus1__1[] nbOfRcrdsPerStsField;

        /// <remarks/>
        public string TtlNbOfRcrds {
            get {
                return this.ttlNbOfRcrdsField;
            }
            set {
                this.ttlNbOfRcrdsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("NbOfRcrdsPerSts")]
        public NumberOfRecordsPerStatus1__1[] NbOfRcrdsPerSts {
            get {
                return this.nbOfRcrdsPerStsField;
            }
            set {
                this.nbOfRcrdsPerStsField = value;
            }
        }
    }
}