using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keel.Objects
{
    public class LispNull : Cons
    {
        public static readonly Symbol Symbol = new Symbol("NIL");
        public static readonly LispNull Nil = new LispNull();

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

        public override string ToString()
        {
            return Symbol.Name;
        }
    }
}
