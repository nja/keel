namespace Keel.SpecialForms
{
    using Keel.Builtins;
    using Keel.Objects;

    public class Defun : SpecialForm
    {
        public Defun()
            : base("DEFUN")
        { }

        public override LispObject Eval(Cons defunBody, LispEnvironment env)
        {
            var symbol = Car.Of(defunBody).As<Symbol>();
            var lambdaList = Car.Of(Cdr.Of(defunBody));
            var lambdaBody = Cdr.Of(Cdr.Of(defunBody)).As<Cons>();
            var progn = Progn.Wrap(lambdaBody, env);

            var lambda = new Lambda(symbol.Name, lambdaList, progn, env);

            env.AddBinding(symbol, lambda);

            return lambda;
        }
    }
}
