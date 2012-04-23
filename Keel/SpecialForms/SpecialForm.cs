namespace Keel.SpecialForms
{
    using Keel.Objects;

    public abstract class SpecialForm
    {
        private readonly string name;
        
        protected SpecialForm(string name)
        {
            this.name = name;
        }

        public string Name { get { return name; } }

        public abstract LispObject Eval(Cons body, LispEnvironment env);
    }
}
