namespace Keel.Builtins
{
    using Keel.Objects;

    public class Add : Builtin
    {
        public Add()
            : base("+")
        { }

        public override LispObject Apply(Cons argumentsValues, LispEnvironment env)
        {
            LispNumber sum = new LispInteger(0);

            foreach (var addend in argumentsValues)
            {
                var num = addend.As<LispNumber>();
                sum = sum.Add(num);
            }

            return sum;
        }
    }
}
