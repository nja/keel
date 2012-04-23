namespace Keel.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Keel.Builtins;

    public class Cons : LispObject, IEnumerable<LispObject>
    {
        #region Constants and Fields

        private LispObject car, cdr;

        #endregion

        #region Constructors and Destructors

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

        #endregion

        #region Public Properties

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

        public override bool IsCons
        {
            get { return true; }
        }

        #endregion

        #region Public Methods and Operators

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
            
            list.Reverse();
            LispObject cdr = list[0];

            foreach (var car in list.Skip(1))
            {
                cdr = new Cons(car, cdr);
            }

            return cdr;
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

        public IEnumerator<LispObject> GetEnumerator()
        {
            return new ListEnumerator(this);
        }

        public Cons Map(Func<LispObject, LispObject> fn)
        {
            return ToList(this.Select(fn));
        }

        public override string ToString()
        {
            return Print.PrintObject(this);
        }

        #endregion

        #region Explicit Interface Methods

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
