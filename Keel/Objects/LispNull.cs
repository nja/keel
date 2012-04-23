namespace Keel.Objects
{
    using System;

    public class LispNull : Cons
    {
        public static readonly LispNull Nil = new LispNull();
        public const string Name = "NIL";

        private LispNull()
        { }

        public override LispObject Car
        {
            get { return this; }
            set { throw new InvalidOperationException(); }
        }

        public override LispObject Cdr
        {
            get { return this; }
            set { throw new InvalidOperationException(); }
        }

        public override bool IsAtom
        {
            get { return true; }
        }

        /// <summary>
        /// LispNull inherits Cons for the Car and Cdr properties, but (consp NIL) => NIL
        /// </summary>
        public override bool IsCons
        {
            get { return false; }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
