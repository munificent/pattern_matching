using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

// An F#-like pattern-matching system in C#, by Bob Nystrom.

namespace PatternMatching
{
    /// <summary>
    /// Exception thrown when trying to get the result of a pattern match block where no cases matched.
    /// </summary>
    public class NoMatchException : Exception
    {
        public NoMatchException() : base() { }
        public NoMatchException(string message) : base(message) { }
        public NoMatchException(string message, Exception innerException) : base(message, innerException) { }
        public NoMatchException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
