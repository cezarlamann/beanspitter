namespace BeanSpitter
{
    using BeanSpitter.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Schema;

    public partial class XmlValidator
    {
        public Task<ValidationFinishedEventArgs> ValidateXmlByteArrayAgainstSchemaAsync(byte[] array, XmlSchemaSet schemaSet, bool reportErrorListAtTheEndOfValidation)
        {
            return ValidateXmlByteArrayAgainstSchemaAsync(array, schemaSet, reportErrorListAtTheEndOfValidation, new CancellationToken(false));
        }

        public async Task<ValidationFinishedEventArgs> ValidateXmlByteArrayAgainstSchemaAsync(byte[] array, XmlSchemaSet schemaSet, bool reportErrorListAtTheEndOfValidation, CancellationToken cancellationToken)
        {
            if (array == null)
            {
                return new ValidationFinishedEventArgs
                {
                    ElapsedTime = TimeSpan.Zero,
                    ErrorCount = 1,
                    Errors = new List<ValidationErrorEventArgs>
                    {
                        new ValidationErrorEventArgs(new Exception("The byte array cannot be null."))
                    }
                };
            }

            if (array.Length == 0)
            {
                return new ValidationFinishedEventArgs
                {
                    ElapsedTime = TimeSpan.Zero,
                    ErrorCount = 1,
                    Errors = new List<ValidationErrorEventArgs>
                    {
                        new ValidationErrorEventArgs(new Exception("The byte array is empty."))
                    }
                };
            }

            using (var stream = memoryStreamFactory.Create(array))
            {
                var res = await ValidateXmlStreamAgainstSchemaAsync(stream, schemaSet, reportErrorListAtTheEndOfValidation, cancellationToken);
                stream.Close();
                return res;
            }
        }

        public Task<ValidationFinishedEventArgs> ValidateXmlFileAgainstSchemaAsync(string path, XmlSchemaSet schemaSet, bool reportErrorListAtTheEndOfValidation)
        {
            return ValidateXmlFileAgainstSchemaAsync(path, schemaSet, reportErrorListAtTheEndOfValidation, new CancellationToken(false));
        }

        public async Task<ValidationFinishedEventArgs> ValidateXmlFileAgainstSchemaAsync(string filePath, XmlSchemaSet schemaSet, bool reportErrorListAtTheEndOfValidation, CancellationToken cancellationToken)
        {
            try
            {
                xmlValidationUtils.ValidateFilePath(filePath, fileSystem);
            }
            catch (Exception e)
            {
                return new ValidationFinishedEventArgs
                {
                    ElapsedTime = TimeSpan.Zero,
                    ErrorCount = 1,
                    Errors = new List<ValidationErrorEventArgs>
                    {
                        new ValidationErrorEventArgs(new Exception($"The given XML file (\"{filePath ?? string.Empty}\") could not be opened. See inner exception for further details.", e))
                    }
                };
            }

            using (var stream = fileSystem.FileStream.Create(filePath, FileMode.Open))
            {
                var res = await ValidateXmlStreamAgainstSchemaAsync(stream, schemaSet, reportErrorListAtTheEndOfValidation, cancellationToken);
                stream.Close();
                return res;
            }
        }

        public Task<ValidationFinishedEventArgs> ValidateXmlStreamAgainstSchemaAsync(Stream stream, XmlSchemaSet schemaSet, bool reportErrorListAtTheEndOfValidation)
        {
            return ValidateXmlStreamAgainstSchemaAsync(stream, schemaSet, reportErrorListAtTheEndOfValidation, new CancellationToken(false));
        }
    }
}
