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
        {
            // Builtins
            AddBinding(Car.Instance.Symbol, Car.Instance);
            AddBinding(Cdr.Instance.Symbol, Cdr.Instance);
            AddBinding(ConsBuiltin.Instance.Symbol, ConsBuiltin.Instance);
            // Constants
            AddBinding(LispNull.Symbol, LispNull.Nil);
            AddBinding(DefaultSymbols.T, DefaultSymbols.T);
        }
    }
}
