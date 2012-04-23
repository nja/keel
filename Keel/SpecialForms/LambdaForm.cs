namespace Keel.SpecialForms
{
    using Keel.Objects;

    public class LambdaForm : SpecialForm
    {
        public LambdaForm()
            : base("LAMBDA")
        { }

        public override LispObject Eval(Cons body, LispEnvironment env)
        {
            var lambdaList = body.Car.As<Cons>();
            var progn = Progn.Wrap(body.Cdr.As<Cons>(), env);

            return new Lambda(lambdaList, progn, env);
        }
    }
}
