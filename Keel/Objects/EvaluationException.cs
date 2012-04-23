namespace Keel.Objects
{
    using System;

    public class EvaluationException : Exception
    {
        #region Constructors and Destructors

        public EvaluationException(string msg)
            : base(msg)
        { }

        public EvaluationException(string msg, Exception inner)
            : base(msg, inner)
        { }

        #endregion
    }
}