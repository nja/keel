using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.SpecialForms;
using Keel.Builtins;

namespace Keel.Objects
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

    public class LispEnvironment : LispObject
    {
        static readonly LispEnvironment NoParent = new LispEnvironment(null);

        private readonly LispEnvironment parent;
        private readonly Dictionary<Symbol, LispObject> dict = new Dictionary<Symbol, LispObject>();

        public LispEnvironment()
            : this(NoParent)
        { }

        public LispEnvironment(LispEnvironment parent)
        {
            this.parent = parent;
        }

        public bool IsRoot { get { return parent == NoParent; } }

        public LispObject LookUp(Symbol symbol)
        {
            LispObject result = null;

            if (dict.TryGetValue(symbol, out result))
            {
                return result;
            }
            else if (!IsRoot)
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
            if (expr is Symbol)
            {
                return LookUp((Symbol)expr);
            }
            else if (expr.IsAtom)
            {
                return expr;
            }
            else if (SpecialForm.IsSpecial((Cons)expr))
            {
                return SpecialForm.EvalForm((Cons)expr, this);
            }
            else
            {
                var funVal = Eval(Car.Of(expr));
                Function fun = funVal as Function;

                if (fun == null)
                {
                    throw new EvaluationException(funVal + " is not a function");
                }

                var arguments = Cdr.Of(expr);
                var argumentValues = arguments.As<Cons>().Select(car => Eval(car));

                return fun.Apply(argumentValues, this);
            }
        }

        protected int Level
        {
            get { return IsRoot ? 0 : parent.Level + 1; }
        }

        protected int Count
        {
            get { return dict.Count + (IsRoot ? 0 : parent.Count); }
        }

        public override string ToString()
        {
            return string.Format("Env[{0}]({1})", Level, Count);
        }
    }
}
