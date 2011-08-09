using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel
{
    public class SymbolsTable
    {
        private readonly Dictionary<string, Symbol> symbols = new Dictionary<string, Symbol>();

        public SymbolsTable()
        {
            symbols.Add(T.TrueName, T.True);
        }

        public Symbol Intern(string name)
        {
            var canonicalName = Symbol.Canonicalize(name);

            Symbol symbol;

            if (!symbols.TryGetValue(canonicalName, out symbol))
            {
                symbol = new Symbol(canonicalName);
                symbols.Add(canonicalName, symbol);
            }

            return symbol;
        }

        public bool TryLookup(string name, out Symbol symbol)
        {
            return symbols.TryGetValue(Symbol.Canonicalize(name), out symbol);
        }
    }
}
