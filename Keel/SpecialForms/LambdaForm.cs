using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel.SpecialForms
{
    public class LambdaForm : SpecialForm
    {
        public LambdaForm()
            : base("LAMBDA")
        { }

        public override LispObject Eval(Cons body, LispEnvironment env)
        {
            var lambdaList = body.Car.As<Cons>();
            var progn = Progn.Wrap(body.Cdr.As<Cons>(), env);

            return new Lambda(lambdaList, progn, env);
        }
    }
}
