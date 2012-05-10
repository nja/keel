namespace Keel.Objects
{
    using System.Globalization;

    public class LispDouble : LispNumber
    {
        #region Constants and Fields

        public readonly double Value;

        #endregion

        #region Constructors and Destructors

        public LispDouble(double value)
        {
            this.Value = value;
        }

        public LispDouble(LispInteger integer)
        {
            this.Value = integer.Value;
        }

        public LispDouble(LispBigInteger bigInteger)
        {
            this.Value = (double)bigInteger.Value;
        }

        #endregion

        #region Public Methods and Operators

        public override LispNumber Add(LispNumber addend)
        {
            return addend.Add(this);
        }

        public override LispNumber Add(LispInteger addend)
        {
            return Add(this, (LispDouble)addend);
        }

        public override LispNumber Add(LispDouble addend)
        {
            return Add(this, addend);
        }

        public override LispNumber Add(LispBigInteger addend)
        {
            return Add(this, (LispDouble)addend);
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

        public override LispNumber DivideBy(LispNumber divisor)
        {
            return divisor.DivideInto(this);
        }

        public override LispNumber DivideBy(LispInteger divisor)
        {
            return Divide(this, (LispDouble)divisor);
        }

        public override LispNumber DivideBy(LispBigInteger divisor)
        {
            return Divide(this, (LispDouble)divisor);
        }

        public override LispNumber DivideBy(LispDouble divisor)
        {
            return Divide(this, divisor);
        }

        public override LispNumber DivideInto(LispNumber dividend)
        {
            return DivideBy(this);
        }

        public override LispNumber DivideInto(LispInteger dividend)
        {
            return Divide((LispDouble)dividend, this);
        }

        public override LispNumber DivideInto(LispDouble dividend)
        {
            return Divide(dividend, this);
        }

        public override LispNumber DivideInto(LispBigInteger dividend)
        {
            return Divide((LispDouble)dividend, this);
        }

        public override LispNumber Multiply(LispNumber factor)
        {
            return factor.Multiply(this);
        }

        public override LispNumber Multiply(LispInteger factor)
        {
            return Multiply(this, (LispDouble)factor);
        }

        public override LispNumber Multiply(LispDouble factor)
        {
            return Multiply(this, factor);
        }

        public override LispNumber Multiply(LispBigInteger factor)
        {
            return Multiply(this, (LispDouble)factor);
        }

        public override LispNumber Negate()
        {
            return new LispDouble(-Value);
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

        public override string ToString()
        {
            return Value.ToString("R", CultureInfo.InvariantCulture);
        }

        #endregion
    }
}
