namespace Keel.Objects
{
    using System.Globalization;
    using System.Numerics;

    public class LispInteger : LispNumber
    {
        #region Constants and Fields

        public static readonly LispInteger Max = new LispInteger(int.MaxValue), 
                                           Min = new LispInteger(int.MinValue);

        public readonly int Value;

        #endregion

        #region Constructors and Destructors

        public LispInteger(int value)
        {
            this.Value = value;
        }

        #endregion

        #region Public Methods and Operators

        public static explicit operator LispBigInteger(LispInteger i)
        {
            return new LispBigInteger(i);
        }

        public static explicit operator LispDouble(LispInteger i)
        {
            return new LispDouble(i);
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
            return Add((LispDouble)this, addend);
        }

        public override LispNumber Add(LispBigInteger addend)
        {
            return Add((LispBigInteger)this, addend);
        }

        public override int CompareTo(LispNumber number)
        {
            return -number.CompareTo(this);
        }

        public override int CompareTo(LispInteger number)
        {
            return Value.CompareTo(number.Value);
        }

        public override int CompareTo(LispDouble number)
        {
            return Compare(this, number);
        }

        public override int CompareTo(LispBigInteger number)
        {
            return Compare(this, number);
        }

        public override LispNumber DivideBy(LispNumber divisor)
        {
            return divisor.DivideInto(this);
        }

        public override LispNumber DivideBy(LispInteger divisor)
        {
            return Divide(this, divisor);
        }

        public override LispNumber DivideBy(LispDouble divisor)
        {
            return Divide((LispDouble)this, divisor);
        }

        public override LispNumber DivideBy(LispBigInteger divisor)
        {
            return Divide((LispBigInteger)this, divisor);
        }

        public override LispNumber DivideInto(LispNumber dividend)
        {
            return dividend.DivideBy(this);
        }

        public override LispNumber DivideInto(LispInteger dividend)
        {
            return Divide(dividend, this);
        }

        public override LispNumber DivideInto(LispDouble dividend)
        {
            return Divide(dividend, (LispDouble)this);
        }

        public override LispNumber DivideInto(LispBigInteger dividend)
        {
            return Divide(dividend, (LispBigInteger)this);
        }

        public override LispNumber Multiply(LispNumber factor)
        {
            return factor.Multiply(this);
        }

        public override LispNumber Multiply(LispInteger factor)
        {
            return Multiply(this, factor);
        }

        public override LispNumber Multiply(LispDouble factor)
        {
            return Multiply((LispDouble)this, factor);
        }

        public override LispNumber Multiply(LispBigInteger factor)
        {
            return Multiply((LispBigInteger)this, factor);
        }

        public override LispNumber Negate()
        {
            if (Value > int.MinValue)
            {
                return new LispInteger(Value * -1);
            }
            
            return new LispBigInteger((BigInteger)this.Value * -1);
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

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        #endregion
    }
}
