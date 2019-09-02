namespace BeanSpitter
{
    using BeanSpitter.Interfaces;
    using System.IO;

    public class MemoryStreamFactory : IMemoryStreamFactory
    {
        public MemoryStream Create(byte[] buffer)
        {
            return new MemoryStream(buffer);
        }

        public MemoryStream Create(int capacity)
        {
            return new MemoryStream(capacity);
        }

        public MemoryStream Create(byte[] buffer, bool writable)
        {
            return new MemoryStream(buffer, writable);
        }

        public MemoryStream Create(byte[] buffer, int index, int count)
        {
            return new MemoryStream(buffer, index, count);
        }

        public MemoryStream Create(byte[] buffer, int index, int count, bool writable)
        {
            return new MemoryStream(buffer, index, count, writable);
        }

        public MemoryStream Create(byte[] buffer, int index, int count, bool writable, bool publiclyVisible)
        {
            return new MemoryStream(buffer, index, count, writable, publiclyVisible);
        }
    }
}
