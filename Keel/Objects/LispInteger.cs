using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Keel.Objects
{
    public class LispInteger : LispNumber
    {
        public readonly int Value;
        public static readonly LispInteger Max = new LispInteger(int.MaxValue), 
                                           Min = new LispInteger(int.MinValue);

        public LispInteger(int value)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            return Value.ToString(FormatProvider);
        }

        public override LispNumber Negate()
        {
            if (Value > int.MinValue)
            {
                return new LispInteger(Value * -1);
            }
            else
            {
                return new LispBigInteger((BigInteger)Value * -1);
            }
        }

        public override LispNumber Add(LispNumber addend)
        {
            return addend.Add(this);
        }

        public override LispNumber Add(LispInteger addend)
        {
            return Add(this, addend);
        }

        public override LispNumber Add(LispDouble addend)
        {
            return addend.Add(this);
        }

        public override LispNumber Add(LispBigInteger addend)
        {
            return addend.Add(this);
        }

        public override bool NumberEquals(LispNumber number)
        {
            return number.NumberEquals(this);
        }

        public override bool NumberEquals(LispInteger number)
        {
            return Value == number.Value;
        }

        public override bool NumberEquals(LispDouble number)
        {
            return NumberEquals(this, number);
        }

        public override bool NumberEquals(LispBigInteger number)
        {
            return NumberEquals(this, number);
        }
    }
}
