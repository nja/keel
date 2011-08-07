﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel.Builtins
{
    public class Cdr : Builtin
    {
        public Cdr()
            : base("CDR", "X")
        { }

        public override LispObject Apply(Cons argumentValues, LispEnvironment env)
        {
            return Cdr.Of(argumentValues.Car);
        }

        public static LispObject Of(LispObject x)
        {
            return x.As<Cons>().Cdr;
        }
    }
}
