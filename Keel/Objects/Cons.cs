using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Builtins;

namespace Keel.Objects
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

        public static Cons ToList(params LispObject[] elements)
        {
            return ToList((IEnumerable<LispObject>)elements);
        }

        public static Cons ToList(IEnumerable<LispObject> elements)
        {
            var list = elements.ToList();
            list.Add(LispNull.Nil);
            return (Cons)ToDottedList(list);
        }

        public static LispObject ToDottedList(params LispObject[] elements)
        {
            return ToDottedList((IEnumerable<LispObject>)elements);
        }

        public static LispObject ToDottedList(IEnumerable<LispObject> elements)
        {
            var list = elements.ToList();
            
            if (list.Count == 0)
            {
                return LispNull.Nil;
            }
            else
            {
                list.Reverse();
                LispObject cdr = list[0];

                foreach (var car in list.Skip(1))
                {
                    cdr = new Cons(car, cdr);
                }

                return cdr;
            }
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

        public override string ToString()
        {
            return Print.PrintObject(this);
        }
    }
}
