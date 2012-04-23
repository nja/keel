namespace Keel.Objects
{
    public class LispObject
    {
        public bool IsNil
        {
            get { return this == LispNull.Nil; }
        }

        public bool IsTrue
        {
            get { return !IsNil; }
        }

        public virtual bool IsAtom
        {
            get { return true; }
        }

        public virtual bool IsCons
        {
            get { return false; }
        }

        public TLo As<TLo>() where TLo : LispObject
        {
            TLo lo = this as TLo;

            if (lo == null)
            {
                throw new TypeException(this + " is not of type " + typeof(TLo).Name);
            }

            return lo;
        }
    }
}
