using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Builtins;
using Keel.SpecialForms;

namespace Keel
{
    public class DefaultSymbols : SymbolsTable
    {
        public DefaultSymbols()
        {
            Set(Car.Instance.Symbol);
            Set(Cdr.Instance.Symbol);
            Set(ConsBuiltin.Instance.Symbol);
            Set(If.Instance.Symbol);
            Set(LambdaForm.Instance.Symbol);
            Set(Progn.Instance.Symbol);
            Set(Quote.Instance.Symbol);
        }
    }
}
