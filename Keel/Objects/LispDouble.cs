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
            this.Value = (double)integer.Value;
        }

        public LispDouble(LispBigInteger bigInteger)
        {
            this.Value = (double)bigInteger.Value;
        }

        public override string ToString()
        {
            return Value.ToString("R", FormatProvider);
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

        public override LispNumber Add(LispBigInteger addend)
        {
            return new LispDouble(addend).Add(this);
        }

        public override bool NumberEquals(LispNumber number)
        {
            return number.Equals(this);
        }

        public override bool NumberEquals(LispInteger number)
        {
            return NumberEquals(number, this);
        }

        public override bool NumberEquals(LispDouble number)
        {
            return NumberEquals(this, number);
        }

        public override bool NumberEquals(LispBigInteger number)
        {
            return NumberEquals(number, this);
        }

        public override int CompareTo(LispNumber number)
        {
            return -number.CompareTo(this);
        }

        public override int CompareTo(LispInteger number)
        {
            return -Compare(number, this);
        }

        public override int CompareTo(LispDouble number)
        {
            return Value.CompareTo(number.Value);
        }

        public override int CompareTo(LispBigInteger number)
        {
            return -Compare(number, this);
        }
    }
}
