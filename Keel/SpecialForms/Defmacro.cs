namespace Keel.SpecialForms
{
    using Keel.Builtins;
    using Keel.Objects;

    public class Defmacro : SpecialForm
    {
        public Defmacro()
            : base("DEFMACRO")
        { }

        public override LispObject Eval(Cons defmacroBody, LispEnvironment env)
        {
            var symbol = defmacroBody.Car.As<Symbol>();
            var lambdaList = Car.Of(Cdr.Of(defmacroBody));
            var macroBody = Cdr.Of(Cdr.Of(defmacroBody)).As<Cons>();
            var progn = Progn.Wrap(macroBody, env);

            var macro = new Macro(symbol.Name, lambdaList, progn, env);

            env.AddBinding(symbol, macro);

            return macro;
        }
    }
}
