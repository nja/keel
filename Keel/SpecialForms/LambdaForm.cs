using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel.SpecialForms
{
    public class LambdaForm : SpecialForm
    {
        public static LambdaForm Instance = new LambdaForm();

        private LambdaForm()
            : base("LAMBDA")
        { }

        public override LispObject Eval(Cons body, LispEnvironment env)
        {
            var lambdaList = body.Car.As<Cons>();
            var progn = Progn.Wrap(body);

            return new Lambda(lambdaList, progn);
        }
    }
}
