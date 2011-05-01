using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel.Builtins
{
    public abstract class Builtin : Function
    {
        private readonly string name;
        
        public Builtin(string name, params string[] args)
            : base(args.Select(s => new Symbol(s)), LispNull.Nil)
        {
            this.name = name;
        }

        public string Name { get { return name; } }

        public override LispObject Apply(IEnumerable<LispObject> arguments, LispEnvironment env)
        {
            return Apply(env, Arguments.ToArray());
        }

        protected abstract LispObject Apply(LispEnvironment env, LispObject[] args);
    }
}
