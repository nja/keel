using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Keel.Objects
{
    public abstract class LispNumber : LispObject
    {
        protected static IFormatProvider FormatProvider = CultureInfo.InvariantCulture;

        public abstract LispNumber Negate();

        public abstract LispNumber Add(LispNumber addend);
        public abstract LispNumber Add(LispInteger addend);
        public abstract LispNumber Add(LispDouble addend);

        protected LispNumber Add(LispInteger a, LispInteger b)
        {
            return new LispInteger(a.Value + b.Value);
        }

        protected LispNumber Add(LispDouble a, LispDouble b)
        {
            return new LispDouble(a.Value + b.Value);
        }
    }
}
