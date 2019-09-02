namespace BeanSpitter
{
    using BeanSpitter.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Schema;

    public partial class XmlParser
    {
        /// <inheritdoc />
        public Task<ValidationFinishedEventArgs> ParseXmlFileFromByteArrayAsync(byte[] array, XmlSchemaSet schemaSet, bool returnErrorListAtTheEndOfTheProcess, params Type[] types)
        {
            return ParseXmlFileFromByteArrayAsync(array, schemaSet, null, returnErrorListAtTheEndOfTheProcess, types: types);
        }

        /// <inheritdoc />
        public Task<ValidationFinishedEventArgs> ParseXmlFileFromByteArrayAsync(byte[] array, XmlSchemaSet schemaSet, Type headerType, bool returnErrorListAtTheEndOfTheProcess, params Type[] types)
        {
            return ParseXmlFileFromByteArrayAsync(array, schemaSet, headerType, returnErrorListAtTheEndOfTheProcess, new CancellationToken(false), types: types);
        }

        /// <inheritdoc />
        public Task<ValidationFinishedEventArgs> ParseXmlFileFromFileAsync(string filePath, XmlSchemaSet schemaSet, bool returnErrorListAtTheEndOfTheProcess, params Type[] types)
        {
            return ParseXmlFileFromFileAsync(filePath, schemaSet, null, returnErrorListAtTheEndOfTheProcess, types: types);
        }

        /// <inheritdoc />
        public Task<ValidationFinishedEventArgs> ParseXmlFileFromFileAsync(string filePath, XmlSchemaSet schemaSet, Type headerType, bool returnErrorListAtTheEndOfTheProcess, params Type[] types)
        {
            return ParseXmlFileFromFileAsync(filePath, schemaSet, headerType, returnErrorListAtTheEndOfTheProcess, new CancellationToken(false), types: types);
        }

        /// <inheritdoc />
        public Task<ValidationFinishedEventArgs> ParseXmlFileFromStreamAsync(Stream stream, XmlSchemaSet schemaSet, bool returnErrorListAtTheEndOfTheProcess, params Type[] types)
        {
            return ParseXmlFileFromStreamAsync(stream, schemaSet, null, returnErrorListAtTheEndOfTheProcess, types: types);
        }

        /// <inheritdoc />
        public Task<ValidationFinishedEventArgs> ParseXmlFileFromStreamAsync(Stream stream, XmlSchemaSet schemaSet, Type headerType, bool returnErrorListAtTheEndOfTheProcess, params Type[] types)
        {
            return ParseXmlFileFromStreamAsync(stream, schemaSet, headerType, returnErrorListAtTheEndOfTheProcess, new CancellationToken(false), types);
        }

        /// <inheritdoc />
        public Task<ValidationFinishedEventArgs> ParseXmlFileFromByteArrayAsync(byte[] array, XmlSchemaSet schemaSet, bool returnErrorListAtTheEndOfTheProcess, CancellationToken cancellationToken, params Type[] types)
        {
            return ParseXmlFileFromByteArrayAsync(array, schemaSet, null, returnErrorListAtTheEndOfTheProcess, cancellationToken, types: types);
        }

        /// <inheritdoc />
        public Task<ValidationFinishedEventArgs> ParseXmlFileFromFileAsync(string filePath, XmlSchemaSet schemaSet, bool returnErrorListAtTheEndOfTheProcess, CancellationToken cancellationToken, params Type[] types)
        {
            return ParseXmlFileFromFileAsync(filePath, schemaSet, null, returnErrorListAtTheEndOfTheProcess, cancellationToken, types: types);
        }

        /// <inheritdoc />
        public async Task<ValidationFinishedEventArgs> ParseXmlFileFromByteArrayAsync(byte[] array, XmlSchemaSet schemaSet, Type headerType, bool returnErrorListAtTheEndOfTheProcess, CancellationToken cancellationToken, params Type[] types)
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
                // Had to use await here because the stream was being closed/disposed while the method was still executing.
                var res = await ParseXmlFileFromStreamAsync(stream, schemaSet, headerType, returnErrorListAtTheEndOfTheProcess, cancellationToken, types: types);
                stream.Close();
                return res;
            }
        }

        /// <inheritdoc />
        public async Task<ValidationFinishedEventArgs> ParseXmlFileFromFileAsync(string filePath, XmlSchemaSet schemaSet, Type headerType, bool returnErrorListAtTheEndOfTheProcess, CancellationToken cancellationToken, params Type[] types)
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
                // Had to use await here because the stream was being closed/disposed while the method was still executing.
                var res = await ParseXmlFileFromStreamAsync(stream, schemaSet, headerType, returnErrorListAtTheEndOfTheProcess, cancellationToken, types: types);
                stream.Close();
                return res;
            }
        }

        /// <inheritdoc />
        public Task<ValidationFinishedEventArgs> ParseXmlFileFromStreamAsync(Stream stream, XmlSchemaSet schemaSet, bool returnErrorListAtTheEndOfTheProcess, CancellationToken cancellationToken, params Type[] types)
        {
            return ParseXmlFileFromStreamAsync(stream, schemaSet, null, returnErrorListAtTheEndOfTheProcess, cancellationToken, types);
        }
    }
}
