using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;
using Keel.Builtins;

namespace Keel.SpecialForms
{
    public class SpecialFormException : Exception
    {
        public SpecialFormException(string msg)
            : base(msg)
        { }
    }

    public abstract class SpecialForm
    {
        public readonly Symbol Symbol;
        private static Dictionary<Symbol, SpecialForm> specials = new Dictionary<Symbol, SpecialForm>();
        
        protected SpecialForm(Symbol symbol)
        {
            this.Symbol = symbol;
            specials.Add(symbol, this);
        }

        protected SpecialForm(string symbolName)
            : this(new Symbol(Symbol.Canonicalize(symbolName)))
        { }

        public static bool IsSpecial(Cons form)
        {
            var car = Car.Of(form);
            var cdr = Cdr.Of(form);
            
            return car is Symbol 
                && cdr is Cons
                && specials.ContainsKey((Symbol)car);
        }

        public static LispObject EvalForm(Cons form, LispEnvironment env)
        {
            if (!IsSpecial(form))
            {
                throw new SpecialFormException(form + " is not a special form");
            }

            var specialSymbol = (Symbol)Car.Of(form);
            var special = specials[specialSymbol];
            var body = (Cons)Cdr.Of(form);

            return special.Eval(body, env);
        }

        public abstract LispObject Eval(Cons body, LispEnvironment env);
    }
}
