namespace Keel.Objects
{
    using System.Collections.Generic;
    using System.Linq;

    using Keel.Builtins;

    public class LispEnvironment : LispObject
    {
        #region Constants and Fields

        private readonly SymbolsTable symbols;

        private static readonly LispEnvironment NoParent = new LispEnvironment();

        private readonly Dictionary<Symbol, LispObject> dict = new Dictionary<Symbol, LispObject>();

        private readonly LispEnvironment parent;

        private readonly Specials specials;

        #endregion

        #region Constructors and Destructors

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

        private LispEnvironment()
        { }

        #endregion

        #region Public Properties

        public bool IsRoot { get { return parent == NoParent; } }

        public SymbolsTable Symbols { get { return symbols; } }

        #endregion

        #region Properties

        protected int Count
        {
            get { return dict.Count + (IsRoot ? 0 : parent.Count); }
        }

        protected int Level
        {
            get { return IsRoot ? 0 : parent.Level + 1; }
        }

        #endregion

        #region Public Methods and Operators

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

        public void AddBinding(string symbolName, LispObject value)
        {
            AddBinding(Symbols.Intern(Symbol.Canonicalize(symbolName)), value);
        }

        public LispObject Eval(LispObject expr)
        {
            if (expr is Symbol)
            {
                return LookUp((Symbol)expr);
            }
            
            if (expr.IsAtom)
            {
                return expr;
            }
            
            if (this.specials.IsSpecial((Cons)expr))
            {
                return this.specials.Eval((Cons)expr, this);
            }
            
            if (this.IsMacro((Cons)expr))
            {
                Cons expansion;
                MacroExpand.Expand((Cons)expr, this, out expansion);
                return this.Eval(expansion);
            }
            
            var funVal = this.Eval(Car.Of(expr));
            var fun = funVal.As<Function>();

            var arguments = Cdr.Of(expr);
            var argumentValues = Cons.ToList(arguments.As<Cons>().Select(this.Eval));

            return fun.Apply(argumentValues, this);
        }

        public void Extend(LispObject lambdaList, Cons values)
        {
            if (lambdaList.IsNil)
            {
                if (values.IsNil)
                {
                    return;
                }
                
                throw new EnvironmentException("Too many values when extending environment");
            }
            
            if (lambdaList.IsAtom)
            {
                this.AddBinding(lambdaList.As<Symbol>(), values);
            }
            else
            {
                if (values.IsNil)
                {
                    throw new EnvironmentException("Too few values when extending environment");
                }

                this.AddBinding(Car.Of(lambdaList).As<Symbol>(), values.Car);
                this.Extend(Cdr.Of(lambdaList), values.Cdr.As<Cons>());
            }
        }

        public void Extend(IEnumerable<Symbol> symbolsEnum, IEnumerable<LispObject> valuesEnum)
        {
            var symbolsList = symbolsEnum.ToList();
            var values = valuesEnum.ToList();

            if (symbolsList.Count != values.Count)
            {
                throw new EnvironmentException("Wrong number of values when extending environment");
            }

            for (int i = 0; i < symbolsList.Count; i++)
            {
                AddBinding(symbolsList[i], values[i]);
            }
        }

        public bool IsMacro(Cons form)
        {
            LispObject carValue;
            return form.Car is Symbol
                && TryLookUp((Symbol)form.Car, out carValue)
                && carValue is Macro;
        }

        public LispObject LookUp(Symbol symbol)
        {
            LispObject result;

            if (!this.TryLookUp(symbol, out result))
            {
                throw new EnvironmentException("No binding for " + symbol);
            }

            return result;
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

        public override string ToString()
        {
            return string.Format("<Environment[{0}]({1})>", Level, Count);
        }

        public bool TryLookUp(Symbol symbol, out LispObject value)
        {
            if (dict.TryGetValue(symbol, out value))
            {
                return true;
            }
            
            if (!this.IsRoot)
            {
                return this.parent.TryLookUp(symbol, out value);
            }
            
            return false;
        }

        #endregion
    }
}
