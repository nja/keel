﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel.Builtins
{
    public abstract class Builtin : Function
    {
        public readonly Symbol Symbol;
        
        public Builtin(string name, params string[] args)
            : base(Cons.ToList(args.Select(s => new Symbol(s))), LispNull.Nil)
        {
            this.Symbol = new Symbol(Symbol.Canonicalize(name));
        }

        public Builtin(string name, object rest, params string[] args)
            : base(Cons.ToDottedList(args.Select(s => new Symbol(s))), LispNull.Nil)
        {
            throw new NotImplementedException();
        }

        public string Name { get { return Symbol.Name; } }
    }
}
