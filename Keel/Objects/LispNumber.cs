namespace Keel.Objects
{
    using System;
    using System.Numerics;

    public abstract class LispNumber : LispObject
    {
        public abstract LispNumber Negate();

        public abstract LispNumber Add(LispNumber addend);

        public abstract LispNumber Add(LispInteger addend);

        public abstract LispNumber Add(LispDouble addend);

        public abstract LispNumber Add(LispBigInteger addend);

        public abstract bool NumberEquals(LispNumber number);

        public abstract bool NumberEquals(LispInteger number);

        public abstract bool NumberEquals(LispDouble number);

        public abstract bool NumberEquals(LispBigInteger number);

        public abstract int CompareTo(LispNumber number);

        public abstract int CompareTo(LispInteger number);

        public abstract int CompareTo(LispDouble number);

        public abstract int CompareTo(LispBigInteger number);

        protected LispNumber Add(LispInteger a, LispInteger b)
        {
            try
            {
                return new LispInteger(checked(a.Value + b.Value));
            }
            catch (OverflowException)
            {
                return new LispBigInteger((BigInteger)a.Value + b.Value);
            }
        }

        protected LispNumber Add(LispDouble a, LispDouble b)
        {
            return new LispDouble(a.Value + b.Value);
        }

        protected LispNumber Add(LispBigInteger a, LispBigInteger b)
        {
            return new LispBigInteger(a.Value + b.Value);
        }

        protected bool NumberEquals(LispInteger a, LispInteger b)
        {
            return a.Value == b.Value;
        }

        protected bool NumberEquals(LispInteger a, LispBigInteger b)
        {
            return (BigInteger)a.Value == b.Value;
        }

        protected bool NumberEquals(LispBigInteger a, LispBigInteger b)
        {
            return a.Value == b.Value;
        }

        protected bool NumberEquals(LispInteger a, LispDouble b)
        {
            return a.Value == b.Value;
        }

        protected bool NumberEquals(LispBigInteger a, LispDouble b)
        {
            return IsIntegral(b.Value) && a.Value == (BigInteger)b.Value;
        }

        protected bool NumberEquals(LispDouble a, LispDouble b)
        {
            return a.Value == b.Value;
        }

        protected int Compare(LispInteger a, LispDouble b)
        {
            return ((double)a.Value).CompareTo(b.Value);
        }

        protected int Compare(LispInteger a, LispBigInteger b)
        {
            return ((BigInteger)a.Value).CompareTo(b.Value);
        }

        protected int Compare(LispBigInteger a, LispDouble b)
        {
            var truncatedB = new BigInteger(b.Value);

            var truncatedCompare = a.Value.CompareTo(truncatedB);

            if (truncatedCompare == 0)
            {
                return -Math.Truncate(b.Value).CompareTo(b.Value);
            }
            
            return truncatedCompare;
        }

        private bool IsIntegral(double x)
        {
            return Math.Truncate(x) == x;
        }
    }
}
