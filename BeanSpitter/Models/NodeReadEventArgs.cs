namespace BeanSpitter.Models
{
    using System;

    public class NodeReadEventArgs : EventArgs
    {
        public object Header { get; set; }
        public object Node { get; private set; }

        public NodeReadEventArgs(object header, object node)
        {
            Header = header;
            Node = node;
        }
    }
}
