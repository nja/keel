using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;
using Keel.Builtins;
using Keel.SpecialForms;

namespace Keel
{
    public class Specials
    {
        private readonly Dictionary<string, SpecialForm> specials = new Dictionary<string, SpecialForm>();

        public Specials()
        {
            Add(new Define());
            Add(new Defmacro());
            Add(new Defun());
            Add(new If());
            Add(new LambdaForm());
            Add(new Progn());
            Add(new Quote());
            Add(new SetForm());
        }

        private void Add(SpecialForm special)
        {
            specials.Add(special.Name, special);
        }

        public bool IsSpecial(Cons form)
        {
            var car = Car.Of(form);
            var cdr = Cdr.Of(form);

            return car is Symbol
                && cdr is Cons
                && specials.ContainsKey(car.As<Symbol>().Name);
        }

        public LispObject Eval(Cons form, LispEnvironment env)
        {
            if (!IsSpecial(form))
            {
                throw new SpecialFormException(form + " is not a special form");
            }

            var specialSymbol = (Symbol)Car.Of(form);
            var special = specials[specialSymbol.Name];
            var body = (Cons)Cdr.Of(form);

            return special.Eval(body, env);
        }
    }
}
