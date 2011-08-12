using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel.Builtins
{
    public class Subtract : Builtin
    {
        public Subtract()
            : base("-")
        { }

        public override LispObject Apply(Cons arguments, LispEnvironment env)
        {
            if (arguments.IsNil)
            {
                throw new EvaluationException("Too few arguments to " + Name);
            }
            else if (arguments.Cdr.IsNil)
            {
                return arguments.Car.As<LispNumber>().Negate();
            }
            else
            {
                var difference = arguments.Car.As<LispNumber>();

                foreach (var subtrahend in arguments.Cdr.As<Cons>())
                {
                    difference = difference.Add(subtrahend.As<LispNumber>().Negate());
                }

                return difference;
            }
        }
    }
}
