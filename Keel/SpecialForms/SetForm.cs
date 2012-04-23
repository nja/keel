namespace Keel.SpecialForms
{
    using Keel.Objects;
    using Keel.Builtins;

    public class SetForm : SpecialForm
    {
        public SetForm()
            : base("SET!")
        { }

        public override LispObject Eval(Cons body, LispEnvironment env)
        {
            var symbol = Car.Of(body).As<Symbol>();
            var value = env.Eval(Car.Of(Cdr.Of(body)));

            env.SetValue(symbol, value);

            return value;
        }
    }
}
