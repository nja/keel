using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel.Builtins
{
    public class Consp : Builtin
    {
        public Consp()
            : base("CONSP", "X")
        { }

        public override LispObject Apply(Cons arguments, LispEnvironment env)
        {
            if (arguments.Car.IsCons)
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
