using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;
using Keel.Builtins;

namespace Keel
{
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
            Add(new Eq());
            Add(new EvalBuiltin());
            Add(new MacroExpand());
            Add(new Print());
            Add(new Subtract());

            AddBinding(T.True, T.True);
            AddBinding(Symbols.Intern(LispNull.Name), LispNull.Nil);
        }

        private void Add(Builtin builtin)
        {
            var symbol = Symbols.Intern(builtin.Name);
            AddBinding(symbol, builtin);
        }
    }
}
