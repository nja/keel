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
        private readonly Symbol[] Args;
        private readonly Cons body;

        public Function(IEnumerable<Symbol> args, Cons body)
        {
            this.Args = args.ToArray();
            this.body = body;
        }

        public abstract LispObject Apply(IEnumerable<LispObject> arguments, LispEnvironment env);

        public IEnumerable<Symbol> Arguments
        {
            get { return Args; }
        }
    }
}
