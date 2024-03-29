﻿namespace Keel
{
    using Keel.Builtins;
    using Keel.Objects;

    public class DefaultEnvironment : LispEnvironment
    {
        public DefaultEnvironment()
            : base(new SymbolsTable(), new Specials())
        {
            Add(new Add());
            Add(new ApplyBuiltin());
            Add(new Atom());
            Add(new Car());
            Add(new Cdr());
            Add(new ConsBuiltin());
            Add(new Consp());
            Add(new Divide());
            Add(new Eq());
            Add(new EvalBuiltin());
            Add(new NumberInequality("<", (a, b) => a.CompareTo(b) < 0));
            Add(new NumberInequality(">", (a, b) => a.CompareTo(b) > 0));
            Add(new NumberInequality("<=", (a, b) => a.CompareTo(b) <= 0));
            Add(new NumberInequality(">=", (a, b) => a.CompareTo(b) >= 0));
            Add(new NumberInequality("=", (a, b) => a.CompareTo(b) == 0));
            Add(new MacroExpand());
            Add(new Print());
            Add(new Subtract());

            AddBinding(T.True, T.True);
            AddBinding(LispNull.Name, LispNull.Nil);

            AddBinding("MAXINT", LispInteger.Max);
            AddBinding("MININT", LispInteger.Min);
        }

        private void Add(Builtin builtin)
        {
            var symbol = Symbols.Intern(builtin.Name);
            AddBinding(symbol, builtin);
        }
    }
}
