namespace BeanSpitter.Interfaces
{
    using BeanSpitter.Models;
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Schema;

    public interface IXmlValidator
    {
        event Func<object, ValidationErrorEventArgs, CancellationToken, Task> ErrorOccurred;
        event Func<object, ValidationFinishedEventArgs, CancellationToken, Task> ValidationFinished;
        void OnErrorOccurred(object sender, ValidationErrorEventArgs e, CancellationToken cancellationToken);
        void OnFinishedValidating(object sender, ValidationFinishedEventArgs e, CancellationToken cancellationToken);

        Task<ValidationFinishedEventArgs> ValidateXmlByteArrayAgainstSchemaAsync(byte[] array, XmlSchemaSet schemaSet, bool reportErrorListAtTheEndOfValidation);
        Task<ValidationFinishedEventArgs> ValidateXmlByteArrayAgainstSchemaAsync(byte[] array, XmlSchemaSet schemaSet, bool reportErrorListAtTheEndOfValidation, CancellationToken cancellationToken);
        Task<ValidationFinishedEventArgs> ValidateXmlFileAgainstSchemaAsync(string filePath, XmlSchemaSet schemaSet, bool reportErrorListAtTheEndOfValidation);
        Task<ValidationFinishedEventArgs> ValidateXmlFileAgainstSchemaAsync(string filePath, XmlSchemaSet schemaSet, bool reportErrorListAtTheEndOfValidation, CancellationToken cancellationToken);
        Task<ValidationFinishedEventArgs> ValidateXmlStreamAgainstSchemaAsync(Stream stream, XmlSchemaSet schemaSet, bool reportErrorListAtTheEndOfValidation);
        Task<ValidationFinishedEventArgs> ValidateXmlStreamAgainstSchemaAsync(Stream stream, XmlSchemaSet schemaSet, bool reportErrorListAtTheEndOfValidation, CancellationToken cancellationToken);
    }
}
