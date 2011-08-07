using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel.Builtins
{
    public class Eq : Builtin
    {
        public Eq()
            : base("EQ", "X", "Y")
        { }

        public override LispObject Apply(Cons argumentValues, LispEnvironment env)
        {
            var x = Car.Of(argumentValues);
            var y = Car.Of(Cdr.Of(argumentValues));

            if (x == y)
            {
                return T.True;
            }
            else if (x is LispInteger && y is LispInteger
                && ((LispInteger)x).Value == ((LispInteger)y).Value)
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
