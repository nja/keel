namespace Keel.Objects
{
    using System.Globalization;
    using System.Numerics;

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
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public override LispNumber Negate()
        {
            if (Value > int.MinValue)
            {
                return new LispInteger(Value * -1);
            }
            
            return new LispBigInteger((BigInteger)this.Value * -1);
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
    }
}
