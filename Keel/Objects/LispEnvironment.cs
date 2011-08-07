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
        static readonly LispEnvironment NoParent = new LispEnvironment();

        private readonly LispEnvironment parent;
        private readonly Dictionary<Symbol, LispObject> dict = new Dictionary<Symbol, LispObject>();
        private readonly Specials specials;
        protected readonly SymbolsTable symbols;

        private LispEnvironment()
        { }

        public LispEnvironment(SymbolsTable symbols, Specials specials)
            : this(NoParent)
        {
            this.symbols = symbols;
            this.specials = specials;
        }

        public LispEnvironment(LispEnvironment parent)
        {
            this.parent = parent;
            this.symbols = parent.symbols;
            this.specials = parent.specials;

            if (Level >= 256)
            {
                throw new EnvironmentException("Stack overflow!");
            }
        }

        public bool IsRoot { get { return parent == NoParent; } }

        public bool TryLookUp(Symbol symbol, out LispObject value)
        {
            if (dict.TryGetValue(symbol, out value))
            {
                return true;
            }
            else if (!IsRoot)
            {
                return parent.TryLookUp(symbol, out value);
            }
            else
            {
                return false;
            }
        }

        public LispObject LookUp(Symbol symbol)
        {
            LispObject result = null;

            if (TryLookUp(symbol, out result))
            {
                return result;
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
            else if (!IsRoot)
            {
                parent.SetValue(symbol, value);
            }
            else
            {
                throw new EnvironmentException("No binding for " + symbol);
            }
        }

        public void Extend(LispObject lambdaList, Cons values)
        {
            if (lambdaList.IsNil)
            {
                if (values.IsNil)
                {
                    return;
                }
                else
                {
                    throw new EnvironmentException("Too many values when extending environment");
                }
            }
            else if (lambdaList.IsAtom)
            {
                AddBinding(lambdaList.As<Symbol>(), values);
            }
            else
            {
                if (values.IsNil)
                {
                    throw new EnvironmentException("Too few values when extending environment");
                }

                AddBinding(Car.Of(lambdaList).As<Symbol>(), values.Car);
                Extend(Cdr.Of(lambdaList), values.Cdr.As<Cons>());
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
            else if (specials.IsSpecial((Cons)expr))
            {
                return specials.Eval((Cons)expr, this);
            }
            else if (IsMacro((Cons)expr))
            {
                Cons expansion;
                MacroExpand.Expand((Cons)expr, this, out expansion);
                return Eval(expansion);
            }
            else
            {
                var funVal = Eval(Car.Of(expr));
                Function fun = funVal.As<Function>();

                var arguments = Cdr.Of(expr);
                var argumentValues = Cons.ToList(arguments.As<Cons>().Select(car => Eval(car)));

                return fun.Apply(argumentValues, this);
            }
        }

        public SymbolsTable Symbols { get { return symbols; } }

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

        public bool IsMacro(Cons form)
        {
            LispObject carValue;
            return form.Car is Symbol
                && TryLookUp((Symbol)form.Car, out carValue)
                && carValue is Macro;
        }
    }
}
