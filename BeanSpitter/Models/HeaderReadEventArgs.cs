namespace BeanSpitter.Models
{
    using System;
    public class HeaderReadEventArgs : EventArgs
    {
        public object Header { get; set; }
        public HeaderReadEventArgs(object header)
        {
            Header = header;
        }
    }
}
