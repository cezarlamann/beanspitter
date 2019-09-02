namespace BeanSpitter.Models
{
    using System;
    using System.Collections.Generic;

    public class ValidationFinishedEventArgs : EventArgs
    {

        public ValidationFinishedEventArgs()
        {
            Errors = new List<ValidationErrorEventArgs>();
            ElapsedTime = new TimeSpan();
        }
        public TimeSpan ElapsedTime { get; set; }
        public ICollection<ValidationErrorEventArgs> Errors { get; set; }
        public long ErrorCount { get; set; }
        public long ParsedNodeCount { get; set; }
    }
}
