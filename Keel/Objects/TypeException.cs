namespace Keel.Objects
{
    using System;

    public class TypeException : Exception
    {
        public TypeException(string msg)
            : base(msg)
        { }
    }
}