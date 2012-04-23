namespace Keel.Builtins
{
    using Keel.Objects;

    public class Eq : Builtin
    {
        public Eq()
            : base("EQ", "X", "Y")
        { }

        public override LispObject Apply(Cons argumentValues, LispEnvironment env)
        {
            var x = Car.Of(argumentValues);
            var y = Car.Of(Cdr.Of(argumentValues));

            if (x == y)
            {
                return T.True;
            }
            
            if (x is LispNumber && y is LispNumber
                && ((LispNumber)x).NumberEquals((LispNumber)y))
            {
                return T.True;
            }
            
            return LispNull.Nil;
        }
    }
}
