using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keel.Objects
{
    public class LispDouble : LispNumber
    {
        public readonly double Value;

        public LispDouble(double value)
        {
            this.Value = value;
        }

        public LispDouble(LispInteger integer)
        {
            this.Value = Convert.ToDouble(integer.Value);
        }

        public override string ToString()
        {
            return Value.ToString(FormatProvider);
        }

        public override LispNumber Negate()
        {
            return new LispDouble(-Value);
        }

        public override LispNumber Add(LispNumber addend)
        {
            return addend.Add(this);
        }

        public override LispNumber Add(LispInteger addend)
        {
            return new LispDouble(addend).Add(this);
        }

        public override LispNumber Add(LispDouble addend)
        {
            return Add(this, addend);
        }
    }
}
