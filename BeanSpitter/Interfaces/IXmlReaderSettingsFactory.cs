namespace BeanSpitter.Interfaces
{
    using System.Xml;

    public interface IXmlReaderSettingsFactory
    {
        XmlReaderSettings CreateXmlSettings(params object[] parameters);
    }
}
