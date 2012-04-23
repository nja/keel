namespace Keel.Builtins
{
    using Keel.Objects;

    public class Macro : LispObject
    {
        private readonly string name;
        private readonly Lambda expander;
        private readonly LispEnvironment macroEnv;

        public Macro(string name, LispObject lambdaList, Cons macroBody, LispEnvironment macroEnv)
        {
            this.name = name;
            this.expander = new Lambda(lambdaList, macroBody, macroEnv);
            this.macroEnv = macroEnv;
        }

        public Cons Expand(Cons form, LispEnvironment env)
        {
            var argumentValues = form.Cdr.As<Cons>();
            return this.expander.Apply(argumentValues, this.macroEnv).As<Cons>();
        }

        public override string ToString()
        {
            return string.Format("<Macro {0}>", this.name);
        }
    }
}