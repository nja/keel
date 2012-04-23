namespace Keel.Builtins
{
    using Keel.Objects;

    public class EvalBuiltin : Builtin
    {
        public EvalBuiltin()
            : base("EVAL", "FORM")
        { }

        public override LispObject Apply(Cons arguments, LispEnvironment env)
        {
            return env.Eval(arguments.Car);
        }
    }
}
