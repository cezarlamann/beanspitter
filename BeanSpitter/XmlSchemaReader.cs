namespace BeanSpitter
{
    using BeanSpitter.Interfaces;
    using System;
    using System.IO.Abstractions;
    using System.Text;
    using System.Xml.Schema;

    /// <inheritdoc />
    public class XmlSchemaReader : IXmlSchemaReader
    {
        private readonly IFileSystem fileSystem;
        private readonly IMemoryStreamFactory memoryStreamFactory;

        public XmlSchemaReader(IFileSystem fileSystem = null, IMemoryStreamFactory memoryStreamFactory = null)
        {
            this.memoryStreamFactory = memoryStreamFactory ?? new MemoryStreamFactory();
            this.fileSystem = fileSystem ?? new FileSystem();
        }

        public XmlSchemaReader()
        {
            memoryStreamFactory = new MemoryStreamFactory();
            fileSystem = new FileSystem();
        }

        /// <inheritdoc />
        public XmlSchema ReadFromByteArray(byte[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (array.LongLength == 0)
            {
                throw new ArgumentException("Byte array must not be empty.", nameof(array));
            }

            XmlSchema result;

            try
            {
                using (var stream = memoryStreamFactory.Create(array, false))
                {
                    result = ReadFromStream(stream);
                }
            }
            catch (Exception e)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Could not create a memory stream from the specified byte array.");
                sb.AppendLine();
                sb.AppendLine("Exception: ");
                sb.AppendLine(e.Message);
                sb.AppendLine();
                sb.AppendLine("Stacktrace: ");
                sb.AppendLine(e.StackTrace);
                throw new InvalidOperationException(sb.ToString(), e);
            }

            return result;
        }

        /// <inheritdoc />
        public XmlSchema ReadFromPath(string path)
        {
            if (!fileSystem.File.Exists(path))
            {
                throw new ArgumentException("Specified path does not exist.", nameof(path));
            }
            var result = new XmlSchema();

            try
            {
                using (var stream = fileSystem.FileStream.Create(path, System.IO.FileMode.Open))
                {
                    result = ReadFromStream(stream);
                }
            }
            catch (Exception e)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Could not create a file stream from the specified path (\"{path}\").");
                sb.AppendLine();
                sb.AppendLine("Exception: ");
                sb.AppendLine(e.Message);
                sb.AppendLine();
                sb.AppendLine("Stacktrace: ");
                sb.AppendLine(e.StackTrace);
                throw new InvalidOperationException(sb.ToString(), e);
            }

            return result;
        }

        /// <inheritdoc />
        public XmlSchema ReadFromStream(System.IO.Stream stream)
        {


            if (!stream.CanRead)
            {
                throw new ArgumentException("Cannot read from specified stream.", nameof(stream));
            }

            if (stream.Length == 0)
            {
                throw new ArgumentException("The specified stream is empty.", nameof(stream));
            }

            if (stream.Position != 0)
            {
                stream.Seek(0, System.IO.SeekOrigin.Begin);
            }

            XmlSchema result;
            result = XmlSchema.Read(stream, (o, e) =>
            {
                throw new XmlSchemaException(e.Message, e.Exception);
            });

            stream.Close();

            return result;
        }
    }
}
