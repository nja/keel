namespace Keel.SpecialForms
{
    using Keel.Builtins;
    using Keel.Objects;

    public class Define : SpecialForm
    {
        public Define()
            : base("DEFINE")
        { }

        public override LispObject Eval(Cons body, LispEnvironment env)
        {
            var symbol = Car.Of(body).As<Symbol>();
            var value = env.Eval(Car.Of(Cdr.Of(body)));

            env.AddBinding(symbol, value);

            return symbol;
        }
    }
}
