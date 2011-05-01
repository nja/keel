using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel.SpecialForms
{
    public class Progn : SpecialForm
    {
        public static Progn Instance = new Progn();

        private Progn()
            : base("PROGN")
        { }

        public override LispObject Eval(Cons body, LispEnvironment env)
        {
            LispObject value;

            do
            {
                value = env.Eval(body.Car);
                body = body.Cdr.As<Cons>();
            }
            while (!body.IsNil);

            return value;
        }

        public static Cons Wrap(Cons body)
        {
            return new Cons(Instance.Symbol, body);
        }
    }
}
