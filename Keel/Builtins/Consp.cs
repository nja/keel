namespace Keel.Builtins
{
    using Keel.Objects;

    public class Consp : Builtin
    {
        public Consp()
            : base("CONSP", "X")
        { }

        public override LispObject Apply(Cons arguments, LispEnvironment env)
        {
            if (arguments.Car.IsCons)
            {
                return T.True;
            }
            
            return LispNull.Nil;
        }
    }
}
