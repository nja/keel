using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;
using Keel.Builtins;

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
            var symbol = Car.Of(body).As<Symbol>();
            var value = env.Eval(Car.Of(Cdr.Of(body)));

            env.AddBinding(symbol, value);

            return value;
        }
    }
}
