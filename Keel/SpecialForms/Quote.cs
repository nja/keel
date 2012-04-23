namespace Keel.SpecialForms
{
    using Keel.Builtins;
    using Keel.Objects;

    public class Quote : SpecialForm
    {
        public Quote()
            : base("QUOTE")
        { }

        public override LispObject Eval(Cons body, LispEnvironment env)
        {
            return Car.Of(body);
        }
    }
}
