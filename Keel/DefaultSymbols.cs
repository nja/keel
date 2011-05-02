using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Builtins;
using Keel.SpecialForms;
using Keel.Objects;

namespace Keel
{
    public class DefaultSymbols : SymbolsTable
    {
        public static readonly Symbol T = new Symbol("T");

        public DefaultSymbols()
        {
            // Builtins
            Set(Add.Instance.Symbol);
            Set(ApplyBuiltin.Instance.Symbol);
            Set(Car.Instance.Symbol);
            Set(Cdr.Instance.Symbol);
            Set(ConsBuiltin.Instance.Symbol);
            Set(Eq.Instance.Symbol);
            Set(Print.Instance.Symbol);
            // Special forms
            Set(Define.Instance.Symbol);
            Set(Defun.Instance.Symbol);
            Set(If.Instance.Symbol);
            Set(LambdaForm.Instance.Symbol);
            Set(Progn.Instance.Symbol);
            Set(Quote.Instance.Symbol);
            // Constants
            Set(LispNull.Symbol);
            Set(T);
        }
    }
}
