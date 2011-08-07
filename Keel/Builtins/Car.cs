using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel.Builtins
{
    public class Car : Builtin
    {
        public Car()
            : base("CAR", "X")
        { }

        public override LispObject Apply(Cons argumentValues, LispEnvironment env)
        {
            return Car.Of(argumentValues.Car);
        }

        public static LispObject Of(LispObject x)
        {
            return x.As<Cons>().Car;
        }
    }
}
