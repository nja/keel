using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keel
{
    public class EnvironmentException : Exception
    {
        public EnvironmentException(string msg)
            : base(msg)
        { }

        public EnvironmentException(string msg, Exception inner)
            : base(msg, inner)
        { }
    }

    public class EvaluationException : Exception
    {
        public EvaluationException(string msg)
            : base(msg)
        { }

        public EvaluationException(string msg, Exception inner)
            : base(msg, inner)
        { }
    }

    public class Environment
    {
        static readonly Environment NoParent = new Environment(null);

        private readonly Environment parent;
        private readonly Dictionary<Symbol, LispObject> dict = new Dictionary<Symbol, LispObject>();

        public Environment()
            : this(NoParent)
        { }

        public Environment(Environment parent)
        {
            this.parent = parent;
        }

        public LispObject LookUp(Symbol symbol)
        {
            LispObject result = null;

            if (dict.TryGetValue(symbol, out result))
            {
                return result;
            }
            else if (parent != NoParent)
            {
                return parent.LookUp(symbol);
            }
            else
            {
                throw new EnvironmentException("No binding for " + symbol);
            }
        }

        public void AddBinding(Symbol symbol, LispObject value)
        {
            if (!dict.ContainsKey(symbol))
            {
                dict.Add(symbol, value);
            }
            else
            {
                throw new EnvironmentException(symbol + " already bound");
            }
        }

        public void SetValue(Symbol symbol, LispObject value)
        {
            if (dict.ContainsKey(symbol))
            {
                dict[symbol] = value;
            }
            else
            {
                throw new EnvironmentException("No binding for " + symbol);
            }
        }

        public void Extend(IEnumerable<Symbol> symbolsEnum, IEnumerable<LispObject> valuesEnum)
        {
            var symbols = symbolsEnum.ToList();
            var values = valuesEnum.ToList();

            if (symbols.Count != values.Count)
            {
                throw new EnvironmentException("Wrong number of values when extending environment");
            }

            for (int i = 0; i < symbols.Count; i++)
            {
                AddBinding(symbols[i], values[i]);
            }
        }

        public LispObject Eval(LispObject expr)
        {
            if (expr.IsNil)
            {
                return LispNull.Nil;
            }
            else if (expr is Symbol)
            {
                return LookUp((Symbol)expr);
            }
            else if (expr.IsAtom)
            {
                return expr;
            }
            else
            {
                Cons cons = (Cons)expr;

                if (cons.Car is Symbol && SpecialForm.IsSpecial((Symbol)cons.Car))
                {
                    return EvalSpecial(cons);
                }
                else
                {
                    return EvalFunction(cons);
                }
            }
        }

        private LispObject EvalFunction(Cons cons)
        {
            Function fun = Eval(cons.Car) as Function;

            if (fun == null)
            {
                throw new EvaluationException(cons.Car + " is not a function");
            }

            var argumentValues = ((Cons)cons.Cdr).Select(x => Eval(x));
                
            Environment env = new Environment(this);
            env.Extend(fun.Arguments, argumentValues);

            return fun.Apply(env);
        }

        private LispObject EvalSpecial(Cons cons)
        {
            return SpecialForm.Eval((Symbol)cons.Car, (Cons)cons.Cdr, this);
        }
    }
}
