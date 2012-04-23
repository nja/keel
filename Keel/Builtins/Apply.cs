namespace Keel.Builtins
{
    using Keel.Objects;

    public class ApplyBuiltin : Builtin
    {
        public ApplyBuiltin()
            : base("APPLY")
        { }

        public static Cons Spread(Cons args)
        {
            if (args.Cdr.IsNil)
            {
                return args.Car.As<Cons>();
            }
            
            return new Cons(args.Car, Spread(args.Cdr.As<Cons>()));
        }

        public override LispObject Apply(Cons argumentValues, LispEnvironment env)
        {
            var fun = argumentValues.Car.As<Function>();
            var args = argumentValues.Cdr.As<Cons>();
            
            var spread = Spread(args);

            return fun.Apply(spread, env);
        }
    }
}
