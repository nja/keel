using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel.Builtins
{
    public class Cdr : Builtin
    {
        public static readonly Cdr Instance = new Cdr();

        private Cdr()
            : base("CDR", "X")
        { }

        protected override LispObject Apply(LispEnvironment env, LispObject[] args)
        {
            return Cdr.Of(args[0]);
        }

        public static LispObject Of(LispObject x)
        {
            return x.As<Cons>().Cdr;
        }
    }
}
