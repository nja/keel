namespace Keel.Builtins
{
    using Keel.Objects;

    public class Car : Builtin
    {
        public Car()
            : base("CAR", "X")
        { }

        public static LispObject Of(LispObject x)
        {
            return x.As<Cons>().Car;
        }

        public override LispObject Apply(Cons argumentValues, LispEnvironment env)
        {
            return Car.Of(argumentValues.Car);
        }
    }
}
