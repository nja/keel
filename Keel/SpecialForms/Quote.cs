using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;
using Keel.Builtins;

namespace Keel.SpecialForms
{
    public class Quote : SpecialForm
    {
        public static readonly Quote Instance = new Quote();

        private Quote()
            : base("QUOTE")
        { }

        public override LispObject Eval(Cons body, LispEnvironment env)
        {
            return Car.Of(Cdr.Of(body));
        }

        public static Cons Wrap(LispObject x)
        {
            return Cons.ToList(new LispObject[] { Quote.Instance.Symbol, x });
        }
    }
}
