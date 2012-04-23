namespace Keel.Builtins
{
    using Keel.Objects;

    public class Cdr : Builtin
    {
        public Cdr()
            : base("CDR", "X")
        { }

        public static LispObject Of(LispObject x)
        {
            return x.As<Cons>().Cdr;
        }

        public override LispObject Apply(Cons argumentValues, LispEnvironment env)
        {
            return Cdr.Of(argumentValues.Car);
        }
    }
}
