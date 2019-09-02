namespace BeanSpitter.Interfaces
{
    using System.IO.Abstractions;
    using System.Xml.Schema;

    public interface IXmlValidationUtils
    {
        void ValidateFilePath(string path, IFileSystem fileSystem, string validationMessage = null);
        void ValidateXmlSchemaSet(XmlSchemaSet schemaSet, string validationMessage = null);
    }
}
