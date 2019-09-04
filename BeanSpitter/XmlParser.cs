using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("BeanSpitter.Tests")]

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
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    /// <inheritdoc />
    public partial class XmlParser : IXmlParser, IDisposable
    {
        private readonly IFileSystem fileSystem;
        private readonly IMemoryStreamFactory memoryStreamFactory;
        private readonly IEventRiserClass<HeaderReadEventArgs> headerReadEventRiser;
        private readonly IEventRiserClass<NodeReadEventArgs> nodeReadEventRiser;
        private readonly IStreamValidator streamValidator;
        private readonly IEventRiserClass<ValidationErrorEventArgs> validationErrorEventRiser;
        private readonly IEventRiserClass<ValidationFinishedEventArgs> validationFinishedEventRiser;
        private readonly IXmlReaderSettingsFactory xmlReaderSettingsFactory;
        private readonly IXmlValidationUtils xmlValidationUtils;

        public XmlParser(
            IMemoryStreamFactory memoryStreamFactory = null,
            IFileSystem fileSystem = null,
            IXmlReaderSettingsFactory xmlReaderSettingsFactory = null,
            IEventRiserClass<HeaderReadEventArgs> headerReadEventRiser = null,
            IEventRiserClass<NodeReadEventArgs> nodeReadEventRiser = null,
            IEventRiserClass<ValidationErrorEventArgs> validationErrorEventRiser = null,
            IEventRiserClass<ValidationFinishedEventArgs> validationFinishedEventRiser = null,
            IStreamValidator streamValidator = null,
            IXmlValidationUtils xmlValidationUtils = null)
        {
            this.memoryStreamFactory = memoryStreamFactory ?? new MemoryStreamFactory();
            this.fileSystem = fileSystem ?? new FileSystem();
            this.xmlReaderSettingsFactory = xmlReaderSettingsFactory ?? new XmlReaderSettingsFactory();
            this.headerReadEventRiser = headerReadEventRiser ?? new EventRiser<HeaderReadEventArgs>();
            this.nodeReadEventRiser = nodeReadEventRiser ?? new EventRiser<NodeReadEventArgs>();
            this.validationErrorEventRiser = validationErrorEventRiser ?? new EventRiser<ValidationErrorEventArgs>();
            this.validationFinishedEventRiser = validationFinishedEventRiser ?? new EventRiser<ValidationFinishedEventArgs>();
            this.streamValidator = streamValidator ?? new StreamUtils();
            this.xmlValidationUtils = xmlValidationUtils ?? new XmlValidationUtils();
        }

        public XmlParser()
        {
            memoryStreamFactory = new MemoryStreamFactory();
            fileSystem = new FileSystem();
            xmlReaderSettingsFactory = new XmlReaderSettingsFactory();
            headerReadEventRiser = new EventRiser<HeaderReadEventArgs>();
            nodeReadEventRiser = new EventRiser<NodeReadEventArgs>();
            validationErrorEventRiser = new EventRiser<ValidationErrorEventArgs>();
            validationFinishedEventRiser = new EventRiser<ValidationFinishedEventArgs>();
            streamValidator = new StreamUtils();
            xmlValidationUtils = new XmlValidationUtils();
        }

        /// <inheritdoc/>
        public event Func<object, ValidationErrorEventArgs, CancellationToken, Task> ErrorOccurred;
        /// <inheritdoc/>
        public event Func<object, HeaderReadEventArgs, CancellationToken, Task> HeaderRead;
        /// <inheritdoc/>
        public event Func<object, NodeReadEventArgs, CancellationToken, Task> NodeRead;
        /// <inheritdoc/>
        public event Func<object, ValidationFinishedEventArgs, CancellationToken, Task> ValidationFinished;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void OnErrorOccurred(object sender, ValidationErrorEventArgs e, CancellationToken cancellationToken)
        {
            validationErrorEventRiser.RaiseEventsOnThreadPool(ErrorOccurred, e, cancellationToken);
        }

        public void OnFinishedValidating(object sender, ValidationFinishedEventArgs e, CancellationToken cancellationToken)
        {
            validationFinishedEventRiser.RaiseEventsOnThreadPool(ValidationFinished, e, cancellationToken);
        }

        public void OnNodeRead(object sender, NodeReadEventArgs e, CancellationToken cancellationToken)
        {
            nodeReadEventRiser.RaiseEventsOnThreadPool(NodeRead, e, cancellationToken);
        }

        public void OnHeaderRead(object sender, HeaderReadEventArgs e, CancellationToken cancellationToken)
        {
            headerReadEventRiser.RaiseEventsOnThreadPool(HeaderRead, e, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<ValidationFinishedEventArgs> ParseXmlFileFromStreamAsync(Stream stream, XmlSchemaSet schemaSet, Type headerType, bool returnErrorListAtTheEndOfTheProcess, CancellationToken cancellationToken, params Type[] types)
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

            var tagsFromSchemaSet = GetTagsFromSchemaSet(schemaSet);

            var headerTypeName = new KeyValuePair<string, HashSet<string>>();
            object header = null;
            XmlSerializer headerSerializer = null;
            XmlRootAttribute attrHeader = null;

            if (headerType != null)
            {
                header = Activator.CreateInstance(headerType);
                headerTypeName = tagsFromSchemaSet.FirstOrDefault(w => w.Key == headerType.Name);
                attrHeader = ReturnXmlRootAttributeForType(headerTypeName.Value.FirstOrDefault(), headerType);
                headerSerializer = new XmlSerializer(headerType, attrHeader);
            }

            var otherTypeNames = types.Select(s => s.Name).ToArray();
            var otherTypeNamesFromDictionary = tagsFromSchemaSet.Where(w => otherTypeNames.Contains(w.Key)).ToArray();

            try
            {
                CheckTypesForAmbiguity(tagsFromSchemaSet, otherTypeNames, otherTypeNamesFromDictionary);
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
                        new ValidationErrorEventArgs(new Exception("There are ambiguities for the given types. Check inner exception for more details.", e))
                    }
                };
            }

            if (cancellationToken.IsCancellationRequested)
            {
                stopWatch.Stop();
                return new ValidationFinishedEventArgs
                {
                    ElapsedTime = stopWatch.Elapsed,
                    ErrorCount = 1,
                    Errors = new List<ValidationErrorEventArgs>
                    {
                        new ValidationErrorEventArgs(new OperationCanceledException("Operation has been canceled."))
                    }
                };
            }

            var serializerDictionary = CreateSerializerDictionary(types, otherTypeNamesFromDictionary);

            otherTypeNames = null;
            otherTypeNamesFromDictionary = null;


            var errorList = new List<ValidationErrorEventArgs>();
            long errorCount = 0;
            long nodeCount = 0;

            var readerSettings = xmlReaderSettingsFactory.CreateXmlSettings();
            readerSettings.Async = true;

            readerSettings.ValidationEventHandler += (s, e) =>
            {
                errorCount++;
                OnErrorOccurred(s, new ValidationErrorEventArgs(e), cancellationToken);
            };

            if (returnErrorListAtTheEndOfTheProcess)
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
                        Errors = errorList,
                        ParsedNodeCount = nodeCount
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
                        reader.MoveToElement();

                        if (reader.NodeType != XmlNodeType.Element)
                        {
                            continue;
                        }

                        var elementName = reader.SchemaInfo?.SchemaElement?.Name;

                        if (string.IsNullOrEmpty(elementName))
                        {
                            continue;
                        }

                        if (cancellationToken.IsCancellationRequested)
                        {
#if DEBUG
                            Debug.WriteLine("Operation Cancelled - Cancel Parsing");
#endif
                            reader.Close();
                            reader.Dispose();

                            serializerDictionary.Clear();
                            headerSerializer = null;

                            stopWatch.Stop();

                            errorCount++;
                            errorList.Add(new ValidationErrorEventArgs(new OperationCanceledException("Operation has been canceled.")));

                            return new ValidationFinishedEventArgs
                            {
                                ElapsedTime = stopWatch.Elapsed,
                                ErrorCount = errorCount,
                                Errors = errorList,
                                ParsedNodeCount = nodeCount
                            };
                        }

                        if (serializerDictionary.ContainsKey(elementName))
                        {
                            try
                            {
                                var serializer = serializerDictionary[elementName];
                                var obj = serializer.Deserialize(reader.ReadSubtree());
                                nodeCount++;
                                OnNodeRead(this, new NodeReadEventArgs(header, obj), cancellationToken);
                            }
                            catch (Exception e)
                            {
                                errorCount++;
                                var exc = new XmlSchemaException(e.Message, e);

                                if (returnErrorListAtTheEndOfTheProcess)
                                {
                                    errorList.Add(new ValidationErrorEventArgs(exc, XmlSeverityType.Error));
                                }

                                OnErrorOccurred(this, new ValidationErrorEventArgs(exc, XmlSeverityType.Error), cancellationToken);
                            }
                        }
                        else if (headerSerializer != null && (headerTypeName.Value?.Contains(elementName)).GetValueOrDefault(false))
                        {
                            try
                            {
                                header = headerSerializer.Deserialize(reader.ReadSubtree());
                                OnHeaderRead(this, new HeaderReadEventArgs(header), cancellationToken);
                            }
                            catch (Exception e)
                            {
                                errorCount++;
                                var exc = new XmlSchemaException(e.Message, e);

                                if (returnErrorListAtTheEndOfTheProcess)
                                {
                                    errorList.Add(new ValidationErrorEventArgs(exc, XmlSeverityType.Error));
                                }

                                OnErrorOccurred(this, new ValidationErrorEventArgs(exc, XmlSeverityType.Error), cancellationToken);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                errorCount++;
                var exc = new XmlSchemaException(e.Message, e);

                if (returnErrorListAtTheEndOfTheProcess)
                {
                    errorList.Add(new ValidationErrorEventArgs(exc));
                }

                OnErrorOccurred(this, new ValidationErrorEventArgs(exc, XmlSeverityType.Error), cancellationToken);
            }
            finally
            {
                stopWatch.Stop();
            }

            await Task.WhenAll(
                nodeReadEventRiser.WaitForTasksToFinish(cancellationToken),
                headerReadEventRiser.WaitForTasksToFinish(cancellationToken),
                validationErrorEventRiser.WaitForTasksToFinish(cancellationToken),
                validationFinishedEventRiser.WaitForTasksToFinish(cancellationToken));

            var result = new ValidationFinishedEventArgs { ElapsedTime = stopWatch.Elapsed, ErrorCount = errorCount, ParsedNodeCount = nodeCount, Errors = errorList };

            OnFinishedValidating(this, result, cancellationToken);

            return result;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var ErrorOccurredHandler = ErrorOccurred;
                var HeaderReadHandler = HeaderRead;
                var NodeReadHandler = NodeRead;
                var ValidationFinishedHandler = ValidationFinished;

                if (ErrorOccurredHandler != null)
                {
                    var invocationList = ErrorOccurredHandler.GetInvocationList();
                    if (invocationList != null)
                    {
                        foreach (var method in invocationList)
                        {
                            ErrorOccurredHandler -= (method as Func<object, ValidationErrorEventArgs, CancellationToken, Task>);
                        }
                    }
                }

                if (HeaderReadHandler != null)
                {
                    var invocationList = HeaderReadHandler.GetInvocationList();
                    if (invocationList != null)
                    {
                        foreach (var method in invocationList)
                        {
                            HeaderReadHandler -= (method as Func<object, HeaderReadEventArgs, CancellationToken, Task>);
                        }
                    }
                }

                if (NodeReadHandler != null)
                {
                    var invocationList = NodeReadHandler.GetInvocationList();
                    if (invocationList != null)
                    {
                        foreach (var method in invocationList)
                        {
                            NodeReadHandler -= (method as Func<object, NodeReadEventArgs, CancellationToken, Task>);
                        }
                    }
                }

                if (ValidationFinishedHandler != null)
                {
                    var invocationList = NodeReadHandler.GetInvocationList();
                    if (invocationList != null)
                    {
                        foreach (var method in invocationList)
                        {
                            ValidationFinishedHandler -= (method as Func<object, ValidationFinishedEventArgs, CancellationToken, Task>);
                        }
                    }
                }
            }
        }

        private static void CheckTypesForAmbiguity(Dictionary<string, HashSet<string>> tagsFromSchemaList, string[] otherTypeNames, KeyValuePair<string, HashSet<string>>[] otherTypeNamesFromDictionary)
        {
            foreach (var type in otherTypeNames)
            {
                var tagsForTheType = otherTypeNamesFromDictionary.FirstOrDefault(f => f.Key == type);

                foreach (var tag in tagsForTheType.Value)
                {
                    var ambiguitity = tagsFromSchemaList.Where(c => c.Value.Contains(tag));

                    if (ambiguitity.Count() > 1)
                    {
                        var sb = new StringBuilder();
                        sb.AppendLine($"The given type ({type}) is mapped to an ambiguous tag name.");
                        sb.AppendLine($"The \"{tag}\" tag is used to other object types inside the provided schema set.");
                        sb.AppendLine("This tag is the same used by the types below:");
                        foreach (var item in ambiguitity)
                        {
                            sb.AppendLine($"- {item.Key}");
                        }
                        sb.AppendLine();
                        sb.AppendLine("To prevent this from happening, please use \"Higher level type\" or a Wrapping type where the desired object is used.");

                        throw new Exception(sb.ToString());
                    }
                }
            }
        }

        private static Dictionary<string, XmlSerializer> CreateSerializerDictionary(Type[] types, KeyValuePair<string, HashSet<string>>[] otherTypeNamesFromDictionary)
        {
            var serializerDictionary = new Dictionary<string, XmlSerializer>();

            foreach (var item in otherTypeNamesFromDictionary)
            {
                var type = types.FirstOrDefault(f => f.Name == item.Key);

                foreach (var value in item.Value)
                {
                    var attr = ReturnXmlRootAttributeForType(value, type);

                    var serializer = new XmlSerializer(type, attr);
                    if (!serializerDictionary.ContainsKey(value))
                    {
                        serializerDictionary.Add(value, serializer);
                    }
                }
            }

            return serializerDictionary;
        }

        private static Dictionary<string, HashSet<string>> GetTagsFromSchemaSet(XmlSchemaSet schemaSet)
        {
            var schemaList = schemaSet.Schemas().OfType<XmlSchema>();
            var schemaObjects = new List<XmlSchemaObject>();

            foreach (var item in schemaList)
            {
                var objs = item.Items.OfType<XmlSchemaObject>();
                schemaObjects.AddRange(objs);
            }

            var res = schemaObjects.GetTagsByType();
            return res;
        }

        private static string ReturnXmlNamespaceFromType(Type type)
        {
            var res = string.Empty;

            if (type.CustomAttributes == null)
            {
                return res;
            }

            var xmlTypeAttr = type.CustomAttributes.FirstOrDefault(f => f.AttributeType == typeof(XmlTypeAttribute));

            if (xmlTypeAttr == null)
            {
                return res;
            }

            if (xmlTypeAttr.NamedArguments == null)
            {
                return res;
            }

            var nsAttr = xmlTypeAttr.NamedArguments.FirstOrDefault(f => f.MemberName.ToLower() == "namespace");

            if (nsAttr == null)
            {
                return res;
            }

            res = nsAttr.TypedValue.Value?.ToString() ?? string.Empty;

            return res;
        }

        private static XmlRootAttribute ReturnXmlRootAttributeForType(string elementName, Type type)
        {
            if (string.IsNullOrEmpty(elementName))
            {
                throw new ArgumentNullException(nameof(elementName));
            }
            var res = new XmlRootAttribute(elementName)
            {
                Namespace = string.Empty,
                IsNullable = true
            };

            if (type.CustomAttributes == null)
            {
                return res;
            }

            var xmlRootAttr = type.CustomAttributes.FirstOrDefault(f => f.AttributeType == typeof(XmlRootAttribute));

            if (xmlRootAttr?.NamedArguments == null)
            {
                res.Namespace = ReturnXmlNamespaceFromType(type);
                return res;
            }

            var nsAttr = xmlRootAttr.NamedArguments.FirstOrDefault(f => f.MemberName.ToLower() == "namespace");

            if (nsAttr != null)
            {
                if (nsAttr.TypedValue != null)
                {
                    if (nsAttr.TypedValue.Value != null)
                    {
                        res.Namespace = nsAttr.TypedValue.Value.ToString();
                    }
                }
            }

            var isNullableAttr = xmlRootAttr.NamedArguments.FirstOrDefault(f => f.MemberName.ToLower() == "isnullable");

            if (isNullableAttr != null)
            {
                if (isNullableAttr.TypedValue != null)
                {
                    if (isNullableAttr.TypedValue.Value != null)
                    {
                        res.IsNullable = Convert.ToBoolean(isNullableAttr.TypedValue.Value);
                    }
                }
            }

            return res;
        }
    }
}
