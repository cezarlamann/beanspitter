namespace BeanSpitter
{
    using BeanSpitter.Interfaces;
    using System.Xml;
    using System.Xml.Schema;

    public class XmlReaderSettingsFactory : IXmlReaderSettingsFactory
    {
        public XmlReaderSettings CreateXmlSettings(params object[] parameters)
        {
            var result = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema,
                ConformanceLevel = ConformanceLevel.Fragment,
                IgnoreWhitespace = true,
                IgnoreComments = true,
                ValidationFlags =
                    XmlSchemaValidationFlags.ProcessInlineSchema |
                    XmlSchemaValidationFlags.ProcessSchemaLocation |
                    XmlSchemaValidationFlags.AllowXmlAttributes
            };
            return result;
        }
    }
}
