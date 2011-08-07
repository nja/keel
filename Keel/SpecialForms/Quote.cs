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
        public Quote()
            : base("QUOTE")
        { }

        public override LispObject Eval(Cons body, LispEnvironment env)
        {
            return Car.Of(body);
        }
    }
}
