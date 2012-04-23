namespace Keel.Objects
{
    using System;

    public class EnvironmentException : Exception
    {
        #region Constructors and Destructors

        public EnvironmentException(string msg)
            : base(msg)
        { }

        public EnvironmentException(string msg, Exception inner)
            : base(msg, inner)
        { }

        #endregion
    }
}