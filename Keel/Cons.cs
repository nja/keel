using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keel
{
    public class Cons : LispObject, IEnumerable<LispObject>
    {
        private LispObject car, cdr;
        
        public Cons()
        {
            this.car = LispNull.Nil;
            this.cdr = LispNull.Nil;
        }

        public Cons(LispObject car, LispObject cdr)
        {
            this.car = car;
            this.cdr = cdr;
        }

        public static Cons ToList(IEnumerable<LispObject> elements)
        {
            Cons prev = LispNull.Nil,
                first = LispNull.Nil;

            foreach (var e in elements)
            {
                var cons = new Cons(e, LispNull.Nil);

                if (first == LispNull.Nil)
                {
                    first = cons;
                } 
                else
                {
                    prev.Cdr = cons;
                }

                prev = cons;
            }

            return first;
        }

        public virtual LispObject Car
        {
            get { return car; }
            set { this.car = value; }
        }

        public virtual LispObject Cdr
        {
            get { return cdr; }
            set { this.cdr = value; }
        }

        public override bool IsAtom
        {
            get { return false; }
        }

        #region IEnumerable<LispObject> Members

        public IEnumerator<LispObject> GetEnumerator()
        {
            return new ListEnumerator(this);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
