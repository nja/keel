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

        protected override LispObject Apply(LispEnvironment env, LispObject[] args)
        {
            int sum = 0;

            foreach (var addend in args)
            {
                var integer = addend.As<LispInteger>();
                sum += integer.Value;
            }

            return new LispInteger(sum);
        }
    }
}
