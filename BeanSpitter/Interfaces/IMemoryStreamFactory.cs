namespace BeanSpitter.Interfaces
{
    using System.IO;

    /// <summary>
    /// Wrapper interface to create MemoryStreams for better testability
    /// </summary>
    public interface IMemoryStreamFactory
    {
        MemoryStream Create(byte[] buffer);
        MemoryStream Create(int capacity);
        MemoryStream Create(byte[] buffer, bool writable);
        MemoryStream Create(byte[] buffer, int index, int count);
        MemoryStream Create(byte[] buffer, int index, int count, bool writable);
        MemoryStream Create(byte[] buffer, int index, int count, bool writable, bool publiclyVisible);
    }
}
