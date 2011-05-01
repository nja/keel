using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keel
{
    public class ListEnumerator : IEnumerator<LispObject>
    {
        static readonly Cons BeforeFirst = new Cons();

        private Cons current;
        private readonly Cons first;

        public ListEnumerator(Cons first)
        {
            this.first = first;
            this.current = BeforeFirst;
        }

        #region IEnumerator<LispObject> Members

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

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IEnumerator Members

        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            if (current == BeforeFirst)
            {
                current = first;
            }
            else
            {
                Cons cdr = current.Cdr as Cons;

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
