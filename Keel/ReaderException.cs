namespace Keel
{
    using System;

    public class ReaderException : Exception
    {
        public ReaderException(string msg)
            : base(msg)
        { }

        public ReaderException(string msg, Exception inner)
            : base(msg, inner)
        { }
    }
}