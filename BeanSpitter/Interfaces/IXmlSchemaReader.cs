namespace BeanSpitter.Interfaces
{
    using System.IO;
    using System.Xml.Schema;

    public interface IXmlSchemaReader
    {
        /// <summary>
        /// Returns a XmlSchema object from a byte array.
        /// </summary>
        /// <param name="array">Byte array containing the Xml Schema Definition.</param>
        /// <returns>The Xml Schema Definition from the byte array.</returns>
        XmlSchema ReadFromByteArray(byte[] array);

        /// <summary>
        /// Returns a XmlSchema object from a file path.
        /// </summary>
        /// <param name="path">Path of a file containing the Xml Schema Definition.</param>
        /// <returns>The Xml Schema Definition from the specified file.</returns>
        XmlSchema ReadFromPath(string path);

        /// <summary>
        /// Returns a XmlSchema object from a Stream object.
        /// </summary>
        /// <param name="stream">Stream containing the Xml Schema Definition.</param>
        /// <returns>The Xml Schema Definition from the specified stream.</returns>
        XmlSchema ReadFromStream(Stream stream);
    }
}
