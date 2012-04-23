namespace Keel.Builtins
{
    using Keel.Objects;

    public class Atom : Builtin
    {
        public Atom()
            : base("ATOM", "X")
        { }

        public override LispObject Apply(Cons arguments, LispEnvironment env)
        {
            if (arguments.Car.IsAtom)
            {
                return T.True;
            }
            
            return LispNull.Nil;
        }
    }
}
