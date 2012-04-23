namespace Keel.Builtins
{
    using System;

    using Keel.Objects;

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
