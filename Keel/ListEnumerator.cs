namespace Keel
{
    using System;
    using System.Collections.Generic;

    using Keel.Objects;

    public class ListEnumerator : IEnumerator<LispObject>
    {
        #region Constants and Fields

        private static readonly Cons BeforeFirst = new Cons();

        private readonly Cons first;

        private Cons current;

        #endregion

        #region Constructors and Destructors

        public ListEnumerator(Cons first)
        {
            this.first = first;
            this.current = BeforeFirst;
        }

        #endregion

        #region Public Properties

        public LispObject Current
        {
            get
            {
                if (current == BeforeFirst)
                {
                    throw new InvalidOperationException("Current accessed before MoveNext()");
                }

                if (current.IsNil)
                {
                    throw new InvalidOperationException("Current accessed after MoveNext() past end");
                }

                return current.Car;
            }
        }

        #endregion

        #region Explicit Interface Properties

        object System.Collections.IEnumerator.Current
        {
            get { return this.Current; }
        }

        #endregion

        #region Public Methods and Operators

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public bool MoveNext()
        {
            if (current == BeforeFirst)
            {
                current = first;
            }
            else
            {
                var cdr = current.Cdr as Cons;

                if (cdr == null)
                {
                    throw new InvalidOperationException("Not a proper list");
                }

                current = cdr;
            }

            return !current.IsNil;
        }

        public void Reset()
        {
            current = BeforeFirst;
        }

        #endregion
    }
}
