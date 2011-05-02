using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel.Builtins
{
    public class Add : Builtin
    {
        public static readonly Add Instance = new Add();

        private Add()
            : base("+")
        { }

        public override LispObject Apply(Cons argumentsValues, LispEnvironment env)
        {
            int sum = 0;

            foreach (var addend in argumentsValues)
            {
                var integer = addend.As<LispInteger>();
                sum += integer.Value;
            }

            return new LispInteger(sum);
        }
    }
}
