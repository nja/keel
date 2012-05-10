namespace Keel.Objects
{
    using System.Globalization;
    using System.Numerics;

    public class LispBigInteger : LispNumber
    {
        public readonly BigInteger Value;

        public LispBigInteger(LispInteger value)
        {
            this.Value = value.Value;
        }

        public LispBigInteger(BigInteger value)
        {
            this.Value = value;
        }

        public static explicit operator LispDouble(LispBigInteger bi)
        {
            return new LispDouble(bi);
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public override LispNumber Negate()
        {
            return new LispBigInteger(Value * -1);
        }

        public override LispNumber Add(LispNumber addend)
        {
            return addend.Add(this);
        }

        public override LispNumber Add(LispInteger addend)
        {
            return new LispBigInteger(addend).Add(this);
        }

        public override LispNumber Add(LispDouble addend)
        {
            return new LispDouble(this).Add(addend);
        }

        public override LispNumber Add(LispBigInteger addend)
        {
            return Add(this, addend);
        }

        public override LispNumber Multiply(LispNumber factor)
        {
            return factor.Multiply(this);
        }

        public override LispNumber Multiply(LispInteger factor)
        {
            return Multiply(this, (LispBigInteger)factor);
        }

        public override LispNumber Multiply(LispDouble factor)
        {
            return Multiply((LispDouble)this, factor);
        }

        public override LispNumber Multiply(LispBigInteger factor)
        {
            return Multiply(this, factor);
        }

        public override LispNumber DivideBy(LispNumber divisor)
        {
            return divisor.DivideInto(this);
        }

        public override LispNumber DivideBy(LispInteger divisor)
        {
            return Divide(this, (LispBigInteger)divisor);
        }

        public override LispNumber DivideBy(LispDouble divisor)
        {
            return Divide((LispDouble)this, divisor);
        }

        public override LispNumber DivideBy(LispBigInteger divisor)
        {
            return Divide(this, divisor);
        }

        public override LispNumber DivideInto(LispNumber dividend)
        {
            return dividend.DivideBy(this);
        }

        public override LispNumber DivideInto(LispInteger dividend)
        {
            return Divide((LispBigInteger)dividend, this);
        }

        public override LispNumber DivideInto(LispDouble dividend)
        {
            return Divide(dividend, (LispDouble)this);
        }

        public override LispNumber DivideInto(LispBigInteger dividend)
        {
            return Divide(dividend, this);
        }

        public override bool NumberEquals(LispNumber number)
        {
            return number.NumberEquals(this);
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
            return NumberEquals(this, number);
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
            return Compare(this, number);
        }

        public override int CompareTo(LispBigInteger number)
        {
            return Value.CompareTo(number.Value);
        }
    }
}
