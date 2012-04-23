namespace Keel.Builtins
{
    using Keel.Objects;

    public class ConsBuiltin : Builtin
    {
        public ConsBuiltin()
            : base("CONS", "X", "Y")
        { }

        public override LispObject Apply(Cons argumentValues, LispEnvironment env)
        {
            return new Cons(Car.Of(argumentValues), Car.Of(Cdr.Of(argumentValues)));
        }
    }
}
