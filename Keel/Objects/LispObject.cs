using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
