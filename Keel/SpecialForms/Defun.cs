using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;
using Keel.Builtins;

namespace Keel.SpecialForms
{
    public class Defun : SpecialForm
    {
        public Defun()
            : base("DEFUN")
        { }

        public override LispObject Eval(Cons defunBody, LispEnvironment env)
        {
            var symbol = Car.Of(defunBody).As<Symbol>();
            var lambdaList = Car.Of(Cdr.Of(defunBody));
            var lambdaBody = Cdr.Of(Cdr.Of(defunBody)).As<Cons>();
            var progn = Progn.Wrap(lambdaBody, env);

            var lambda = new Lambda(lambdaList, progn, env);

            env.AddBinding(symbol, lambda);

            return lambda;
        }
    }
}
