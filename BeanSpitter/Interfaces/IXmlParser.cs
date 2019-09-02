namespace BeanSpitter.Interfaces
{
    using BeanSpitter.Models;
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Schema;

    /// <summary>
    /// IXmlParser: Interface for the XmlParserClass
    /// </summary>
    public interface IXmlParser
    {
        /// <summary>
        /// Event that is raised by the XmlParser when a header is read.
        /// </summary>
        event Func<object, HeaderReadEventArgs, CancellationToken, Task> HeaderRead;
        /// <summary>
        /// Event that is raised by the XmlParser when a node is read.
        /// </summary>
        event Func<object, NodeReadEventArgs, CancellationToken, Task> NodeRead;
        /// <summary>
        /// Event that is raised by the XmlParser when an error occurs during the initial XML stream validation or during XML Parsing.
        /// </summary>
        event Func<object, ValidationErrorEventArgs, CancellationToken, Task> ErrorOccurred;
        /// <summary>
        /// Event that is raised by the XmlParser when the validation or the parsing process finish.
        /// </summary>
        event Func<object, ValidationFinishedEventArgs, CancellationToken, Task> ValidationFinished;


        void OnErrorOccurred(object sender, ValidationErrorEventArgs e, CancellationToken cancellationToken);
        void OnFinishedValidating(object sender, ValidationFinishedEventArgs e, CancellationToken cancellationToken);
        void OnHeaderRead(object sender, HeaderReadEventArgs e, CancellationToken cancellationToken);
        void OnNodeRead(object sender, NodeReadEventArgs e, CancellationToken cancellationToken);

        /// <summary>
        /// Parses an XML file from a byte array asynchronously.
        /// </summary>
        /// <param name="array">Byte array containing the XML file to be parsed.</param>
        /// <param name="schemaSet">The XmlSchemaSet containing the XML Schema Definitions (XSD) for the file that needs to be parsed.</param>
        /// <param name="returnErrorListAtTheEndOfTheProcess">Flag indicating if the parser should provide a detailed error list by the end of the process. If true, the "Errors" property will contain a list with all the errors and the error count. If false it will return only the error count.</param>
        /// <param name="types">An array of Type objects from the XSD class that will be identified and parsed from the XML file. <see cref="Type"/></param>
        /// <returns>ValidationFinishedEventArgs <see cref="ValidationFinishedEventArgs"/></returns>
        Task<ValidationFinishedEventArgs> ParseXmlFileFromByteArrayAsync(byte[] array, XmlSchemaSet schemaSet, bool returnErrorListAtTheEndOfTheProcess, params Type[] types);

        /// <summary>
        /// Parses an XML file from a byte array asynchronously.
        /// </summary>
        /// <param name="array">Byte array containing the XML file to be parsed.</param>
        /// <param name="schemaSet">The XmlSchemaSet containing the XML Schema Definitions (XSD) for the file that needs to be parsed.</param>
        /// <param name="headerType">The Type of the Header for the given file. If a file has a header type, all nodes will be returned with a header of this type. <see cref="Type"/></param>
        /// <param name="returnErrorListAtTheEndOfTheProcess">Flag indicating if the parser should provide a detailed error list by the end of the process. If true, the "Errors" property will contain a list with all the errors and the error count. If false it will return only the error count.</param>
        /// <param name="types">An array of Type objects from the XSD class that will be identified and parsed from the XML file. <see cref="Type"/></param>
        /// <returns>ValidationFinishedEventArgs <see cref="ValidationFinishedEventArgs"/></returns>
        Task<ValidationFinishedEventArgs> ParseXmlFileFromByteArrayAsync(byte[] array, XmlSchemaSet schemaSet, Type headerType, bool returnErrorListAtTheEndOfTheProcess, params Type[] types);
        /// <summary>
        /// Parses an XML file asynchronously.
        /// </summary>
        /// <param name="filePath">Path of the XML file to be parsed</param>
        /// <param name="schemaSet">The XmlSchemaSet containing the XML Schema Definitions (XSD) for the file that needs to be parsed.</param>
        /// <param name="returnErrorListAtTheEndOfTheProcess">Flag indicating if the parser should provide a detailed error list by the end of the process. If true, the "Errors" property will contain a list with all the errors and the error count. If false it will return only the error count.</param>
        /// <param name="types">An array of Type objects from the XSD class that will be identified and parsed from the XML file. <see cref="Type"/></param>
        /// <returns>ValidationFinishedEventArgs <see cref="ValidationFinishedEventArgs"/></returns>
        Task<ValidationFinishedEventArgs> ParseXmlFileFromFileAsync(string filePath, XmlSchemaSet schemaSet, bool returnErrorListAtTheEndOfTheProcess, params Type[] types);
        /// <summary>
        /// Parses an XML file asynchronously.
        /// </summary>
        /// <param name="filePath">Path of the XML file to be parsed</param>
        /// <param name="schemaSet">The XmlSchemaSet containing the XML Schema Definitions (XSD) for the file that needs to be parsed.</param>
        /// <param name="headerType">The Type of the Header for the given file. If a file has a header type, all nodes will be returned with a header of this type. <see cref="Type"/></param>
        /// <param name="returnErrorListAtTheEndOfTheProcess">Flag indicating if the parser should provide a detailed error list by the end of the process. If true, the "Errors" property will contain a list with all the errors and the error count. If false it will return only the error count.</param>
        /// <param name="types">An array of Type objects from the XSD class that will be identified and parsed from the XML file. <see cref="Type"/></param>
        /// <returns>ValidationFinishedEventArgs <see cref="ValidationFinishedEventArgs"/></returns>
        Task<ValidationFinishedEventArgs> ParseXmlFileFromFileAsync(string filePath, XmlSchemaSet schemaSet, Type headerType, bool returnErrorListAtTheEndOfTheProcess, params Type[] types);
        /// <summary>
        /// Parses an XML file from a stream asynchronously
        /// </summary>
        /// <param name="stream">A stream containing an XML file.<see cref="Stream"/></param>
        /// <param name="schemaSet">The XmlSchemaSet containing the XML Schema Definitions (XSD) for the file that needs to be parsed.</param>
        /// <param name="returnErrorListAtTheEndOfTheProcess">Flag indicating if the parser should provide a detailed error list by the end of the process. If true, the "Errors" property will contain a list with all the errors and the error count. If false it will return only the error count.</param>
        /// <param name="types">An array of Type objects from the XSD class that will be identified and parsed from the XML file. <see cref="Type"/></param>
        /// <returns>ValidationFinishedEventArgs <see cref="ValidationFinishedEventArgs"/></returns>
        Task<ValidationFinishedEventArgs> ParseXmlFileFromStreamAsync(Stream stream, XmlSchemaSet schemaSet, bool returnErrorListAtTheEndOfTheProcess, params Type[] types);
        /// <summary>
        /// Parses an XML file from a stream asynchronously
        /// </summary>
        /// <param name="stream">A stream containing an XML file.<see cref="Stream"/></param>
        /// <param name="schemaSet">The XmlSchemaSet containing the XML Schema Definitions (XSD) for the file that needs to be parsed.</param>
        /// <param name="headerType">The Type of the Header for the given file. If a file has a header type, all nodes will be returned with a header of this type. <see cref="Type"/></param>
        /// <param name="returnErrorListAtTheEndOfTheProcess">Flag indicating if the parser should provide a detailed error list by the end of the process. If true, the "Errors" property will contain a list with all the errors and the error count. If false it will return only the error count.</param>
        /// <param name="types">An array of Type objects from the XSD class that will be identified and parsed from the XML file. <see cref="Type"/></param>
        /// <returns>ValidationFinishedEventArgs <see cref="ValidationFinishedEventArgs"/></returns>
        Task<ValidationFinishedEventArgs> ParseXmlFileFromStreamAsync(Stream stream, XmlSchemaSet schemaSet, Type headerType, bool returnErrorListAtTheEndOfTheProcess, params Type[] types);
        /// <summary>
        /// Parses an XML file from a byte array asynchronously.
        /// </summary>
        /// <param name="array">Byte array containing the XML file to be parsed.</param>
        /// <param name="schemaSet">The XmlSchemaSet containing the XML Schema Definitions (XSD) for the file that needs to be parsed.</param>
        /// <param name="returnErrorListAtTheEndOfTheProcess">Flag indicating if the parser should provide a detailed error list by the end of the process. If true, the "Errors" property will contain a list with all the errors and the error count. If false it will return only the error count.</param>
        /// <param name="cancellationToken">The cancellation token that will be used by the parser and also be passed along to the event handlers. <see cref="CancellationToken"/></param>
        /// <param name="types">An array of Type objects from the XSD class that will be identified and parsed from the XML file. <see cref="Type"/></param>
        /// <returns>ValidationFinishedEventArgs <see cref="ValidationFinishedEventArgs"/></returns>
        Task<ValidationFinishedEventArgs> ParseXmlFileFromByteArrayAsync(byte[] array, XmlSchemaSet schemaSet, bool returnErrorListAtTheEndOfTheProcess, CancellationToken cancellationToken, params Type[] types);
        /// <summary>
        /// Parses an XML file from a byte array asynchronously.
        /// </summary>
        /// <param name="array">Byte array containing the XML file to be parsed.</param>
        /// <param name="schemaSet">The XmlSchemaSet containing the XML Schema Definitions (XSD) for the file that needs to be parsed.</param>
        /// <param name="headerType">The Type of the Header for the given file. If a file has a header type, all nodes will be returned with a header of this type. <see cref="Type"/></param>
        /// <param name="returnErrorListAtTheEndOfTheProcess">Flag indicating if the parser should provide a detailed error list by the end of the process. If true, the "Errors" property will contain a list with all the errors and the error count. If false it will return only the error count.</param>
        /// <param name="cancellationToken">The cancellation token that will be used by the parser and also be passed along to the event handlers. <see cref="CancellationToken"/></param>
        /// <param name="types">An array of Type objects from the XSD class that will be identified and parsed from the XML file. <see cref="Type"/></param>
        /// <returns>ValidationFinishedEventArgs <see cref="ValidationFinishedEventArgs"/></returns>
        Task<ValidationFinishedEventArgs> ParseXmlFileFromByteArrayAsync(byte[] array, XmlSchemaSet schemaSet, Type headerType, bool returnErrorListAtTheEndOfTheProcess, CancellationToken cancellationToken, params Type[] types);
        /// <summary>
        /// Parses an XML file asynchronously.
        /// </summary>
        /// <param name="filePath">Path of the XML file to be parsed</param>
        /// <param name="schemaSet">The XmlSchemaSet containing the XML Schema Definitions (XSD) for the file that needs to be parsed.</param>
        /// <param name="returnErrorListAtTheEndOfTheProcess">Flag indicating if the parser should provide a detailed error list by the end of the process. If true, the "Errors" property will contain a list with all the errors and the error count. If false it will return only the error count.</param>
        /// <param name="cancellationToken">The cancellation token that will be used by the parser and also be passed along to the event handlers. <see cref="CancellationToken"/></param>
        /// <param name="types">An array of Type objects from the XSD class that will be identified and parsed from the XML file. <see cref="Type"/></param>
        /// <returns>ValidationFinishedEventArgs <see cref="ValidationFinishedEventArgs"/></returns>
        Task<ValidationFinishedEventArgs> ParseXmlFileFromFileAsync(string filePath, XmlSchemaSet schemaSet, bool returnErrorListAtTheEndOfTheProcess, CancellationToken cancellationToken, params Type[] types);
        /// <summary>
        /// Parses an XML file asynchronously.
        /// </summary>
        /// <param name="filePath">Path of the XML file to be parsed</param>
        /// <param name="schemaSet">The XmlSchemaSet containing the XML Schema Definitions (XSD) for the file that needs to be parsed.</param>
        /// <param name="headerType">The Type of the Header for the given file. If a file has a header type, all nodes will be returned with a header of this type. <see cref="Type"/></param>
        /// <param name="returnErrorListAtTheEndOfTheProcess">Flag indicating if the parser should provide a detailed error list by the end of the process. If true, the "Errors" property will contain a list with all the errors and the error count. If false it will return only the error count.</param>
        /// <param name="cancellationToken">The cancellation token that will be used by the parser and also be passed along to the event handlers. <see cref="CancellationToken"/></param>
        /// <param name="types">An array of Type objects from the XSD class that will be identified and parsed from the XML file. <see cref="Type"/></param>
        /// <returns>ValidationFinishedEventArgs <see cref="ValidationFinishedEventArgs"/></returns>
        Task<ValidationFinishedEventArgs> ParseXmlFileFromFileAsync(string filePath, XmlSchemaSet schemaSet, Type headerType, bool returnErrorListAtTheEndOfTheProcess, CancellationToken cancellationToken, params Type[] types);
        /// <summary>
        /// Parses an XML file from a stream asynchronously
        /// </summary>
        /// <param name="stream">A stream containing an XML file.<see cref="Stream"/></param>
        /// <param name="schemaSet">The XmlSchemaSet containing the XML Schema Definitions (XSD) for the file that needs to be parsed.</param>
        /// <param name="returnErrorListAtTheEndOfTheProcess">Flag indicating if the parser should provide a detailed error list by the end of the process. If true, the "Errors" property will contain a list with all the errors and the error count. If false it will return only the error count.</param>
        /// <param name="cancellationToken">The cancellation token that will be used by the parser and also be passed along to the event handlers. <see cref="CancellationToken"/></param>
        /// <param name="types">An array of Type objects from the XSD class that will be identified and parsed from the XML file. <see cref="Type"/></param>
        /// <returns>ValidationFinishedEventArgs <see cref="ValidationFinishedEventArgs"/></returns>
        Task<ValidationFinishedEventArgs> ParseXmlFileFromStreamAsync(Stream stream, XmlSchemaSet schemaSet, bool returnErrorListAtTheEndOfTheProcess, CancellationToken cancellationToken, params Type[] types);
        /// <summary>
        /// Parses an XML file from a stream asynchronously
        /// </summary>
        /// <param name="stream">A stream containing an XML file.<see cref="Stream"/></param>
        /// <param name="schemaSet">The XmlSchemaSet containing the XML Schema Definitions (XSD) for the file that needs to be parsed.</param>
        /// <param name="headerType">The Type of the Header for the given file. If a file has a header type, all nodes will be returned with a header of this type. <see cref="Type"/></param>
        /// <param name="returnErrorListAtTheEndOfTheProcess">Flag indicating if the parser should provide a detailed error list by the end of the process. If true, the "Errors" property will contain a list with all the errors and the error count. If false it will return only the error count.</param>
        /// <param name="cancellationToken">The cancellation token that will be used by the parser and also be passed along to the event handlers. <see cref="CancellationToken"/></param>
        /// <param name="types">An array of Type objects from the XSD class that will be identified and parsed from the XML file. <see cref="Type"/></param>
        /// <returns>ValidationFinishedEventArgs <see cref="ValidationFinishedEventArgs"/></returns>
        Task<ValidationFinishedEventArgs> ParseXmlFileFromStreamAsync(Stream stream, XmlSchemaSet schemaSet, Type headerType, bool returnErrorListAtTheEndOfTheProcess, CancellationToken cancellationToken, params Type[] types);
    }
}
