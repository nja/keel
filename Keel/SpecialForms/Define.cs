using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel.SpecialForms
{
    public class Define : SpecialForm
    {
        public static readonly Define Instance = new Define();

        private Define()
            : base("DEFINE")
        { }

        public override LispObject Eval(Cons body, LispEnvironment env)
        {
            var symbol = body.Car.As<Symbol>();
            var value = env.Eval(body.Cdr);

            env.AddBinding(symbol, value);

            return value;
        }
    }
}
