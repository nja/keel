using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keel
{
    public class SpecialFormException : Exception
    {
        public SpecialFormException(string msg)
            : base(msg)
        { }
    }

    public abstract class SpecialForm
    {
        private static Dictionary<Symbol, SpecialForm> specials = new Dictionary<Symbol, SpecialForm>();

        protected SpecialForm(Symbol symbol)
        {
            specials.Add(symbol, this);
        }

        public static bool IsSpecial(Symbol symbol)
        {
            return specials.ContainsKey(symbol);
        }

        public static LispObject Eval(Symbol symbol, Cons body, Environment env)
        {
            return specials[symbol].Eval(body, env);
        }

        public abstract LispObject Eval(Cons body, Environment env);
    }

    public class IfForm : SpecialForm
    {
        public static IfForm If = new IfForm();

        private IfForm()
            : base(new Symbol("IF"))
        { }

        public override LispObject Eval(Cons body, Environment env)
        {
            var test = ((Cons)body.Car).Car;

            if (env.Eval(test).IsTrue)
            {
                return env.Eval(Car.Of(Cdr.Of(body)));
            }
            else
            {
                return env.Eval(Car.Of(Cdr.Of(Cdr.Of(body))));
            }
        }
    }
}
