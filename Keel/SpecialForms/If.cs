namespace Keel.SpecialForms
{
    using Keel.Builtins;
    using Keel.Objects;

    public class If : SpecialForm
    {
        public If()
            : base("IF")
        { }

        public override LispObject Eval(Cons body, LispEnvironment env)
        {
            var testForm = Car.Of(body);
            var thenForm = Car.Of(Cdr.Of(body));
            var elseForm = Car.Of(Cdr.Of(Cdr.Of(body)));

            if (env.Eval(testForm).IsTrue)
            {
                return env.Eval(thenForm);
            }
            
            return env.Eval(elseForm);
        }
    }
}
