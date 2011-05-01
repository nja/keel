using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel.Builtins
{
    public class Eq : Builtin
    {
        public static readonly Eq Instance = new Eq();

        private Eq()
            : base("EQ", "X", "Y")
        { }

        protected override LispObject Apply(LispEnvironment env, LispObject[] args)
        {
            var x = args[0];
            var y = args[1];

            if (x == y)
            {
                return DefaultSymbols.T;
            }
            else if (x is LispInteger && y is LispInteger
                && ((LispInteger)x).Value == ((LispInteger)y).Value)
            {
                return DefaultSymbols.T;
            }
            else
            {
                return LispNull.Nil;
            }
        }
    }
}
