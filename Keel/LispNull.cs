using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keel
{
    public class LispNull : Cons
    {
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
    }
}
