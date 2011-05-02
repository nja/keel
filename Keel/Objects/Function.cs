using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keel.Objects
{
    public class FunctionException : Exception
    {
        public FunctionException(string msg)
            : base(msg)
        { }
    }

    public abstract class Function : LispObject
    {
        private readonly LispObject args;
        private readonly Cons body;

        public Function(LispObject args, Cons body)
        {
            this.args = args;
            this.body = body;
        }

        public abstract LispObject Apply(Cons arguments, LispEnvironment env);

        public LispObject Arguments
        {
            get { return args; }
        }
    }
}
