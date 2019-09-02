namespace BeanSpitter.Models
{
    using BeanSpitter.Interfaces;
    using System;
    using System.Threading;
    using System.Xml.Schema;

    public class ValidationErrorEventArgs : EventArgs
    {
        public ValidationErrorEventArgs(Exception ex) : base()
        {
            Exception = ex ?? throw new ArgumentNullException(nameof(ex));
            Severity = XmlSeverityType.Error;
        }

        public ValidationErrorEventArgs(Exception ex, XmlSeverityType severity) : base()
        {
            Exception = ex ?? throw new ArgumentNullException(nameof(ex));
            Exception = ex;
            Severity = severity;
        }

        public ValidationErrorEventArgs(ValidationEventArgs args) : this(args.Exception, args.Severity)
        {
        }

        public Exception Exception { get; private set; }
        public string Message =>

                string.IsNullOrEmpty(Exception.Message) ?
                    string.Empty :
                    Exception.Message;

        public XmlSeverityType Severity { get; private set; }
    }
}
