using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel.Builtins
{
    public class EvalBuiltin : Builtin
    {
        public EvalBuiltin()
            : base("EVAL", "FORM")
        { }

        public override LispObject Apply(Cons arguments, LispEnvironment env)
        {
            return env.Eval(arguments.Car);
        }
    }
}
