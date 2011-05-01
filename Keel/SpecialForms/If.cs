using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;
using Keel.Builtins;

namespace Keel.SpecialForms
{
    public class If : SpecialForm
    {
        public static If IfForm = new If();

        private If()
            : base("IF")
        { }

        public override LispObject Eval(Cons body, LispEnvironment env)
        {
            var testForm = Car.Of(body);
            var thenForm = Car.Of(Cdr.Of(body));
            var elseForm = Car.Of(Cdr.Of(Cdr.Of(body)));

            if (env.Eval(testForm).IsTrue)
            {
                return env.Eval(thenForm);
            }
            else
            {
                return env.Eval(elseForm);
            }
        }
    }
}
