using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel.Builtins
{
    public class DelegateBuiltin : Builtin
    {
        private readonly Func<LispObject> fun;

        public DelegateBuiltin(string name, Func<LispObject> fun)
            : base(name)
        {
            this.fun = fun;
        }

        public override LispObject Apply(Cons arguments, LispEnvironment env)
        {
            return fun();
        }
    }
}
