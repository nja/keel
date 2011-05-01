using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel.Builtins
{
    public class Car : Builtin
    {
        public static readonly Car Instance = new Car();

        private Car()
            : base("CAR", "X")
        { }

        protected override LispObject Apply(LispEnvironment env, LispObject[] args)
        {
            return Car.Of(args[0]);
        }

        public static LispObject Of(LispObject x)
        {
            return x.As<Cons>().Car;
        }
    }
}
