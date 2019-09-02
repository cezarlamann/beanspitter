using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("BeanSpitter.Tests")]

namespace BeanSpitter.Utils
{
    using BeanSpitter.Interfaces;
    using System;
    using System.IO.Abstractions;
    using System.Xml.Schema;

    public class XmlValidationUtils : IXmlValidationUtils
    {
        internal const string emptyPathMsg = "The given file path cannot be null or empty.";
        internal const string fileCannotBeReadMsg = "The given file path points to a file that couldn't be opened.";
        internal const string nonExistantFilePathMsg = "The given file path points to a non-existant file.";
        internal const string schemaEmptyMsg = "The given XmlSchemaSet object cannot be empty.";
        internal const string schemaNullMsg = "The given XmlSchemaSet object cannot be null.";

        public void ValidateFilePath(string path, IFileSystem fileSystem, string validationMessage = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path), string.IsNullOrEmpty(validationMessage) ? emptyPathMsg : validationMessage);
            }
            if (!fileSystem.File.Exists(path))
            {
                throw new ArgumentException(string.IsNullOrEmpty(validationMessage) ? nonExistantFilePathMsg : validationMessage, nameof(path));
            }
            try
            {
                using (var fs = fileSystem.File.OpenRead(path))
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
            catch (Exception e)
            {
                throw new Exception($"{fileCannotBeReadMsg} {e.Message}", e);
            }
        }

        public void ValidateXmlSchemaSet(XmlSchemaSet schemaSet, string validationMessage = null)
        {
            if (schemaSet == null)
            {
                throw new ArgumentNullException(nameof(schemaSet), string.IsNullOrEmpty(validationMessage) ? schemaNullMsg : validationMessage);
            }
            if (schemaSet.Schemas().Count == 0)
            {
                throw new ArgumentException(string.IsNullOrEmpty(validationMessage) ? schemaEmptyMsg : validationMessage, nameof(schemaSet));
            }
        }
    }
}
