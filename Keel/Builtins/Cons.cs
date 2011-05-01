using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel.Builtins
{
    public class ConsBuiltin : Builtin
    {
        public static readonly ConsBuiltin Instance = new ConsBuiltin();

        public ConsBuiltin()
            : base("CONS", "X", "Y")
        { }

        protected override LispObject Apply(LispEnvironment env, LispObject[] args)
        {
            return new Cons(args[0], args[1]);
        }
    }
}
