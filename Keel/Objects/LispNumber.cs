namespace Keel.Objects
{
    using System;
    using System.Numerics;

    public abstract class LispNumber : LispObject
    {
        public static LispNumber MakeNumber(int x)
        {
            return new LispInteger(x);
        }

        public static LispNumber MakeNumber(BigInteger x)
        {
            if (x < int.MinValue || int.MaxValue < x)
            {
                return new LispBigInteger(x);
            }

            return new LispInteger((int)x);
        }

        public static LispNumber MakeNumber(double x)
        {
            return new LispDouble(x);
        }

        public abstract LispNumber Negate();

        public abstract LispNumber Add(LispNumber addend);

        public abstract LispNumber Add(LispInteger addend);

        public abstract LispNumber Add(LispDouble addend);

        public abstract LispNumber Add(LispBigInteger addend);

        public abstract LispNumber Multiply(LispNumber factor);

        public abstract LispNumber Multiply(LispInteger factor);

        public abstract LispNumber Multiply(LispDouble factor);

        public abstract LispNumber Multiply(LispBigInteger factor);

        public abstract LispNumber DivideBy(LispNumber divisor);

        public abstract LispNumber DivideBy(LispInteger divisor);

        public abstract LispNumber DivideBy(LispDouble divisor);

        public abstract LispNumber DivideBy(LispBigInteger divisor);

        public abstract LispNumber DivideInto(LispNumber dividend);

        public abstract LispNumber DivideInto(LispInteger dividend);

        public abstract LispNumber DivideInto(LispDouble dividend);

        public abstract LispNumber DivideInto(LispBigInteger dividend);

        public abstract bool NumberEquals(LispNumber number);

        public abstract bool NumberEquals(LispInteger number);

        public abstract bool NumberEquals(LispDouble number);

        public abstract bool NumberEquals(LispBigInteger number);

        public abstract int CompareTo(LispNumber number);

        public abstract int CompareTo(LispInteger number);

        public abstract int CompareTo(LispDouble number);

        public abstract int CompareTo(LispBigInteger number);
        
        protected static LispNumber Add(LispInteger a, LispInteger b)
        {
            try
            {
                var sum = checked(a.Value + b.Value);
                return MakeNumber(sum);
            }
            catch (OverflowException)
            {
                return new LispBigInteger((BigInteger)a.Value + b.Value);
            }
        }

        protected static LispNumber Add(LispDouble a, LispDouble b)
        {
            return MakeNumber(a.Value + b.Value);
        }

        protected static LispNumber Add(LispBigInteger a, LispBigInteger b)
        {
            return MakeNumber(a.Value + b.Value);
        }

        protected static LispNumber Divide(LispInteger dividend, LispInteger divisor)
        {
            try
            {
                checked
                {
                    int remainder;
                    int quotient = Math.DivRem(dividend.Value, divisor.Value, out remainder);

                    if (remainder != 0)
                    {
                        return Divide((LispDouble)dividend, (LispDouble)divisor);
                    }

                    return MakeNumber(quotient);
                }
            }
            catch (OverflowException)
            {
                return Divide((LispBigInteger)dividend, (LispBigInteger)divisor);
            }
        }

        protected static LispNumber Divide(LispBigInteger dividend, LispBigInteger divisor)
        {
            BigInteger remainder;
            BigInteger quotient = BigInteger.DivRem(dividend.Value, divisor.Value, out remainder);

            if (remainder != 0)
            {
                return Divide((LispDouble)dividend, (LispDouble)divisor);
            }

            return MakeNumber(quotient);
        }

        protected static LispNumber Divide(LispDouble dividend, LispDouble divisor)
        {
            return MakeNumber(dividend.Value / divisor.Value);
        }

        protected static LispNumber Multiply(LispInteger a, LispInteger b)
        {
            try
            {
                int product = checked(a.Value * b.Value);
                return MakeNumber(product);
            }
            catch (OverflowException)
            {
                BigInteger product = (BigInteger)a.Value * b.Value;
                return MakeNumber(product);
            }
        }

        protected static LispNumber Multiply(LispDouble a, LispDouble b)
        {
            return MakeNumber(a.Value * b.Value);
        }

        protected static LispNumber Multiply(LispBigInteger a, LispBigInteger b)
        {
            return MakeNumber(a.Value * b.Value);
        }

        protected static bool NumberEquals(LispInteger a, LispInteger b)
        {
            return a.Value == b.Value;
        }

        protected static bool NumberEquals(LispInteger a, LispBigInteger b)
        {
            return (BigInteger)a.Value == b.Value;
        }

        protected static bool NumberEquals(LispBigInteger a, LispBigInteger b)
        {
            return a.Value == b.Value;
        }

        protected static bool NumberEquals(LispInteger a, LispDouble b)
        {
            return a.Value == b.Value;
        }

        protected static bool NumberEquals(LispBigInteger a, LispDouble b)
        {
            return IsIntegral(b.Value) && a.Value == (BigInteger)b.Value;
        }

        protected bool NumberEquals(LispDouble a, LispDouble b)
        {
            return a.Value == b.Value;
        }

        protected static int Compare(LispInteger a, LispDouble b)
        {
            return ((double)a.Value).CompareTo(b.Value);
        }

        protected static int Compare(LispInteger a, LispBigInteger b)
        {
            return ((BigInteger)a.Value).CompareTo(b.Value);
        }

        protected static int Compare(LispBigInteger a, LispDouble b)
        {
            var truncatedB = new BigInteger(b.Value);

            var truncatedCompare = a.Value.CompareTo(truncatedB);

            if (truncatedCompare == 0)
            {
                return -Math.Truncate(b.Value).CompareTo(b.Value);
            }
            
            return truncatedCompare;
        }

        private static bool IsIntegral(double x)
        {
            return Math.Truncate(x) == x;
        }
    }
}
