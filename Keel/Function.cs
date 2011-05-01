using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keel
{
    public class FunctionException : Exception
    {
        public FunctionException(string msg)
            : base(msg)
        { }
    }

    public abstract class Function : LispObject
    {
        protected readonly Symbol[] Args;
        private readonly LispObject body;

        public Function(IEnumerable<Symbol> args, LispObject body)
        {
            this.Args = args.ToArray();
            this.body = body;
        }

        public abstract LispObject Apply(Environment env);

        public IEnumerable<Symbol> Arguments
        {
            get { return Args; }
        }
    }
}
