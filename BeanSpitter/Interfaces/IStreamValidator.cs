namespace BeanSpitter.Interfaces
{
    using System.IO;

    public interface IStreamValidator
    {
        bool StreamIsValid(Stream stream);
    }
}
