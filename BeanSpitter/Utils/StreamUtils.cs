namespace BeanSpitter.Utils
{
    using BeanSpitter.Interfaces;
    using System.IO;

    public class StreamUtils : IStreamValidator
    {
        public bool StreamIsValid(Stream stream)
        {
            return stream.CanRead && stream.CanSeek;
        }
    }
}
