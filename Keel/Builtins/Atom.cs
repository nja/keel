using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel.Builtins
{
    public class Atom : Builtin
    {
        public Atom()
            : base("ATOM", "X")
        { }

        public override LispObject Apply(Cons arguments, LispEnvironment env)
        {
            if (arguments.Car.IsAtom)
            {
                return T.True;
            }
            else
            {
                return LispNull.Nil;
            }
        }
    }
}
