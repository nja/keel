namespace Keel.SpecialForms
{
    using Keel.Objects;

    public class Progn : SpecialForm
    {
        private const string PrognName = "PROGN";

        public Progn()
            : base(PrognName)
        { }

        public static Cons Wrap(Cons body, LispEnvironment env)
        {
            return new Cons(env.Symbols.Intern(PrognName), body);
        }

        public override LispObject Eval(Cons body, LispEnvironment env)
        {
            LispObject value;

            do
            {
                value = env.Eval(body.Car);
                body = body.Cdr.As<Cons>();
            }
            while (!body.IsNil);

            return value;
        }
    }
}
