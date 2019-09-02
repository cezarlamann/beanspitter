namespace BeanSpitter
{
    using BeanSpitter.Interfaces;
    using BeanSpitter.Models;
    using BeanSpitter.Utils;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Abstractions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Schema;

    public partial class XmlValidator : IXmlValidator
    {
        private readonly IEventRiserClass<ValidationErrorEventArgs> validationErrorEventRiser;
        private readonly IEventRiserClass<ValidationFinishedEventArgs> validationFinishedEventRiser;
        private readonly IFileSystem fileSystem;
        private readonly IMemoryStreamFactory memoryStreamFactory;
        private readonly IStreamValidator streamValidator;
        private readonly IXmlReaderSettingsFactory xmlReaderSettingsFactory;
        private readonly IXmlValidationUtils xmlValidationUtils;

        public XmlValidator(
            IMemoryStreamFactory memoryStreamFactory = null,
            IFileSystem fileSystem = null,
            IXmlReaderSettingsFactory xmlReaderSettingsFactory = null,
            IEventRiserClass<ValidationErrorEventArgs> validationErrorEventRiser = null,
            IEventRiserClass<ValidationFinishedEventArgs> validationFinishedEventRiser = null,
            IStreamValidator streamValidator = null,
            IXmlValidationUtils xmlValidationUtils = null)
        {
            this.memoryStreamFactory = memoryStreamFactory ?? new MemoryStreamFactory();
            this.fileSystem = fileSystem ?? new FileSystem();
            this.xmlReaderSettingsFactory = xmlReaderSettingsFactory ?? new XmlReaderSettingsFactory();
            this.validationErrorEventRiser = validationErrorEventRiser ?? new EventRiser<ValidationErrorEventArgs>();
            this.validationFinishedEventRiser = validationFinishedEventRiser ?? new EventRiser<ValidationFinishedEventArgs>();
            this.streamValidator = streamValidator ?? new StreamUtils();
            this.xmlValidationUtils = xmlValidationUtils ?? new XmlValidationUtils();
        }

        public XmlValidator()
        {
            memoryStreamFactory = new MemoryStreamFactory();
            fileSystem = new FileSystem();
            xmlReaderSettingsFactory = new XmlReaderSettingsFactory();
            validationErrorEventRiser = new EventRiser<ValidationErrorEventArgs>();
            validationFinishedEventRiser = new EventRiser<ValidationFinishedEventArgs>();
            streamValidator = new StreamUtils();
            xmlValidationUtils = new XmlValidationUtils();
        }

        public event Func<object, ValidationErrorEventArgs, CancellationToken, Task> ErrorOccurred;
        public event Func<object, ValidationFinishedEventArgs, CancellationToken, Task> ValidationFinished;

        public void OnErrorOccurred(object sender, ValidationErrorEventArgs e, CancellationToken cancellationToken)
        {
            validationErrorEventRiser.RaiseEventsOnThreadPool(ErrorOccurred, e, cancellationToken);
        }

        public void OnFinishedValidating(object sender, ValidationFinishedEventArgs e, CancellationToken cancellationToken)
        {
            validationFinishedEventRiser.RaiseEventsOnThreadPool(ValidationFinished, e, cancellationToken);
        }

        public async Task<ValidationFinishedEventArgs> ValidateXmlStreamAgainstSchemaAsync(Stream stream, XmlSchemaSet schemaSet, bool reportErrorListAtTheEndOfValidation, CancellationToken cancellationToken)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                xmlValidationUtils.ValidateXmlSchemaSet(schemaSet);
            }
            catch (Exception e)
            {
                stopWatch.Stop();
                return new ValidationFinishedEventArgs
                {
                    ElapsedTime = stopWatch.Elapsed,
                    ErrorCount = 1,
                    Errors = new List<ValidationErrorEventArgs>
                    {
                        new ValidationErrorEventArgs(new XmlSchemaException("The XML Schema Set is not valid. See inner exception for more details.", e))
                    }
                };
            }

            if (!streamValidator.StreamIsValid(stream))
            {
                stopWatch.Stop();
                return new ValidationFinishedEventArgs
                {
                    ElapsedTime = stopWatch.Elapsed,
                    ErrorCount = 1,
                    Errors = new List<ValidationErrorEventArgs>
                    {
                        new ValidationErrorEventArgs(new Exception("The XML Stream is not valid. It is either closed or could not be seeked."))
                    }
                };
            }

            stream.Seek(0, SeekOrigin.Begin);

            var errorList = new List<ValidationErrorEventArgs>();
            var errorCount = 0;

            var readerSettings = xmlReaderSettingsFactory.CreateXmlSettings();
            readerSettings.Async = true;

            readerSettings.ValidationEventHandler += (s, e) =>
            {
                errorCount++;
                OnErrorOccurred(s, new ValidationErrorEventArgs(e), cancellationToken);
            };


            if (reportErrorListAtTheEndOfValidation)
            {
                readerSettings.ValidationEventHandler += (s, e) =>
                {
                    errorList.Add(new ValidationErrorEventArgs(e));
                };
            }

            if (!schemaSet.IsCompiled)
            {
                try
                {
                    schemaSet.Compile();
                }
                catch (Exception e)
                {
                    errorCount++;
                    errorList.Add(new ValidationErrorEventArgs(e));

                    return new ValidationFinishedEventArgs
                    {
                        ElapsedTime = stopWatch.Elapsed,
                        ErrorCount = errorCount,
                        Errors = errorList
                    };
                }
            }

            readerSettings.Schemas = schemaSet;

            try
            {
                using (var reader = XmlReader.Create(stream, readerSettings))
                {
                    while (await reader.ReadAsync())
                    {
                        // Do nothing. Just read and let the reader validate things.
                    }
                }
            }
            catch (Exception e)
            {
                errorCount++;
                var exc = new XmlSchemaException(e.Message, e);

                if (reportErrorListAtTheEndOfValidation)
                {
                    errorList.Add(new ValidationErrorEventArgs(exc));
                }

                OnErrorOccurred(this, new ValidationErrorEventArgs(exc, XmlSeverityType.Error), cancellationToken);
            }
            finally
            {
                stopWatch.Stop();
            }

            var result = new ValidationFinishedEventArgs { ElapsedTime = stopWatch.Elapsed, ErrorCount = errorCount, Errors = errorList };

            OnFinishedValidating(this, result, cancellationToken);

            return result;
        }
    }
}
